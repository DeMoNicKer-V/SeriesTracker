using SeriesTracker.Models;
using SeriesTracker.Views;

#if ANDROID

using static Android.Media.MediaRouter;

#endif

namespace SeriesTracker.ViewModels
{
    public partial class DetailSeriesPageViewModel : BaseSeriesModel
    {
        public Command CloseCommand { get; }

        public Command DeleteCommand { get; }

        public Command DetachCommand { get; }

        public Command EditCommand { get; }

        public Command BackCommand { get; }
        public DetailSeriesPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            BackCommand = new Command(OnBackCommand);
            EditCommand = new Command(OnEditCommand);
            DeleteCommand = new Command(OnDeleteCommand);
            DetachCommand = new Command(OnDetachCommand);
            Series = new Series();
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand]
        public async Task SaveSeries()
        {
            var newSeries = Series;
            if (newSeries is null)
                return;
            newSeries.isFavourite = !newSeries.isFavourite;
            await App.SeriesService.AddUpdateSeriesAsync(newSeries);
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand]
        public async Task EditEpisode(string Text)
        {
            var newSeries = Series;
            if (newSeries is null)
                return;
            if (Convert.ToInt32(Text) > newSeries.lastEpisode || Convert.ToInt32(Text) < newSeries.startEpisode)
            {
                return;
            }
            newSeries.currentEpisode = Convert.ToInt32(Text);
            await App.SeriesService.AddUpdateSeriesAsync(newSeries);
        }
            private async void OnDeleteCommand()
        {
            var newSeries = Series;
            if (newSeries is null)
                return;
            await App.SeriesService.DeleteSeriesAsync(newSeries.seriesId);
            await Shell.Current.GoToAsync("..");
        }

        private async void OnDetachCommand()
        {
            var newSeries = Series;
            if (newSeries is null)
                return;
            if (!newSeries.isOver)
            {
                newSeries.isOver = true;
                newSeries.currentEpisode = newSeries.lastEpisode;
                newSeries.overDate = DateTime.Now.ToString();
            }
            else
            {
                newSeries.isOver = false;
                newSeries.currentEpisode = newSeries.startEpisode;
                newSeries.overDate = string.Empty ;
            }
            await App.SeriesService.AddUpdateSeriesAsync(newSeries);
            await Shell.Current.GoToAsync("..");
        }

        private async void OnEditCommand()
        {
            await Navigation.PushAsync(new NewSeriesPage(Series));
        }

        private async void OnBackCommand() 
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}