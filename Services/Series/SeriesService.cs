using SeriesTracker.Models;
using SQLite;
using static SeriesTracker.Services.Extensions.SeriesEqualExtension;

namespace SeriesTracker.Services;

public class SeriesService : ISeriesRepository
{
    public SQLiteAsyncConnection _database;
    public int AllDatabaseItemsCount;
    public int relativeItemsCount;
    public SeriesService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Series>().Wait();
    }

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

    public async Task<bool> DeleteAll()
    {
        await _database.DeleteAllAsync<Series>();
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteSeriesAsync(int seriesId)
    {
        await _database.DeleteAsync<Series>(seriesId);
        return await Task.FromResult(true);
    }

    public async Task<int> GetAllSeriesCount()
    {
        return await Task.FromResult(await _database.Table<Series>().CountAsync());
    }

    public async Task<int> GetAllSeriesCountByFlag(bool overFlag)
    {
        AllDatabaseItemsCount = await _database.Table<Series>().CountAsync();
        return await Task.FromResult(await _database.Table<Series>().Where(s => s.isOver == overFlag).CountAsync());
    }

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

    public async Task<IEnumerable<Series>> GetSeriesAsync(bool overFlag, int skip, string name, bool favorite)
    {
        switch (favorite)
        {
            case true:
                relativeItemsCount = await _database.Table<Series>().Where(s => s.isOver == overFlag & s.isFavourite == true).CountAsync();
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.isOver == overFlag & s.hiddenSeriesName.Contains(name) & s.isFavourite == true).Skip(skip).Take(5).ToListAsync());

            case false:
                relativeItemsCount = await _database.Table<Series>().Where(s => s.isOver == overFlag & s.hiddenSeriesName.Contains(name)).CountAsync();
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.hiddenSeriesName.Contains(name) & s.isOver == overFlag).Skip(skip).Take(5).OrderByDescending(s => s.isFavourite).ToListAsync());
        }
    }

    public async Task<IEnumerable<Series>> GetSeriesAsync()
    {
        return await Task.FromResult(await _database.Table<Series>().ToListAsync());
    }

    public async Task<Series> GetSeriesAsyncById(int seriesId)
    {
        return await _database.Table<Series>().Where(n => n.seriesId == seriesId).FirstOrDefaultAsync();
    }

    public async Task<Series> GetSeriesAsyncByName(string name)
    {
        return await _database.Table<Series>().Where(n => n.hiddenSeriesName == name).FirstOrDefaultAsync();
    }

}