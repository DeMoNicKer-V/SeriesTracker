using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;

#if ANDROID

using static Android.Media.MediaRouter;

#endif

namespace SeriesTracker.ViewModels
{
    public partial class DetailSeriesPageViewModel : BaseSeriesModel
    {
        public Command ChangeRatingCommand { get; }

        public DetailSeriesPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            DeleteCommand = new Command(OnDetailDeleteCommand);
            DetachCommand = new Command(OnDetailDetachCommand);
            ChangeRatingCommand = new Command(OnChangeRating);
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        [RelayCommand]
        public async Task ChangeFavoriteStatus()
        {
            if (!CheckSeries(Series)) { return; }
            Series.isFavourite = !Series.isFavourite;
            await App.SeriesService.AddUpdateSeriesAsync(Series);
        }

        public async void OnChangeRating()
        {
            if (!CheckSeries(Series)) { return; }
            await App.SeriesService.AddUpdateSeriesAsync(Series);
        }

        [RelayCommand]
        public async Task EditEpisode(string Text)
        {
            if (!CheckSeries(Series)) { return; }
            if (Convert.ToInt32(Text) > Series.lastEpisode || Convert.ToInt32(Text) < 0)
            {
                return;
            }
            Series.currentEpisode = Convert.ToInt32(Text);
            await App.SeriesService.AddUpdateSeriesAsync(Series);
        }

        private async void OnDetailDeleteCommand() 
        {
            OnDeleteCommand();
            await Shell.Current.GoToAsync("..");
        }
        private async void OnDetailDetachCommand()
        {
            OnDetachCommand();
            await Shell.Current.GoToAsync("..");
        }
    }
}