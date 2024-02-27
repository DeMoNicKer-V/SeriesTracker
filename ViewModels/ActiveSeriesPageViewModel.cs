using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Controls.PopUp;
using SeriesTracker.Models;
using SeriesTracker.Services.Constant;
using SeriesTracker.Views;
using System.Collections.ObjectModel;
using static SeriesTracker.Services.Constant.Parameters;

namespace SeriesTracker.ViewModels;

public partial class ActiveSeriesPageViewModel : BaseSeriesModel
{
    public string queryText;

    [ObservableProperty]
    public int seriesCount;

    [ObservableProperty]
    public int viewedSeriesCount;

    internal bool favoriteFlag = false;
    private readonly ContentPage _page;
    private ActivePagePopUp activeSeriesPagePopUp;
    private Series currentSeries;

    private ObservableCollection<Series> seriesList = new ObservableCollection<Series>();
    private int skip = 0;

    public ActiveSeriesPageViewModel(INavigation navigation, ContentPage contentPageBehavior)
    {
        Navigation = navigation;
        _page = contentPageBehavior;
        activeSeriesPagePopUp = new ActivePagePopUp();
        activeSeriesPagePopUp.ContentPageBehavior = _page;
        activeSeriesPagePopUp.EditCommand = new Command(OnEditCommand);
        activeSeriesPagePopUp.DeleteCommand = new Command(OnDeleteCommand);
        activeSeriesPagePopUp.DetachCommand = new Command(OnDetachCommand);
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
        }
    }

    public void OnAppearing()
    {
        IsRefreshing = true;
        IsRefreshing = false;
        IsBusy = true;
    }

    [RelayCommand]
    private async Task AdditionalAction(Series series)
    {
        activeSeriesPagePopUp.Title = series.seriesName;
        currentSeries = series;
        await _page.ShowPopupAsync(activeSeriesPagePopUp);
        return;
    }

    [RelayCommand]
    private async Task DetailView(Series series)
    {
        await Navigation.PushAsync(new DetailSeriesPage(series));
    }

    [RelayCommand]
    private void FilterSeries(string query)
    {
        try
        {
            queryText = new string(query.ToLower());
            OnAppearing();
        }
        catch (Exception) { }
        finally { IsBusy = false; }
    }


    [RelayCommand]
    private async Task LoadSeries()
    {
        IsBusy = true;
        try
        {
            SeriesList.Clear();
            IEnumerable<Series> newSeriesList = new List<Series>();
            if (string.IsNullOrEmpty(queryText))
            {
                newSeriesList = await App.SeriesService.GetSeriesAsync(WachedFlag, skip, favoriteFlag);
            }
            else
            {
                newSeriesList = await App.SeriesService.GetSeriesAsync(WachedFlag, skip, queryText, favoriteFlag);
            }

            if (newSeriesList is not null && newSeriesList.Count() > 0)
            {
                foreach (var item in newSeriesList)
                {
                    SeriesList.Add(item);
                }
            }
            SeriesCount = App.SeriesService.relativeItemsCount;
            ViewedSeriesCount = SeriesList.Count() + skip;
        }
        catch (Exception)
        {
        }
        finally
        {
            IsRefreshing = false;
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task OnAddSeries()
    {
        await Shell.Current.GoToAsync(nameof(NewSeriesPage));
    }

    [RelayCommand]
    private void OnDecSeriesList()
    {
        if (skip >= 5)
        {
            skip -= 5;
            OnAppearing();
        }
        if (skip < 0) { OnAppearing(); skip = 0; }
    }

    private async void OnDeleteCommand()
    {
        if (currentSeries is null)
        {
            return;
        }
        await App.SeriesService.DeleteSeriesAsync(currentSeries.seriesId);
        OnAppearing();
    }

    private async void OnDetachCommand()
    {
        if (currentSeries is null)
        {
            return;
        }

        currentSeries.isOver = !currentSeries.isOver;
        if (Parameters.WachedFlag == false)
        {
            currentSeries.currentEpisode = currentSeries.lastEpisode;
            currentSeries.overDate = DateTime.Now.ToString();
        }
        else
        {
            currentSeries.currentEpisode = currentSeries.startEpisode;
            currentSeries.overDate = string.Empty;
        }

        await App.SeriesService.AddUpdateSeriesAsync(currentSeries);
        OnAppearing();
    }

    private async void OnEditCommand()
    {
        await Navigation.PushAsync(new NewSeriesPage(currentSeries));
    }

    [RelayCommand]
    private void OnIncSeriesList()
    {
        if ((SeriesList.Count() + skip) < SeriesCount)
        {
            skip += 5;
            OnAppearing();
        }
    }
}