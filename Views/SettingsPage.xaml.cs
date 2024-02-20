using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Services;
using SeriesTracker.ViewModels;
using System.Text;
using System.Text.Json;

namespace SeriesTracker.Views;

public partial class SettingsPage : ContentPage
{
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private IFileSaver fileSaver;
    private SettingsPageViewModel settingsPageViewModel;
    public SettingsPage()
    {
        InitializeComponent();
    }
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
        try
        {
            await GetSeriesList();
            var json = JsonSerializer.Serialize(seriesList);

            using var stream = new MemoryStream(Encoding.Default.GetBytes(json));
            var path = await fileSaver.SaveAsync("SeriesTrackerData.json", stream, cancellationTokenSource.Token);
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте наличие экспортируемых данных. Также имя файла должно быть корректным", "Ок");
        }
    }

    private async Task GetSeriesList()
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
            catch (Exception ex)
            {

            }
        finally { IsBusy = false; }

    }

    [RelayCommand]
    private async Task<FileResult> PickAndShow()
    {
        try
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "application/json" } },
                    { DevicePlatform.Android, new[] { "application/json" } },
                    { DevicePlatform.WinUI, new[] { "application/json" } },
                    { DevicePlatform.Tizen, new[] { "application/json" } },
                    { DevicePlatform.macOS, new[] { "application/json" } },
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
                    await GetSeriesList();

                    using var stream = await result.OpenReadAsync();
                    using (var reader = new StreamReader(stream))
                    {
                        string jsonfileData = reader.ReadToEnd();
                        var pSeriesList = JsonSerializer.Deserialize<List<Series>>(jsonfileData);
                        var resultList = pSeriesList.Except(seriesList, new SeriesComparer()).ToList();
                      


                        foreach (Series series in resultList)
                        {
                            await App.SeriesService.AddUpdateSeriesAsync(series);

                        }
                    }
                    //await Shell.Current.DisplayAlert("Данные импортированы", $"Кол-во импортированных сериалов: {pSeriesList.Count}", "Ок");
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

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Preferences.Set("AppTheme", SettingsService.Instance.Theme.AppTheme.ToString());
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

    public class SeriesComparer : IEqualityComparer<Series>
    {
        public bool Equals(Series x, Series y)
        {
            return x.seriesName == y.seriesName;
        }

        public int GetHashCode(Series obj)
        {
            return (obj.seriesName).GetHashCode();
        }
    }
}