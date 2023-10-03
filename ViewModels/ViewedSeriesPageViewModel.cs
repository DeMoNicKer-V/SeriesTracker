using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.ViewModels
{
    public partial class ViewedSeriesPageViewModel : BaseSeriesModel
    {
        public ViewedSeriesPageViewModel(INavigation navigation)
        {
            seriesList = new ObservableCollection<Series>();
            Navigation = navigation;
        }

        public List<Color> colorList { get; } = new List<Color>() {
        Color.FromArgb("#42A5F5"),
        Color.FromArgb("#EC407A"),
        Color.FromArgb("#26C6DA"),
        Color.FromArgb("#AB47BC"),
        Color.FromArgb("#EF5350"),
        Color.FromArgb("#D59E17"),
        Color.FromArgb("#FFA726"),
        Color.FromArgb("#66BB6A"),
        Color.FromArgb("#8D6E63"),
        Color.FromArgb("#78909C"),
        Color.FromArgb("#26A69A"),
        Color.FromArgb("#5C6BC0"),
        Color.FromArgb("#7E57C2"),
        Color.FromArgb("#FF7043"),
        Color.FromArgb("#00C853"),
        Color.FromArgb("#8BC34A"),
        Color.FromArgb("#FF8A80"),
        Color.FromArgb("#FF80AB") };

        private int randomNumber;

        public Color GetRandomColor
        {
            get
            {
                randomNumber = new Random().Next(0, 18);
                return colorList[randomNumber];
            }
        }

        public ObservableCollection<Series> seriesList
        {
            get;
        }

        public int ActionIndex(string action)
        {
            switch (action)
            {
                case "Удалить": return 0;
                case "Delete": return 0;
                case "Возобновить просмотр": return 1;
                case "Resume viewing": return 1;
                case "Редактировать": return 2;
                case "Edit": return 2;
            }
            return -1;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        [RelayCommand]
        private async void AdditionalAction(Series series)
        {
            string action = await Shell.Current.DisplayActionSheet(series.seriesName, "Закрыть", "Удалить", "Возобновить просмотр", "Редактировать");
            if (action != null)
            {
                switch (ActionIndex(action))
                {
                    case 0: await App.SeriesService.DeleteSeriesAsync(series.seriesId); OnAppearing(); return;
                    case 1: series.isOver = false; await App.SeriesService.AddUpdateSeriesAsync(series); OnAppearing(); return;
                    case 2: await Shell.Current.GoToAsync(nameof(NewSeriesPage)); return;
                }
            }

            return;
        }

        [RelayCommand]
        private async Task LoadSeries()
        {
            IsBusy = true;
            try
            {
                seriesList.Clear();
                var newSeriesList = await App.SeriesService.GetSeriesAsync(true);
                foreach (var item in newSeriesList)
                {
                    seriesList.Add(item);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async void OnDecEpisode(Series series)
        {
            if (series == null | series.currentEpisode == series.startEpisode)
            {
                await Shell.Current.DisplayAlert("Текущий объект", ":(", "Ok");
                return;
            }
            series.currentEpisode -= 1;
            await App.SeriesService.AddUpdateSeriesAsync(series);
            OnAppearing();
        }

        [RelayCommand]
        private async void OnIncEpisode(Series series)
        {
            if (series == null | series.currentEpisode == series.lastEpisode)
            {
                await Shell.Current.DisplayAlert("Текущий объект", ":(", "Ok");
                return;
            }
            series.currentEpisode += 1;
            await App.SeriesService.AddUpdateSeriesAsync(series);
            OnAppearing();
        }
    }
}
