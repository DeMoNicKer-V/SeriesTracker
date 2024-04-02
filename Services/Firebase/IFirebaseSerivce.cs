using SeriesTracker.Models;

namespace SeriesTracker.Services.Firebase
{
    internal interface IFirebaseService
    {
        /// <summary>
        /// Add <see cref="Series"/> element to FireBase database.
        /// (Needs Internet connection)
        /// </summary>
        /// <returns></returns>
        Task<bool> AddUpdateSeriesAsync(Series series);

        /// <summary>
        /// Delete all <see cref="Series"/> elements from FireBase database.
        /// (Needs Internet connection)
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteAll();

        /// <summary>
        /// Delete <see cref="Series"/> element from FireBase database by Id.
        /// (Needs Internet connection)
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteSeriesAsync(int seriesId);

        /// <summary>
        /// Upload FireBase cloud database data to SQLite database.
        /// (Needs Internet Connection)
        /// </summary>
        /// <returns></returns>
        Task InSynchronize();

        /// <summary>
        /// Upload database data to FireBase cloud database.
        /// (Needs Internet Connection)
        /// </summary>
        /// <returns></returns>
        Task OutSynchronize();
    }
}