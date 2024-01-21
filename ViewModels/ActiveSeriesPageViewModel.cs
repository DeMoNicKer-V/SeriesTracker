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
    public string quaryText;
    [ObservableProperty]
    public int seriesCount;

    [ObservableProperty]
    public int viewedSeriesCount;

    internal LOAD_PARAMETER loadParameter;
    internal bool favoriteflag = false;
    private readonly ContentPage _page;
    private ActivePagePopUp activeSeriesPagePopUp;
    private Series currentSeries;

    private ObservableCollection<Series> seriesList = new ObservableCollection<Series>();
    private int skip = 0;
    public ActiveSeriesPageViewModel(INavigation navigation, ContentPage contentPageBehavior)
    {
        Navigation = navigation;
        _page = contentPageBehavior;
    }


    public Command DeleteCommand { get; }

    public Command DetachCommand { get; }

    public Command EditCommand { get; }

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

    [RelayCommand]
    private async Task FilterSeries(string query)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                loadParameter = LOAD_PARAMETER.FILTER;
                quaryText = new string(query.ToLower());
                skip = 0;
                IsBusy = true;
            }
            else if (loadParameter == LOAD_PARAMETER.FILTER)
            {
                loadParameter = LOAD_PARAMETER.DEFAULT;
                skip = 0;
                IsBusy = true;
            }
        }
        catch (Exception) { }
        finally { }
    }

    [RelayCommand]
    private async Task LoadSeries()
    {
        IsBusy = true;
        try
        {
            SeriesList.Clear();
            IEnumerable<Series> newSeriesList = new List<Series>();
            switch (loadParameter)
            {
                case LOAD_PARAMETER.DEFAULT:
                    newSeriesList = await App.SeriesService.GetSeriesAsync(WachedFlag, skip, favoriteflag);
                    newSeriesList = newSeriesList.OrderByDescending(f => f.isFavourite);
                    if (newSeriesList != null && newSeriesList.Count() > 0)
                    {
                        foreach (var item in newSeriesList)
                        {
                            SeriesList.Add(item);
                        }
                    }
                    SeriesCount = App.SeriesService.relativeItemsCount;
                    ViewedSeriesCount = SeriesList.Count() + skip;
                    break;

                case LOAD_PARAMETER.FILTER:
                    newSeriesList = await App.SeriesService.GetSeriesAsync(WachedFlag, skip, quaryText, favoriteflag);
                    newSeriesList = newSeriesList.OrderByDescending(f => f.isFavourite);
                    if (newSeriesList != null && newSeriesList.Count() > 0)
                    {
                        foreach (var item in newSeriesList)
                        {
                            SeriesList.Add(item);
                        }
                    }
                    SeriesCount = App.SeriesService.relativeItemsCount;
                    ViewedSeriesCount = SeriesList.Count() + skip;
                    break;
                default:
                    break;
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
    private async void OnAddSeries()
    {
        await Shell.Current.GoToAsync(nameof(NewSeriesPage));
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
        if (Parameters.WachedFlag == false)
        {


            currentSeries.isOver = true;
            currentSeries.currentEpisode = currentSeries.lastEpisode;
            currentSeries.overDate = DateTime.Now.ToString();
        }
        else 
        {
            currentSeries.isOver = false;
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
    private async void OnIncSeriesList()
    {
        if ((SeriesList.Count() + skip) < SeriesCount)
        {
            skip += 5;
            IsBusy = true;
        }
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