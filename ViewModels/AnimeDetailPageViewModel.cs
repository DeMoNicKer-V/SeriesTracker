using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Classes.Shikimori;


namespace SeriesTracker.ViewModels
{
    public partial class AnimeDetailPageViewModel : BaseSeriesModel
    {
        public AnimeDetailPageViewModel(INavigation navigation)
        { 
            Navigation = navigation;
            Anime = new Anime();
    
            Series = new Models.Series();
            BackCommand = new Command(OnBackCommand); 
        }

        public Command BackCommand { get; }
        private async void OnBackCommand()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task AddSeries(Anime anime)
        {
            var newSeries = Series;
            if (newSeries is null)
                return;
            newSeries.seriesName = anime.Title;
            newSeries.seriesDescription = anime.Description;
            newSeries.imagePath = anime.PictureUrl;
            newSeries.lastEpisode = anime.Episodes;
            newSeries.releaseDate = DateTime.Parse(anime.StartDate);
            var (isValid, errorMessage) = newSeries.Validate();
            if (!isValid)
            {
                await AnimeDetailPageViewModel.ShowToast(errorMessage);
                return;
            }

            newSeries.hiddenSeriesName = newSeries.seriesName.ToLower();
            newSeries.addedDate = DateTime.Now.ToString();
            await App.SeriesService.AddUpdateSeriesAsync(newSeries);

            await Shell.Current.GoToAsync("..//..//..");
        }
        private static async Task ShowToast(string text)
        {

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var toast = Toast.Make(text, ToastDuration.Short, 14);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}