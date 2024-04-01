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
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker.ViewModels
{
    public partial class SeriesListPageViewModel : BaseSeriesModel
    {

        [ObservableProperty]
        public int currentPage;
        public int offset;
        private MALBase MALBase;
        private ShikimoriBase ShikimoriBase;
        public ObservableCollection<AnimeBase> SeriesList { get; set; } = new ObservableCollection<AnimeBase>();
        public SeriesListPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ShikimoriBase = new ShikimoriBase();
            MALBase = new MALBase();
            Series = new Series();
            CurrentPage = 1;
            offset = 0;
        }


        [RelayCommand]
        public async Task AddSeries(AnimeBase anime)
        {
            if (Series is null)  return;
            Series.seriesName = anime.Title;
            Series.seriesDescription = anime.Description;
            Series.imagePath = anime.PictureUrl;
            Series.lastEpisode = anime.Episodes;
            Series.releaseDate = DateTime.Parse(anime.StartDate);
            var (isValid, errorMessage) = Series.Validate();
            if (!isValid)
            {
                await ShowToast(errorMessage);
                return;
            }

            Series.hiddenSeriesName = Series.seriesName.ToLower();
            Series.addedDate = DateTime.Now.ToString();
            await App.SeriesService.AddUpdateSeriesAsync(Series);

            await Shell.Current.GoToAsync("..//..");
        }

        public async Task OnAppearing()
        {
            SeriesList.Clear();
            await LoadSeries();
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
                if (AnimeSourceSite == false)
                {
                    var graphQLResponse = new GraphQLResponse<AnimeList<ShikimoriAnime>>();
                    if (string.IsNullOrEmpty(QueryText))
                    {
                        graphQLResponse = await ShikimoriBase.GetAnimes(CurrentPage);
                    }
                    else { graphQLResponse = await ShikimoriBase.GetAnimesByName(CurrentPage, QueryText); }
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
                    var request = new MALRequest { Limit = 5, Offset = offset, Search = QueryText };
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
            QueryText = new string(query.ToLower());
            CurrentPage = 1;
            offset = 0;
            await OnAppearing();
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