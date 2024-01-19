using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeriesTracker.Models;

namespace SeriesTracker.Services;
internal interface ISeriesRepository
{
    Task<bool> AddUpdateSeriesAsync(Series series);
    Task<bool> DeleteSeriesAsync(int seriesId);
    Task<Series> GetSeriesAsyncById(int seriesId);
    Task<IEnumerable<Series>> GetSeriesAsync();
    Task<IEnumerable<Series>> GetAllSeriesAsync();
    Task<int> GetAllSeriesCount(bool overFlag);
}
