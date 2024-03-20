using CommunityToolkit.Mvvm.ComponentModel;
using SeriesTracker.Classes;
using SeriesTracker.Classes.Shikimori;
using SeriesTracker.Models;
using System.Collections.ObjectModel;

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

    public INavigation Navigation
    {
        get; set;
    }
}