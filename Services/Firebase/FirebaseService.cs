using Firebase.Database;
using Firebase.Database.Query;
using SeriesTracker.Models;

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

        private FirebaseClient ActiveFirebaseClient { get; set; }
        public async Task<bool> AddUpdateSeriesAsync(Series series)
        {
            if (series == null) return await Task.FromResult(false);
            await ActiveFirebaseClient.Child("Series").Child(series.seriesId.ToString()).PutAsync(series);
            return await Task.FromResult(true);
        }

        public async Task GetAll()
        {
            var collection = await ActiveFirebaseClient.Child("Series").OnceAsync<Series>();
            var seriesList = new List<Series>();
            foreach (var item in collection)
            {
                seriesList.Add(item.Object);
            }
            await App.SeriesService.AddUpdateSeriesAsyncSynchonize(seriesList);
        }
    }
}