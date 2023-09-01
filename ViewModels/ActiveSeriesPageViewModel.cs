using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Views;

namespace SeriesTracker.ViewModels;
public partial class ActiveSeriesPageViewModel: BaseSeriesModel
{
    public ObservableCollection<Series> seriesList
    {
        get;
    }

    public ActiveSeriesPageViewModel(INavigation navigation)
    {
        seriesList = new ObservableCollection<Series>();
        Navigation = navigation;
    }

    [RelayCommand]
    private async Task LoadSeries()
    {
        IsBusy = true;
        try
        {
            seriesList.Clear();
            var newSeriesList = await App.SeriesService.GetSeriesAsync();
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
    public void OnAppearing()
    {
        IsBusy = true;
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
    private async void OnAddSeries()
    {
        await Shell.Current.GoToAsync(nameof(NewSeriesPage));
    }
}
