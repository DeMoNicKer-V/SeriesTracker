using CommunityToolkit.Mvvm.ComponentModel;
using SeriesTracker.Models;

namespace SeriesTracker.ViewModels;

public partial class BaseSeriesModel : BaseViewModel
{
    [ObservableProperty]
    private Series _series;

    public INavigation Navigation
    {
        get; set;
    }
}