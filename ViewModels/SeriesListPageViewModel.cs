using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
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
        public bool AnimeWhat = false;
        [ObservableProperty]
        public int currentPage;

        public int offset;
        public string quaryText;
        private MALBase MALBase;
        private ObservableCollection<AnimeBase> seriesList = new ObservableCollection<AnimeBase>();
        private ShikimoriBase ShikimoriBase;
        public SeriesListPageViewModel(INavigation navigation, bool what)
        {
            Navigation = navigation;
            AnimeWhat = what;
            BackCommand = new Command(OnBackCommand);
            ShikimoriBase = new ShikimoriBase();
            MALBase = new MALBase();
            Series = new Series();
            CurrentPage = 1;
            offset = 0;
        }

        public Command BackCommand { get; }
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

        public async Task OnAppearing()
        {
            SeriesList.Clear();
            await LoadSeries();
        }

        private static async Task ShowToast(string text)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var toast = Toast.Make(text, ToastDuration.Short, 14);

            await toast.Show(cancellationTokenSource.Token);
        }

        [RelayCommand]
        private async Task DetailView(AnimeBase anime)
        {
            await Navigation.PushAsync(new AnimeDetailPage(anime));
        }

        private async Task LoadSeries()
        {
            IsBusy = true;
            try
            {
                if (AnimeWhat == false)
                {
                    var graphQLResponse = new GraphQLResponse<AnimeList<ShikimoriAnime>>();
                    if (string.IsNullOrEmpty(quaryText))
                    {
                        graphQLResponse = await ShikimoriBase.GetAnimes(CurrentPage);
                    }
                    else { graphQLResponse = await ShikimoriBase.GetAnimesByName(CurrentPage, quaryText); }
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
                    var request = new MALRequest { Limit = 5, Offset = offset, Search = quaryText };
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
            catch (Exception e)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LoadSeriesByname(string query)
        {
            quaryText = new string(query.ToLower());
            CurrentPage = 1;
            offset = 0;
            await OnAppearing();
        }

        private async void OnBackCommand()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task OnDecSeriesList()
        {
            if (CurrentPage > 1)
            {
                CurrentPage = CurrentPage - 1;
                await OnAppearing();
            }
            if (offset >= 5)
            {
                offset = offset - 5;
                await OnAppearing();
            }
        }

        [RelayCommand]
        private async Task OnIncSeriesList()
        {
            CurrentPage = CurrentPage + 1;
            offset = offset + 5;
            await OnAppearing();
        }
    }
}