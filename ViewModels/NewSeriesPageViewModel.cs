using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Services.SyncJournal;

#if ANDROID

using static Android.Media.MediaRouter;

#endif

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
                Series.SyncUid = (Series.hiddenSeriesName.ToLower().GetHashCode());
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

        private static async Task ShowToast(string text)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make(text, ToastDuration.Short, 14);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
}