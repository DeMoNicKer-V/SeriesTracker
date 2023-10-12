using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Services;
using SeriesTracker.ViewModels;
using System.Text.Json;
using System.Text;
using System.Collections.ObjectModel;
using SeriesTracker.Models;

namespace SeriesTracker.Views;

public partial class SettingsPage : ContentPage
{
    SettingsPageViewModel settingsPageViewModel;
    IFileSaver fileSaver;
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    public SettingsPage(IFileSaver fileSaver)
	{
		InitializeComponent();
        seriesList = new List<Series>();
        BindingContext = this;
        this.fileSaver = fileSaver; 
    }

    public List<Series> seriesList
    {
        get;
    }

    [RelayCommand]
    private async Task SaveFile()
    {
        IsBusy = true;
        try
        {
            seriesList.Clear();
            var newSeriesList = await App.SeriesService.GetSeriesAsync(false);
            foreach (var item in newSeriesList)
            {
                seriesList.Add(item);
            }
            var json = JsonSerializer.Serialize(seriesList);

            using var stream = new MemoryStream(Encoding.Default.GetBytes(json));
            var path = await fileSaver.SaveAsync("SeriesTrackerJson.json", stream, cancellationTokenSource.Token);
            await Shell.Current.DisplayAlert("Файл экспортирован", "Данные были сохранены в файл формата JSON.", "Ок");
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте Интернет соединение.", "Ок");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://vk.com/v_shakov");
            Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте Интернет соединение.", "Ок");
            return;
        }
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            Uri uri = new Uri("https://github.com/DeMoNicKer-V");
            Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте Интернет соединение.", "Ок");
            return;
        }
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Preferences.Set("AppTheme", SettingsService.Instance.Theme.AppTheme.ToString());
    }
}