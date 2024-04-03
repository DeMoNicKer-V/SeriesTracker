using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GraphQL;
using SeriesTracker.Classes;
using SeriesTracker.Classes.Shikimori;
using SeriesTracker.Models;
using SeriesTracker.Services.MALBase;
using SeriesTracker.Services.ShikimoriBase;
using SeriesTracker.Services.SyncJournal;
using SeriesTracker.Views;
using System.Collections.ObjectModel;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker.ViewModels
{
    public partial class SeriesListPageViewModel : BaseSeriesModel
    {
        [ObservableProperty]
        public int currentPage;

        private int offSet;

        public int OffSet
        {
            get => offSet;
            set
            {
                offSet = value;
                if (offSet < 0) { offSet = 0; }
            }
        }

        public string RequestText { get; set; } = string.Empty;
        private readonly MALBase MALBase;
        private readonly ShikimoriBase ShikimoriBase;
        public ObservableCollection<AnimeBase> SeriesList { get; set; } = new ObservableCollection<AnimeBase>();
        private static readonly string DateNow = DateTime.Now.ToString();

        public SeriesListPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ShikimoriBase = new ShikimoriBase();
            MALBase = new MALBase();
            Series = new Series();
            CurrentPage = 1;
            OffSet = 0;
        }

        [RelayCommand]
        public async Task AddSeries(AnimeBase anime)
        {
            if (Series is null) return;
            Series.seriesName = anime.Title.TrimEnd();
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
            Series.SyncUid = Series.hiddenSeriesName.GetHashCode();
            Series.addedDate = DateNow;
            new Journal(new AddUpdateItem(Series.SyncUid, Series.SyncUid)).JournalToJson();

            Series.ChangedDate = DateNow;
            if (await App.SeriesService.AddUpdateSeriesAsync(Series) == false) { await ShowToast("Запись с таким названием уже есть в БД"); }
            else await Shell.Current.GoToAsync("..//..//..");
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
                    var graphQLResponse = new GraphQLResponse<ShikimoriAnimeList>();
                    if (string.IsNullOrEmpty(RequestText))
                    {
                        graphQLResponse = await ShikimoriBase.GetAnimes(CurrentPage);
                    }
                    else { graphQLResponse = await ShikimoriBase.GetAnimesByName(CurrentPage, RequestText); }
                    var shikimoriAnimes = graphQLResponse.Data.Animes;
                    if (graphQLResponse.Data.Animes.Any())
                    {
                        foreach (var item in shikimoriAnimes)
                        {
                            SeriesList.Add(item);
                        }
                    }
                }
                else
                {
                    var request = new MALRequest { Limit = 5, Offset = OffSet, Search = RequestText };
                    var result = await MALBase.GetAnimes(request);
                    var malAnimes = result.Animes;
                    if (malAnimes.Any())
                    {
                        foreach (var item in malAnimes)
                        {
                            SeriesList.Add(item.Node);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await ShowErrorAlert(Shell.Current, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task LoadSeriesByname(string query)
        {
            if (string.IsNullOrEmpty(query)) return;
            try
            {
                RequestText = new string(query.ToLower());
                CurrentPage = 1;
                OffSet = 0;
                await OnAppearing();
            }
            catch (Exception ex)
            {
                await ShowErrorAlert(Shell.Current, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task OnDecSeriesList()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await OnAppearing();
            }
            OffSet -= 5;
            await OnAppearing();
        }

        [RelayCommand]
        private async Task OnIncSeriesList()
        {
            CurrentPage++;
            OffSet += 5;
            await OnAppearing();
        }
    }
}