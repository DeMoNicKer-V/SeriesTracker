using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Services.SyncJournal;
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
            if (Series.SyncUid == 0)
            {
                Series.SyncUid = (Series.hiddenSeriesName.GetHashCode());
                Series.addedDate = DateNow;
                new Journal(new AddUpdateItem(Series.SyncUid, Series.SyncUid)).JournalToJson();
            }
            else
            {
                new Journal(new AddUpdateItem(Series.hiddenSeriesName.GetHashCode(), Series.SyncUid), new DeleteItem(Series.SyncUid)).JournalToJson();
            }
            Series.ChangedDate = DateNow;

            if (await App.SeriesService.AddUpdateSeriesAsync(Series) == false) { await ShowToast("Запись с таким названием уже есть в БД"); }
            else await Shell.Current.GoToAsync("..//..");
        }
    }
}