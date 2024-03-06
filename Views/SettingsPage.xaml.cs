using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using SeriesTracker.Services;
using SeriesTracker.ViewModels;
using System.Diagnostics;
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
            await Shell.Current.DisplayAlert("��������� ������", "����������, ��������� ������� �������������� ������. ����� ��� ����� ������ ���� ����������", "��");
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
            Debug.WriteLine(ex.Message);
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
                PickerTitle = "����������, �������� ���� � �������!",
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
                    //await Shell.Current.DisplayAlert("������ �������������", $"���-�� ��������������� ��������: {pSeriesList.Count}", "��");
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
            Debug.WriteLine(ex.Message);
        }

        return null;
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Preferences.Set("AppTheme", SettingsService.Instance.Theme.AppTheme.ToString());
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