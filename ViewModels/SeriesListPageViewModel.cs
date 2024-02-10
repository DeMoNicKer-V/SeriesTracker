using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using GraphQL;
using SeriesTracker.Classes;
using SeriesTracker.Classes.Shikimori;
using SeriesTracker.Models;
using SeriesTracker.Services.MALBase;
using SeriesTracker.Services.ShikimoriBase;
using SeriesTracker.Views;
using System.Collections.ObjectModel;

namespace SeriesTracker.ViewModels
{
    public partial class SeriesListPageViewModel : BaseSeriesModel
    {
        public string quaryText;
        public bool AnimeWhat = false;
        private static int currentPage;
        private ObservableCollection<AnimeBase> seriesList = new ObservableCollection<AnimeBase>();
        private ShikimoriBase ShikimoriBase;
        private MALBase MALBase;
        public Command BackCommand { get; }
        public SeriesListPageViewModel(INavigation navigation, bool what)
        {
            Navigation = navigation;
            AnimeWhat = what;
            BackCommand = new Command(OnBackCommand);
            ShikimoriBase = new ShikimoriBase();
            MALBase = new MALBase();
            Series = new Series();
        }

        public ObservableCollection<AnimeBase> SeriesList
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
                if (AnimeWhat == false)
                {
                    var graphQLResponse = new GraphQLResponse<AnimeList<Anime>>();
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

                else
                {
                    var request = new MALRequest { Limit = 10, Offset=0, Search = quaryText };
                    var result = await MALBase.GetAnimes(request);
                    var shikimoriAnimes = result.Animes;
                    if (shikimoriAnimes != null && shikimoriAnimes.Count() > 0)
                    {
                        foreach (var item in shikimoriAnimes)
                        {
                            SeriesList.Add(item.Node);
                        }
                    }
                }
            }
            catch (Exception e )
            {
                var a = e.Message;
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

        [RelayCommand]
        private async Task DetailView(AnimeBase anime)
        {
            await Navigation.PushAsync(new AnimeDetailPage(anime));
        }

        [RelayCommand]
        public async Task AddSeries(AnimeBase anime)
        {
            var newSeries = Series;
            var _anime = anime;
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
                await SeriesListPageViewModel.ShowToast(errorMessage);
                return;
            }
         
            newSeries.hiddenSeriesName = newSeries.seriesName.ToLower();
            newSeries.addedDate = DateTime.Now.ToString();
            await App.SeriesService.AddUpdateSeriesAsync(newSeries);

            await Shell.Current.GoToAsync("..//..");
        }
        private static async Task ShowToast(string text)
        {

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var toast = Toast.Make(text, ToastDuration.Short, 14);

            await toast.Show(cancellationTokenSource.Token);
        }
        private async void OnBackCommand()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}