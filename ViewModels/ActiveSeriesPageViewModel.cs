using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SeriesTracker.ViewModels;

public partial class ActiveSeriesPageViewModel : BaseSeriesModel
{

    public ActiveSeriesPageViewModel(INavigation navigation)
    {
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



    public ICommand PerformSearch => new Command<string>((string query) =>
    {
        if (string.IsNullOrWhiteSpace(query)) return;
    var normalizedQuery = query?.ToLower() ?? "";
    SeriesList = SeriesList.Where(item => item.seriesName.ToLowerInvariant().Contains(normalizedQuery)).ToObservableCollection();
    });

    public string GetTitleDescription(Series series)
    {
        
            if (series.seriesSeason > 0)
            {
                return $"Сезон {series.seriesSeason}-й, год выхода - {series.releaseYear}";
            }
            return $"Год выхода - {series.releaseYear}";
        
    }
    public Color GetRandomColor
    {
        get
        {
            return colorList[new Random().Next(0, 18)];
        }
    }

    private ObservableCollection<Series> seriesList = new ObservableCollection<Series>();

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

    public int ActionIndex(string action)
    {
        switch (action)
        {
            case "Удалить": return 0;
            case "Delete": return 0;
            case "Закончить просмотр": return 1;
            case "Finish viewing": return 1;
            case "Редактировать": return 2;
            case "Edit": return 2;
        }
        return -1;
    }

    public void OnAppearing()
    {
        IsBusy = true;
    }

    [RelayCommand]
    private async void AdditionalAction(Series series)
    {
        string action = await Shell.Current.DisplayActionSheet(series.seriesName, "Закрыть", "Удалить", "Закончить просмотр", "Редактировать");
        if (action != null | series != null)
        {
            switch (ActionIndex(action))
            {
                case 0: await App.SeriesService.DeleteSeriesAsync(series.seriesId); OnAppearing(); return;
                case 1: series.isOver = true; await App.SeriesService.AddUpdateSeriesAsync(series); OnAppearing(); return;
                case 2: await Navigation.PushAsync(new NewSeriesPage(series)); return;
            }
        }

        return;
    }

    [RelayCommand]
    private async Task DetailView(Series series)
    {
        await Navigation.PushAsync(new DetailSeriesPage(series));
    }

    [RelayCommand]
    private async Task LoadSeries()
    {
        IsBusy = true;
        try
        {
            SeriesList.Clear();
            var newSeriesList = await App.SeriesService.GetSeriesAsync(false);
            if (newSeriesList != null && newSeriesList.Count() > 0)
            {
                foreach (var item in newSeriesList)
                {
                    SeriesList.Add(item);
                }
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
            return;
        }
        series.currentEpisode -= 1;
        await App.SeriesService.AddUpdateSeriesAsync(series);
        OnAppearing();
    }

    [RelayCommand]
    private async void OnIncEpisode(Series series)
    {
        if (series == null)
        {
            return;
        }
        else if (series.currentEpisode == series.lastEpisode)
        {
            bool action = await Shell.Current.DisplayAlert($"{series.seriesName} - конец?",
                "Похоже, что вы посмотрели все эпизоды. Пометить данный сериал как просмотренный?", "Да", "Нет");
            if (action) 
            { 
                series.isOver = true; 
                await App.SeriesService.AddUpdateSeriesAsync(series); 
                OnAppearing(); 
                return; 
            }
            return;
        }
        series.currentEpisode += 1;
        await App.SeriesService.AddUpdateSeriesAsync(series);
        OnAppearing();
    }
}