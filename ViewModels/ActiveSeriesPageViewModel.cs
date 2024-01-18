using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Controls.PopUp;
using SeriesTracker.Models;
using SeriesTracker.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SeriesTracker.ViewModels;

public partial class ActiveSeriesPageViewModel : BaseSeriesModel
{
    private readonly ContentPage _page;
    private ActivePagePopUp activeSeriesPagePopUp;
    private Series currentSeries;

    public Command DeleteCommand { get; }

    public Command DetachCommand { get; }

    public Command EditCommand { get; }

    public ActiveSeriesPageViewModel(INavigation navigation, ContentPage contentPageBehavior)
    {
        Navigation = navigation;
        _page = contentPageBehavior;
 
    }
    [ObservableProperty]
    public int seriesCount;

    [ObservableProperty]
    public int viewedSeriesCount;

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

    private async void OnDeleteCommand()
    {

        if (currentSeries is null)
            return;
        await App.SeriesService.DeleteSeriesAsync(currentSeries.seriesId);
        OnAppearing();
    }

    private async void OnDetachCommand()
    {

        if (currentSeries is null)
            return;

        currentSeries.isOver = true;
        currentSeries.currentEpisode = currentSeries.lastEpisode;
        currentSeries.overDate = DateTime.Now.ToString();
        
        await App.SeriesService.AddUpdateSeriesAsync(currentSeries);
        OnAppearing();
    }

    private async void OnEditCommand()
    {
        await Navigation.PushAsync(new NewSeriesPage(currentSeries));
    }
    public void OnAppearing()
    {
        IsBusy = true;
    }

    [RelayCommand]
    private async void AdditionalAction(Series series)
    {
        activeSeriesPagePopUp = new ActivePagePopUp();
        activeSeriesPagePopUp.ContentPageBehavior = _page;
        activeSeriesPagePopUp.Title = series.seriesName;
        currentSeries = series;
        activeSeriesPagePopUp.EditCommand = new Command(OnEditCommand);
        activeSeriesPagePopUp.DeleteCommand = new Command(OnDeleteCommand);
        activeSeriesPagePopUp.DetachCommand = new Command(OnDetachCommand);
        await _page.ShowPopupAsync(activeSeriesPagePopUp);
        return;
    }

    [RelayCommand]
    private async Task DetailView(Series series)
    {
        await Navigation.PushAsync(new DetailSeriesPage(series));
    }
    private int skip = 0;
    [RelayCommand]
    private async Task LoadSeries()
    {
            IsBusy = true;
        try
        {
            FilterList.Clear();
            SeriesList.Clear();
            SeriesCount = await App.SeriesService.GetAllSeriesCount(false);
            var newSeriesList = await App.SeriesService.Test(false, skip);
            newSeriesList = newSeriesList.OrderByDescending(f => f.isFavourite);
            if (newSeriesList != null && newSeriesList.Count() > 0)
            {
                foreach (var item in newSeriesList)
                {
                    SeriesList.Add(item);
                }
            }
            ViewedSeriesCount = SeriesList.Count() + skip;
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
    private async void OnIncSeriesList()
    {
        if ((SeriesList.Count() + skip)  < SeriesCount)
        {
            skip += 5;
            IsBusy = true;
        }

    }

    [RelayCommand]
    private async void OnDecSeriesList()
    {
        if (skip >= 5)
        {
            skip -= 5;
            IsBusy = true;
        }
        if (skip < 0) { IsBusy = true; skip = 0; }
      
    }

    [RelayCommand]
    private async void OnAddSeries()
    {
        await Shell.Current.GoToAsync(nameof(NewSeriesPage));
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
}