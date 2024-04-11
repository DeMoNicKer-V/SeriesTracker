using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker.ViewModels
{
    public partial class NewSeriesPageViewModel : BaseSeriesModel
    {
        private static readonly string DateNow = DateTime.Now.ToString();

        public NewSeriesPageViewModel()
        {
            Series = new Series();
        }

        [RelayCommand]
        public async Task SaveSeries()
        {
            var (isValid, errorMessage) = Series.Validate();
            if (isValid == false)
            {
                await ShowToast(errorMessage);
                return;
            }
            Series.hiddenSeriesName = Series.seriesName.ToLower().TrimEnd();
            Series.seriesDescription = string.IsNullOrEmpty(Series.seriesDescription) ? null : Series.seriesDescription;
            Series.addedDate = string.IsNullOrEmpty(Series.addedDate) ? DateNow : Series.addedDate;
            Series.ChangedDate = DateNow;

            if (await App.SeriesService.AddUpdateSeriesAsync(Series) == false) { await ShowToast("Запись с таким названием уже есть в БД"); }
            else await Shell.Current.GoToAsync("..//..");
        }
    }
}