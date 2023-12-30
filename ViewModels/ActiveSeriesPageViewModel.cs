using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SeriesTracker.ViewModels;

public partial class ActiveSeriesPageViewModel : BaseSeriesModel
{

    public ActiveSeriesPageViewModel(INavigation navigation)
    {
        Navigation = navigation;
    }

    public string GetTitleDescription(Series series)
    {
        
            if (series.seriesSeason > 0)
            {
                return $"Сезон {series.seriesSeason}-й, год выхода - {series.releaseYear}";
            }
            return $"Год выхода - {series.releaseYear}";
        
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
            case "Закончить просмотр": return 1;
            case "Finish viewing": return 1;
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
        string action = await Shell.Current.DisplayActionSheet(series.seriesName, "Закрыть", "Удалить", "Закончить просмотр", "Редактировать");
        if (action != null | series != null)
        {
            switch (ActionIndex(action))
            {
                case 0: await App.SeriesService.DeleteSeriesAsync(series.seriesId); OnAppearing(); return;
                case 1: series.isOver = true; await App.SeriesService.AddUpdateSeriesAsync(series); OnAppearing(); return;
                case 2: await Navigation.PushAsync(new NewSeriesPage(series)); return;
            }
        }

        return;
    }

    [RelayCommand]
    private async Task DetailView(Series series)
    {
        await Navigation.PushAsync(new DetailSeriesPage(series));
    }

    [RelayCommand]
    private async Task LoadSeries()
    {
        IsBusy = true;
        try
        {
            FilterList.Clear();
            SeriesList.Clear();
            var newSeriesList = await App.SeriesService.GetSeriesAsync(false);
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

    [RelayCommand]
    private async void OnAddSeries()
    {
        await Shell.Current.GoToAsync(nameof(NewSeriesPage));
    }

    [RelayCommand]
    private async void OnDecEpisode(Series series)
    {
        if (series == null | series.currentEpisode == series.startEpisode)
        {
            return;
        }
        series.currentEpisode -= 1;
        await App.SeriesService.AddUpdateSeriesAsync(series);
        OnAppearing();
    }

    [RelayCommand]
    private async void SetFavourite(Series series)
    {
        if (series == null)
        {
            return;
        }
        series.isFavourite = !series.isFavourite;
        OnAppearing();
    }

    [RelayCommand]
    private async void OnIncEpisode(Series series)
    {
        if (series == null)
        {
            return;
        }
        else if (series.currentEpisode == series.lastEpisode)
        {
            bool action = await Shell.Current.DisplayAlert($"{series.seriesName} - конец?",
                "Похоже, что вы посмотрели все эпизоды. Пометить данный сериал как просмотренный?", "Да", "Нет");
            if (action) 
            { 
                series.isOver = true; 
                await App.SeriesService.AddUpdateSeriesAsync(series); 
                OnAppearing(); 
                return; 
            }
            return;
        }
        series.currentEpisode += 1;
        await App.SeriesService.AddUpdateSeriesAsync(series);
        OnAppearing();
    }
}