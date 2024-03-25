using Firebase.Database;
using Firebase.Database.Query;
using SeriesTracker.Models;
using SeriesTracker.Services.SyncJournal;

namespace SeriesTracker.Services.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        public FirebaseService(FirebaseSettings configuration)
        {
            ActiveFirebaseClient = new FirebaseClient(
                configuration.BaseUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(configuration.AppSecret)
                });
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

        private FirebaseClient ActiveFirebaseClient { get; set; }

        public async Task<bool> DeleteAll()
        {
            await ActiveFirebaseClient.Child("Series").DeleteAsync();
            return await Task.FromResult(true);
        }

        public async Task<bool> AddUpdateSeriesAsync(Series series)
        {
            if (series == null) return await Task.FromResult(false);

            await ActiveFirebaseClient.Child("Series").Child(series.SyncUid.ToString()).PutAsync(series);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteSeriesAsync(int Id)
        {
            await ActiveFirebaseClient.Child("Series").Child(Id.ToString()).DeleteAsync();
            return await Task.FromResult(true);
        }

        public async Task<bool> AddJournal(Journal journal)
        {
            await ActiveFirebaseClient.Child("SyncJournal").DeleteAsync();
            await ActiveFirebaseClient.Child("SyncJournal").PutAsync(journal);
            return await Task.FromResult(true);
        }

        public async Task OutSynchronize()
        {
            await App.SeriesService.OutSeriesAsyncSynchonize();
        }
        public async Task InSynchronize()
        {
            var collection = await GetSeriesAsync();
            await App.SeriesService.InSeriesAsyncSynchonize(collection);
        }
    }
}