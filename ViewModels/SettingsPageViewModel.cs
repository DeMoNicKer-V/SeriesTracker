using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using static SeriesTracker.Services.Extensions.SeriesEqualExtension;
using static SeriesTracker.Services.Constant.SeriesBaseParameters;

namespace SeriesTracker.ViewModels
{
    public partial class SettingsPageViewModel : BaseSeriesModel
    {
        [ObservableProperty]
        public int activeIndicator = 0;

        [ObservableProperty]
        public string dateSyncLast;

        private static readonly CancellationTokenSource cancellationTokenSource = new();
        private static readonly string DateNow = DateTime.Now.ToString();
        private readonly IFileSaver fileSaver;
        private List<Series> SeriesList { get; set; } = new List<Series>();

        public SettingsPageViewModel(IFileSaver fileSaver)
        {
            this.fileSaver = fileSaver;
            var lastDate = Preferences.Get("LastSyncDate", "");
            DateSyncLast = string.IsNullOrEmpty(lastDate) ? null : DateTime.Parse(lastDate).ToString("d MMMM, yyyy" + " г.");
        }

        public async Task OnAppearing()
        {
            IsBusy = false;
            AllSeriesCount = await App.SeriesService.GetAllSeriesCount();
            ActiveIndicator = 0;
        }

        private async Task AfterSyncUpdate(string message)
        {
            await OnAppearing(); SetSyncLastDate(); await ShowToast(message);
            var lastDate = Preferences.Get("LastSyncDate", "");
            DateSyncLast = string.IsNullOrEmpty(lastDate) ? null : DateTime.Parse(lastDate).ToString("d MMMM, yyyy" + " г.");
        }

        private async Task<bool> CheckInternetAccess()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType != NetworkAccess.Internet)
            {
                await ShowToast("Отсутствует интернет подключение");
                return false;
            }
            return true;
        }

        [RelayCommand]
        private async Task ExportData()
        {
            try
            {
                await GetSeriesList(true);
                if (SeriesList.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Произошла ошибка", "В базе данных не найдено элементов для экспорта.", "Ок");
                }
                var json = JsonSerializer.Serialize(SeriesList);

                using var stream = new MemoryStream(Encoding.Default.GetBytes(json));
                var path = await fileSaver.SaveAsync("SeriesTrackerData.json", stream, cancellationTokenSource.Token);
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте наличие экспортируемых данных. Также имя файла должно быть корректным", "Ок");
            }
        }

        [RelayCommand]
        private async Task FullSync()
        {
            if (await CheckInternetAccess() == false) return;
            IsBusy = true;
            ActiveIndicator = 3;
            try
            {
                await App.FirebaseService.InSynchronize();
                await App.FirebaseService.OutSynchronize();
            }
            catch (Exception ex)
            {
                await ShowErrorAlert(Shell.Current, ex.Message);
                return;
            }
            await AfterSyncUpdate("Полная синхронизация выполнена");
        }

        private async Task GetSeriesList(bool flag)
        {
            IsBusy = true;
            try
            {
                SeriesList.Clear();
                var newSeriesList = await App.SeriesService.GetSeriesAsync();
                foreach (var item in newSeriesList)
                {
                    if (flag == true) item.seriesId = 0;
                    SeriesList.Add(item);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorAlert(Shell.Current, ex.Message);
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        private async Task InPutSync()
        {
            if (await CheckInternetAccess() == false) return;
            IsBusy = true;
            ActiveIndicator = 1;
            try
            {
                await App.FirebaseService.InSynchronize();
            }
            catch (Exception ex)
            {
                await ShowErrorAlert(Shell.Current, ex.Message);
                return;
            }
            await AfterSyncUpdate("Входящая синхронизация выполнена");
        }

        [RelayCommand]
        private async Task OnDeleteAllCloud()
        {
            if (await CheckInternetAccess() == false) return;
            await App.FirebaseService.DeleteAll();
            await OnAppearing();
            await ShowToast("Все данные в облаке удалены");
        }

        [RelayCommand]
        private async Task OnDeleteAllDataBase()
        {
            await App.SeriesService.DeleteAll();
            await OnAppearing();
            await ShowToast("Все данные в БД удалены");
        }

        [RelayCommand]
        private async Task OutPutSync()
        {
            if (await CheckInternetAccess() == false) return;
            IsBusy = true;
            ActiveIndicator = 2;
            try
            {
                await App.FirebaseService.OutSynchronize();
            }
            catch (Exception ex)
            {
                await ShowErrorAlert(Shell.Current, ex.Message);
                return;
            }
            await AfterSyncUpdate("Исходящая синхронизация выполнена"); 
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
                        await GetSeriesList(false);

                        using var stream = await result.OpenReadAsync();
                        using (var reader = new StreamReader(stream))
                        {
                            string jsonfileData = reader.ReadToEnd();
                            var pSeriesList = JsonSerializer.Deserialize<List<Series>>(jsonfileData);
                            var resultList = pSeriesList.Except(SeriesList, new SeriesSyncComparer()).ToList();

                            foreach (Series series in resultList)
                            {
                                await App.SeriesService.AddUpdateSeriesAsync(series);
                            }
                        }
                    }
                }
                await OnAppearing();
                return result;
            }
            catch (Exception ex)
            {
                await ShowErrorAlert(Shell.Current, ex.Message);
            }

            return null;
        }

        private void SetSyncLastDate()
        {
            Preferences.Set("LastSyncDate", DateNow);
        }
    }
}