using SeriesTracker.Models;

namespace SeriesTracker.Services;

internal interface ISeriesRepository
{
    /// <summary>
    /// Add or update an <see cref="Series"/> element to database.
    /// </summary>
    /// <returns></returns>
    Task<bool> AddUpdateSeriesAsync(Series series);

    /// <summary>
    /// Delete all <see cref="Series"/> elements from database.
    /// </summary>
    /// <returns></returns>
    Task<bool> DeleteAll();

    /// <summary>
    /// Delete an <see cref="Series"/> element by Id.
    /// </summary>
    /// <returns></returns>
    Task<bool> DeleteSeriesAsync(int seriesId);

    /// <summary>
    /// Returns a count of all <see cref="Series"/> elements.
    /// </summary>
    /// <returns></returns>
    Task<int> GetAllSeriesCount();

    /// <summary>
    /// Returns a count of all <see cref="Series"/> elements, filtered by OverFlag.
    /// </summary>
    /// <returns></returns>
    Task<int> GetAllSeriesCountByFlag(bool overFlag);
    /// <summary>
    /// Returns an <see cref="Series"/> list.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Series>> GetSeriesAsync();

    /// <summary>
    /// Returns an <see cref="Series"/> list filtered by Name, Over and Favorite flags.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Series>> GetSeriesAsync(bool overFlag, int skip, string name, bool favorite);

    /// <summary>
    /// Returns an <see cref="Series"/> list filtered by Over and Favorite flags.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Series>> GetSeriesAsync(bool overFlag, int skip, bool favorite);

    /// <summary>
    /// Returns a <see cref="Series"/> element by Id.
    /// </summary>
    /// <returns></returns>
    Task<Series> GetSeriesAsyncById(int seriesId);

    /// <summary>
    /// Returns an <see cref="Series"/> filtered by Series name.
    /// </summary>
    /// <returns></returns>
    Task<Series> GetSeriesAsyncByName(string name);

    /// <summary>
    /// Upload FireBase cloud database data to SQLite database.
    /// (Needs Internet Connection)
    /// </summary>
    /// <returns></returns>
    Task<bool> InSeriesAsyncSynchonize(IEnumerable<Series> syncSeriesList);

    /// <summary>
    /// Upload database data to FireBase cloud database.
    /// (Needs Internet Connection)
    /// </summary>
    /// <returns></returns>
    Task<bool> OutSeriesAsyncSynchonize();
}