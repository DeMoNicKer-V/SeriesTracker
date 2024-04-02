using Firebase.Database;
using Firebase.Database.Query;
using SeriesTracker.Models;

namespace SeriesTracker.Services.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        private FirebaseClient ActiveFirebaseClient { get; set; }
        public FirebaseService(FirebaseSettings configuration)
        {
            ActiveFirebaseClient = new FirebaseClient(
                configuration.BaseUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(configuration.AppSecret)
                });
        }

        public async Task<bool> AddUpdateSeriesAsync(Series series)
        {
            if (series == null) return await Task.FromResult(false);
            await ActiveFirebaseClient.Child("Series").Child(series.SyncUid.ToString()).PutAsync(series);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAll()
        {
            await ActiveFirebaseClient.Child("Series").DeleteAsync();
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteSeriesAsync(int seriesId)
        {
            await ActiveFirebaseClient.Child("Series").Child(seriesId.ToString()).DeleteAsync();
            return await Task.FromResult(true);
        }

        public async Task InSynchronize()
        {
            var collection = await GetSeriesAsync();
            await App.SeriesService.InSeriesAsyncSynchonize(collection);
        }

        public async Task OutSynchronize()
        {
            await App.SeriesService.OutSeriesAsyncSynchonize();
        }

        private async Task<List<Series>> GetSeriesAsync()
        {
            var collection = await ActiveFirebaseClient.Child("Series").OnceAsync<Series>();
            var seriesList = new List<Series>();
            foreach (var item in collection)
            {
                item.Object.seriesId = 0;
                seriesList.Add(item.Object);
            }
            return seriesList;
        }
    }
}