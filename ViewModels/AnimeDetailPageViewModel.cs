using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Classes;
using SeriesTracker.Models;


namespace SeriesTracker.ViewModels
{
    public partial class AnimeDetailPageViewModel : BaseSeriesModel
    {
        private readonly SeriesListPageViewModel seriesListPageViewModel;
        public AnimeDetailPageViewModel(INavigation navigation)
        { 
            Navigation = navigation;
            Series = new Series();
            seriesListPageViewModel = new(navigation);
        }

        [RelayCommand]
        public async Task AddSeries(AnimeBase anime)
        {
            await seriesListPageViewModel.AddSeries(anime);
        }
    }
}