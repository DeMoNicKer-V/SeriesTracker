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

        private ObservableCollection<Series> seriesList = new ObservableCollection<Series>();
        private ObservableCollection<Series> filterList = new ObservableCollection<Series>();

        public ObservableCollection<Series> FilterList
        {
            get
            {
                return filterList;
            }
            set
            {
                filterList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Series> SeriesList
        {
            get
            {
                return seriesList;
            }
            set
            {
                seriesList = value;
                OnPropertyChanged();
            }
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
        private async Task DetailView(Series series)
        {
            await Navigation.PushAsync(new DetailSeriesPage(series));
        }

        [RelayCommand]
        private async void AdditionalAction(Series series)
        {
            string action = await Shell.Current.DisplayActionSheet(series.seriesName, "Закрыть", "Удалить", "Возобновить просмотр", "Редактировать");
            if (action != null | series != null)
            {
                switch (ActionIndex(action))
                {
                    case 0: await App.SeriesService.DeleteSeriesAsync(series.seriesId); OnAppearing(); return;
                    case 1: series.isOver = false; series.currentEpisode = series.startEpisode; series.overDate = string.Empty; await App.SeriesService.AddUpdateSeriesAsync(series); OnAppearing(); return;
                    case 2: await Navigation.PushAsync(new NewSeriesPage(series)); return;
                }
            }

            return;
        }
        int skip = 0;
        [RelayCommand]
        private async Task LoadSeries()
        {
            IsBusy = true;
            try
            {
                FilterList.Clear();
                SeriesList.Clear();
                var newSeriesList = await App.SeriesService.GetSeriesAsync(true, skip, false);
                newSeriesList = newSeriesList.OrderByDescending(f => f.isFavourite);
                if (newSeriesList != null && newSeriesList.Count() > 0)
                {
                    foreach (var item in newSeriesList)
                    {
                        SeriesList.Add(item);
                    }
                }
                FilterList = SeriesList;
            }
            catch (Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
