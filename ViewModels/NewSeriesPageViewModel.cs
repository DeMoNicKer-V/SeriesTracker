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
        public async void SaveSeries()
        {
            var newSeries = Series;
            if (newSeries is null)
                return;

            var (isValid, errorMessage) = newSeries.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Ошибка ввода данных", errorMessage, "Ok");
                return;
            }
            newSeries.addedDate = DateTime.Now.ToString();
            await App.SeriesService.AddUpdateSeriesAsync(newSeries);

            await Shell.Current.GoToAsync("..");
        }
        private async void OnBackCommand()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
