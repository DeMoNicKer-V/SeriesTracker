using SeriesTracker.Models;
using SeriesTracker.Services.SyncJournal;
using SQLite;
using static SeriesTracker.Services.Extensions.SeriesEqualExtension;

namespace SeriesTracker.Services;

public class SeriesService : ISeriesRepository
{
    public SQLiteAsyncConnection _database;
    public int relativeItemsCount;

    public SeriesService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Series>().Wait();
    }

    /// <summary>
    /// Add or update an <see cref="Series"/> element to database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<bool> AddUpdateSeriesAsync(Series series)
    {
        switch (series.seriesId)
        {
            case > 0:
                await _database.UpdateAsync(series);
                break;

            case <= 0:
                if (await _database.Table<Series>().Where(n => n.hiddenSeriesName == series.hiddenSeriesName).FirstOrDefaultAsync() != null) return await Task.FromResult(false);
                await _database.InsertAsync(series);
                break;
        }
        return await Task.FromResult(true);
    }

    public async Task<bool> OutSeriesAsyncSynchonize()
    {
        var Action = new Journal().GetJournal();
        if (Action == null)
        {
            return await Task.FromResult(false);
        }
        foreach (var item in Action.DeleteItems)
        {
            await App.FirebaseService.DeleteSeriesAsync(item.Id);
        }
        foreach (var item in Action.UpdateItems)
        {
            var series = await _database.Table<Series>().Where(n => n.SyncUid == item.PrevId).FirstOrDefaultAsync();
            if (series != null)
            {
                if (!series.hiddenSeriesName.GetHashCode().Equals(series.SyncUid))
                {
                    series.SyncUid = series.hiddenSeriesName.GetHashCode();
                    await _database.UpdateAsync(series);
                }
                await App.FirebaseService.AddUpdateSeriesAsync(series);
            }
        }
        //await App.FirebaseService.AddJournal(Action);
        return await Task.FromResult(true);
    }

    public async Task<bool> InSeriesAsyncSynchonize(IEnumerable<Series> syncSeriesList)
    {
        var allSeries = await _database.Table<Series>().ToListAsync();
        var seriesByName = syncSeriesList.Except(allSeries, new SeriesSyncComparer()).ToList();

        foreach (var item in seriesByName)
        {
            await AddUpdateSeriesAsync(item);
        }
        new Journal().UpdateJournal();
        return await Task.FromResult(true);
    }

    /// <summary>
    /// Delete an <see cref="Series"/> element by Id.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<bool> DeleteSeriesAsync(int seriesId)
    {
        await _database.DeleteAsync<Series>(seriesId);
        return await Task.FromResult(true);
    }

    /// <summary>
    /// Returns a <see cref="Series"/> element by Id.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<Series> GetSeriesAsyncById(int seriesId)
    {
        return await _database.Table<Series>().Where(n => n.seriesId == seriesId).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Returns an <see cref="Series"/> filtered by Series name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<Series> GetSeriesAsyncByName(string name)
    {
        return await _database.Table<Series>().Where(n => n.hiddenSeriesName == name).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Returns an <see cref="Series"/> list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Series>> GetSeriesAsync(bool overFlag, int skip, bool favorite)
    {
        switch (favorite)
        {
            case true:
                relativeItemsCount = await _database.Table<Series>().Where(s => s.isOver == overFlag & s.isFavourite == true).CountAsync();
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.isOver == overFlag & s.isFavourite == true).Skip(skip).Take(5).ToListAsync());

            case false:
                relativeItemsCount = await _database.Table<Series>().Where(s => s.isOver == overFlag).CountAsync();
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.isOver == overFlag).Skip(skip).Take(5).OrderByDescending(s => s.isFavourite).ToListAsync());
        }
    }

    /// <summary>
    /// Returns an <see cref="Series"/> list filtered by Series name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Series>> GetSeriesAsync(bool overFlag, int skip, string query, bool favorite)
    {
        switch (favorite)
        {
            case true:
                relativeItemsCount = await _database.Table<Series>().Where(s => s.isOver == overFlag & s.isFavourite == true).CountAsync();
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.isOver == overFlag & s.hiddenSeriesName.Contains(query) & s.isFavourite == true).Skip(skip).Take(5).ToListAsync());

            case false:
                relativeItemsCount = await _database.Table<Series>().Where(s => s.isOver == overFlag & s.hiddenSeriesName.Contains(query)).CountAsync();
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.hiddenSeriesName.Contains(query) & s.isOver == overFlag).Skip(skip).Take(5).OrderByDescending(s => s.isFavourite).ToListAsync());
        }
    }

    /// <summary>
    /// Returns an <see cref="Series"/> list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Series>> GetSeriesAsync()
    {
        return await Task.FromResult(await _database.Table<Series>().ToListAsync());
    }

    /// <summary>
    /// Returns a count of all <see cref="Series"/> elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<int> GetAllSeriesCount(bool overFlag)
    {
        return await Task.FromResult(await _database.Table<Series>().Where(s => s.isOver == overFlag).CountAsync());
    }
}