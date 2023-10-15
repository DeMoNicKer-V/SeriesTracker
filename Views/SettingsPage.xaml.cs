using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Services;
using SeriesTracker.ViewModels;
using System.Text.Json;
using System.Text;
using System.Collections.ObjectModel;
using SeriesTracker.Models;
using System;

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
    private async Task ExportData()
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
            var path = await fileSaver.SaveAsync("SeriesTrackerData.json", stream, cancellationTokenSource.Token);
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте наличиние экспортируемых данных. Также имя файла должно быть корректным", "Ок");
        }
        finally
        {
            IsBusy = false;
        }
    }


    [RelayCommand]
    private async Task<FileResult> PickAndShow()
    {
        try
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "application/json" } }, // UTType values
                    { DevicePlatform.Android, new[] { "application/json" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { "application/json" } }, // file extension
                    { DevicePlatform.Tizen, new[] { "application/json" } },
                    { DevicePlatform.macOS, new[] { "application/json" } }, // UTType values
                });

            PickOptions options = new()
            {
                PickerTitle = "Пожалуйста, выбирите файл с данными!",
                FileTypes = customFileType,
            };
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("json", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    using (var reader = new StreamReader(stream))
                    {
                        string jsonfileData = reader.ReadToEnd();
                        var list = JsonSerializer.Deserialize<List<Series>>(jsonfileData);
                        await Shell.Current.DisplayAlert("Произошла ошибка", list[0].seriesName, "Ок");
                    }
             

                }
            }

            return result;
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }

        return null;
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