using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using Org.W3c.Dom;
using static Android.Media.MediaRouter;
using static Android.Provider.ContactsContract.CommonDataKinds;
using SeriesTracker.Models;

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
    private async void OnAddSeries()
    {
        //await Shell.Current.GoToAsync(nameof(DetailNotePage));
    }
}
