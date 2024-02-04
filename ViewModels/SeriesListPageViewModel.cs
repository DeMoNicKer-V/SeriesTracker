using CommunityToolkit.Mvvm.Input;
using GraphQL;
using SeriesTracker.Classes.Shikimori;
using SeriesTracker.Services.ShikimoriBase;
using System.Collections.ObjectModel;

namespace SeriesTracker.ViewModels
{
    public partial class SeriesListPageViewModel : BaseSeriesModel
    {
        public string quaryText;
        private static int currentPage;
        private ObservableCollection<Anime> seriesList = new ObservableCollection<Anime>();
        private ShikimoriBase ShikimoriBase;
        public SeriesListPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ShikimoriBase = new ShikimoriBase();
        }

        public ObservableCollection<Anime> SeriesList
        {
            get
            {
                return seriesList;
            }
            set
            {
                seriesList = value;
                OnPropertyChanged();
            }
        }

        public async void OnAppearing()
        {
            SeriesList.Clear();
            currentPage = 1;
        }

        [RelayCommand]
        private async Task LoadSeries()
        {
            IsBusy = true;
            try
            {
                var graphQLResponse = new GraphQLResponse<AnimeList>();
                if (string.IsNullOrEmpty(quaryText))
                {
                    graphQLResponse = await ShikimoriBase.GetAnimes(currentPage);
                }
                else { graphQLResponse = await ShikimoriBase.GetAnimesByName(currentPage, quaryText); }
                var shikimoriAnimes = graphQLResponse.Data.Animes;
                if (shikimoriAnimes != null && shikimoriAnimes.Count() > 0)
                {
                    foreach (var item in shikimoriAnimes)
                    {
                        SeriesList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                currentPage = currentPage + 1;
                IsBusy = false;
            }
        }
        [RelayCommand]
        private void LoadSeriesByname(string query)
        {
            quaryText = new string(query.ToLower());
            OnAppearing();
        }
    }
}