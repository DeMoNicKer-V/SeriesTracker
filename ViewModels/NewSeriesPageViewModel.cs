using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
#if ANDROID
using static Android.Media.MediaRouter;
#endif

namespace SeriesTracker.ViewModels
{
    public partial class NewSeriesPageViewModel : BaseSeriesModel
    {
        public Command BackCommand { get; }
        public NewSeriesPageViewModel()
        {
            BackCommand = new Command(OnBackCommand);
            Series = new Series();
        }

        [RelayCommand]
        public async Task SaveSeries()
        {
            var newSeries = Series;
            if (newSeries is null)
                return;

            var (isValid, errorMessage) = newSeries.Validate();
            if (!isValid)
            {
                await NewSeriesPageViewModel.ShowToast(errorMessage);
                return;
            }
            var old = await App.SeriesService.GetSeriesAsyncByName(newSeries.seriesName.ToLower());
            if (old != null) { await NewSeriesPageViewModel.ShowToast("Сериал со схожим названием уже есть в базе");
                return;
            }
            var date = DateTime.Now.ToString();
            newSeries.hiddenSeriesName = newSeries.seriesName.ToLower();
            newSeries.addedDate = newSeries.addedDate == null ? date : newSeries.addedDate;
            if (string.IsNullOrEmpty(newSeries.SyncUid)) { newSeries.SyncUid = (newSeries.seriesName.ToLower().GetHashCode() + date.GetHashCode()).ToString(); }
            newSeries.ChangedDate = date;
            await App.SeriesService.AddUpdateSeriesAsync(newSeries);
            //await App.FirebaseService.AddUpdateSeriesAsync(newSeries);
            await Shell.Current.GoToAsync("..//..");
        }

        private async void OnBackCommand()
        {
            await Shell.Current.GoToAsync("..");
        }

        private static async Task ShowToast(string text)
        {

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var toast = Toast.Make(text, ToastDuration.Short, 14);

            await toast.Show(cancellationTokenSource.Token);
        }
    }
}
