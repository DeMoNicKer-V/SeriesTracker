using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Controls.PopUp;
using SeriesTracker.Models;
using SeriesTracker.Services.Constant;
using SeriesTracker.Services.SyncJournal;
using SeriesTracker.Views;
using System.Collections.ObjectModel;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker.ViewModels;

public partial class ActiveSeriesPageViewModel : BaseSeriesModel
{
    private readonly ContentPage _page;
    private ActivePagePopUp activeSeriesPagePopUp;
    private Series currentSeries;

    private ObservableCollection<Series> seriesList = new();
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

    public ActiveSeriesPageViewModel(INavigation navigation, ContentPage contentPageBehavior)
    {
        Navigation = navigation;
        _page = contentPageBehavior;
    }

    public async Task OnAppearing()
    {
        SeriesCount = 0;
        ViewedSeriesCount = 0;
        SeriesList.Clear();
        await LoadSeries();
    }

    [RelayCommand]
    private async Task AdditionalAction(Series series)
    {
        activeSeriesPagePopUp = new ActivePagePopUp();
        activeSeriesPagePopUp.ContentPageBehavior = _page;
        activeSeriesPagePopUp.EditCommand = new Command(OnEditCommand);
        activeSeriesPagePopUp.DeleteCommand = new Command(OnDeleteCommand);
        activeSeriesPagePopUp.DetachCommand = new Command(OnDetachCommand);
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
    private async Task FilterSeries(string query)
    {
        try
        {
            QueryText = new string(query.ToLower());
            SkipItem = 0;
            await OnAppearing();
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
    private async Task LoadSeries()
    {
        IsBusy = true;
        try
        {
            AllSeriesCount = await App.SeriesService.GetAllSeriesCount(WachedFlag);
            if (AllSeriesCount == 0) 
            { 
                IsBusy = false; 
                return; 
            }

            IEnumerable<Series> newSeriesList = await App.SeriesService.GetSeriesAsync(WachedFlag, SkipItem, QueryText, FavoriteFlag);
            if (newSeriesList is not null && newSeriesList.Count() > 0)
            {
                foreach (var item in newSeriesList)
                {
                    SeriesList.Add(item);
                }
            }
            SeriesCount = App.SeriesService.relativeItemsCount;
            ViewedSeriesCount = SeriesList.Count() + SkipItem;
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
    private async Task OnAddSeries()
    {
        await Shell.Current.GoToAsync(nameof(NewSeriesPage));
    }

    [RelayCommand]
    private async Task OnDecSeriesList()
    {
        if (SkipItem > 0)
        {
            SkipItem -= 5;
            await OnAppearing();
        }
    }

    private async void OnDeleteCommand()
    {
        if (currentSeries is null)
        {
            return;
        }
        new Journal(new DeleteItem(currentSeries.SyncUid)).JournalToJson();

        await App.SeriesService.DeleteSeriesAsync(currentSeries.seriesId);
        if (SeriesList.Count() == 1)
        {
            SkipItem -=5;
        }
        await OnAppearing();
    }

    private async void OnDetachCommand()
    {
        if (currentSeries is null)
        {
            return;
        }

        if (WachedFlag == false)
        {
            currentSeries.currentEpisode = currentSeries.lastEpisode;
            currentSeries.overDate = DateTime.Now.ToString();
        }
        else
        {
            currentSeries.currentEpisode = 1;
            currentSeries.overDate = string.Empty;
        }
        currentSeries.isOver = !currentSeries.isOver;
        await App.SeriesService.AddUpdateSeriesAsync(currentSeries);
        await OnAppearing();
    }

    private async void OnEditCommand()
    {
        await Navigation.PushAsync(new NewSeriesPage(currentSeries));
    }

    [RelayCommand]
    private async Task OnIncSeriesList()
    {
        if (SeriesList.Count() == 5 & (SeriesList.Count() + SkipItem) < SeriesCount)
        {
            SkipItem += 5;
            await OnAppearing();
        }
    }
}