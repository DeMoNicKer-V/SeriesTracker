using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeriesTracker.Models;
using SQLite;

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
    /// Adds an <see cref="Series"/> element to database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<bool> AddUpdateSeriesAsync(Series series)
    {
        if (series.seriesId > 0)
        {
            await _database.UpdateAsync(series);
        }
        else
        {
            await _database.InsertAsync(series);
        }
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
    /// Returns an <see cref="Series"/> list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Series>> GetSeriesAsync(bool overFlag, int skip, bool favorite)
    {
        relativeItemsCount = await _database.Table<Series>().Where(s => s.isOver == overFlag).CountAsync();
        switch (favorite)
        {
            case true:
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.isOver == overFlag & s.isFavourite == true).Skip(skip).Take(5).ToListAsync());
            case false:
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.isOver == overFlag).Skip(skip).Take(5).ToListAsync());

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
        relativeItemsCount = await _database.Table<Series>().Where(s => s.hiddenSeriesName.Contains(query)).CountAsync();
        switch (favorite)
        {
            case true:
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.hiddenSeriesName.Contains(query) & s.isFavourite == true).Skip(skip).Take(5).ToListAsync());
            case false:
                return await Task.FromResult(await _database.Table<Series>().Where(s => s.hiddenSeriesName.Contains(query) & s.isOver == overFlag).Skip(skip).Take(5).ToListAsync());

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
