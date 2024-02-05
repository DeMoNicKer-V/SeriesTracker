using SeriesTracker.Classes.Shikimori;


namespace SeriesTracker.ViewModels
{
    public partial class AnimeDetailPageViewModel : BaseSeriesModel
    {
        public AnimeDetailPageViewModel(INavigation navigation)
        { 
            Navigation = navigation;
            Anime = new Anime();
            Anime.poster = new Poster();
            Anime.airedOne = new AiredDate();
            BackCommand = new Command(OnBackCommand); 
        }

        public Command BackCommand { get; }
        private async void OnBackCommand()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}