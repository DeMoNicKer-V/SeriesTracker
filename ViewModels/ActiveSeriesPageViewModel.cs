using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Views;
using System.Collections.ObjectModel;

namespace SeriesTracker.ViewModels;

public partial class ActiveSeriesPageViewModel : BaseSeriesModel
{
    public ActiveSeriesPageViewModel(INavigation navigation)
    {
        seriesList = new ObservableCollection<Series>();
        Navigation = navigation;
    }

    public List<Color> colorList { get; } = new List<Color>() { 
        Color.FromArgb("#42A5F5"),
        Color.FromArgb("#EC407A"),
        Color.FromArgb("#26C6DA"),
        Color.FromArgb("#AB47BC"),
        Color.FromArgb("#EF5350"),
        Color.FromArgb("#D59E17"),
        Color.FromArgb("#FFA726"),
        Color.FromArgb("#66BB6A"),
        Color.FromArgb("#8D6E63"),
        Color.FromArgb("#78909C"),
        Color.FromArgb("#26A69A"),
        Color.FromArgb("#5C6BC0"),
        Color.FromArgb("#7E57C2"),
        Color.FromArgb("#FF7043"),
        Color.FromArgb("#00C853"),
        Color.FromArgb("#8BC34A"),
        Color.FromArgb("#FF8A80"),
        Color.FromArgb("#FF80AB") };

    private int randomNumber;

    public Color GetRandomColor
    {
        get
        {
            randomNumber = new Random().Next(0, 18);
            return colorList[randomNumber];
        }
    }

    public ObservableCollection<Series> seriesList
    {
        get;
    }

    public void OnAppearing()
    {
        IsBusy = true;
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

    [RelayCommand]
    private async void OnAddSeries()
    {
        await Shell.Current.GoToAsync(nameof(NewSeriesPage));
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
}