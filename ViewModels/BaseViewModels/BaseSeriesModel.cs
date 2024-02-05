using CommunityToolkit.Mvvm.ComponentModel;
using SeriesTracker.Classes.Shikimori;
using SeriesTracker.Models;

namespace SeriesTracker.ViewModels;

public partial class BaseSeriesModel : BaseViewModel
{
    [ObservableProperty]
    private Series _series;
    [ObservableProperty]
    private Anime _anime;

    public INavigation Navigation
    {
        get; set;
    }
}