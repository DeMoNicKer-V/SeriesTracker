using SeriesTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.Firebase
{
    interface IFirebaseService
    {
        Task<bool> AddUpdateSeriesAsync(Series series);
       /* Task<bool> DeleteSeriesAsync(int seriesId);
        Task<Series> GetSeriesAsyncById(int seriesId);
        Task<IEnumerable<Series>> GetSeriesListAsync();
        Task<int> GetAllSeriesCount();*/
    }
}
