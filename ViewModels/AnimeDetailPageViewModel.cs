using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Classes;
using SeriesTracker.Models;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;


namespace SeriesTracker.ViewModels
{
    public partial class AnimeDetailPageViewModel : BaseSeriesModel
    {
        public AnimeDetailPageViewModel(INavigation navigation)
        { 
            Navigation = navigation;
            Series = new Series();
        }

        [RelayCommand]
        public async Task AddSeries(AnimeBase anime)
        {
            if (Series is null) return;
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
            if (await App.SeriesService.AddUpdateSeriesAsync(Series) == false) { await ShowToast("Запись с таким названием уже есть в БД"); }
            else await Shell.Current.GoToAsync("..//..//..");
        }
    }
}