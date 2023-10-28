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
    public SeriesService(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Series>().Wait();
    }

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

    public async Task<bool> DeleteSeriesAsync(int seriesId)
    {
        await _database.DeleteAsync<Series>(seriesId);
        return await Task.FromResult(true);
    }

    public async Task<Series> GetSeriesAsyncById(int seriesId)
    {
        return await _database.Table<Series>().Where(n => n.seriesId == seriesId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Series>> GetSeriesAsync(bool overFlag)
    {
        return await Task.FromResult(await _database.Table<Series>().Where(s => s.isOver == overFlag).ToListAsync());
    }

    public async Task<IEnumerable<Series>> GetAllSeriesAsync()
    {
        return await Task.FromResult(await _database.Table<Series>().ToListAsync());
    }
}
