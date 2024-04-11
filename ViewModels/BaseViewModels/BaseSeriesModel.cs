using CommunityToolkit.Mvvm.ComponentModel;
using SeriesTracker.Classes;
using SeriesTracker.Models;
using SeriesTracker.Views;

namespace SeriesTracker.ViewModels;

public partial class BaseSeriesModel : BaseViewModel
{
    [ObservableProperty]
    private Series _series;

    [ObservableProperty]
    private AnimeBase _anime;

    [ObservableProperty]
    public int _seriesCount;

    [ObservableProperty]
    public int _viewedSeriesCount;

    [ObservableProperty]
    public int _allSeriesCount;

    public Command DeleteCommand { get; set; }

    public Command DetachCommand { get; set; }

    public Command EditCommand { get; set; }

    public Command BackCommand { get; set; }

    public BaseSeriesModel() 
    {
        BackCommand = new Command(OnBackCommand);
        EditCommand = new Command(OnEditCommand);
    }
    public INavigation Navigation
    {
        get; set;
    }

    public static bool CheckSeries(Series series)
    {
        if (series is null) return false;
        return true;
    }

    public async void OnDeleteCommand()
    {
        if (!BaseSeriesModel.CheckSeries(Series)) { return; }
        await App.SeriesService.DeleteSeriesAsync(Series.seriesId);
    }

    public async void OnDetachCommand()
    {
        if (!CheckSeries(Series)) { return; }
        if (!Series.isOver)
        {
            Series.currentEpisode = Series.lastEpisode;
            Series.overDate = DateTime.Now.ToString();
        }
        else
        {
            Series.currentEpisode = 0;
            Series.overDate = string.Empty;
        }
        Series.isOver = !Series.isOver;
        await App.SeriesService.AddUpdateSeriesAsync(Series);
    }

    public async void OnEditCommand()
    {
        await Navigation.PushAsync(new NewSeriesPage(Series));
    }

    public async void OnBackCommand()
    {
        await Shell.Current.GoToAsync("..");
    }
}