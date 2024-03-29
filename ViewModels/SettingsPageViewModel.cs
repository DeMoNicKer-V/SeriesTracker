﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SeriesTracker.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using static SeriesTracker.Services.Extensions.SeriesEqualExtension;

namespace SeriesTracker.ViewModels
{
    public partial class SettingsPageViewModel : BaseSeriesModel
    {
        private readonly ContentPage _page;
        private static readonly CancellationTokenSource cancellationTokenSource = new();
        private IFileSaver fileSaver;

        [ObservableProperty]
        public int activeIndicator = 0;

        public SettingsPageViewModel(INavigation navigation, ContentPage contentPageBehavior, IFileSaver fileSaver)
        {
            Navigation = navigation;
            _page = contentPageBehavior;
            seriesList = new List<Series>();
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
                await GetSeriesList(true);
                if (seriesList.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Произошла ошибка", "В базе данных не найдено элементов для экспорта.", "Ок");
                }
                var json = JsonSerializer.Serialize(seriesList);

                using var stream = new MemoryStream(Encoding.Default.GetBytes(json));
                var path = await fileSaver.SaveAsync("SeriesTrackerData.json", stream, cancellationTokenSource.Token);
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Произошла ошибка", "Пожалуйста, проверьте наличие экспортируемых данных. Также имя файла должно быть корректным", "Ок");
            }
        }

        private async Task GetSeriesList(bool flag)
        {
            IsBusy = true;
            try
            {
                seriesList.Clear();
                var newSeriesList = await App.SeriesService.GetSeriesAsync();
                foreach (var item in newSeriesList)
                {
                    if (flag == true) item.seriesId = 0;
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
        private async Task InPutSync()
        {
            IsBusy = true;
            ActiveIndicator = 1;
            try
            {
                await App.FirebaseService.InSynchronize();
            }
            catch (Exception)
            {
                throw;
            }
            finally { await OnAppearing(); await ShowToast("Входящая синхронизация выполнена"); }
        }

        [RelayCommand]
        private async Task OutPutSync()
        {
            IsBusy = true;
            ActiveIndicator = 2;
            try
            {
                await App.FirebaseService.OutSynchronize();
            }
            catch (Exception)
            {
                throw;
            }
            finally { await OnAppearing(); await ShowToast("Истодящая синхронизация выполнена"); }
        }

        [RelayCommand]
        private async Task FullSync()
        {
            IsBusy = true;
            ActiveIndicator = 3;
            try
            {
                await App.FirebaseService.InSynchronize();
                await App.FirebaseService.OutSynchronize();
            }
            catch (Exception)
            {
                throw;
            }
            finally { await OnAppearing(); await ShowToast("Полная синхронизация выполнена"); }
        }

        [RelayCommand]
        private async Task OnDeleteAllCloud()
        {
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

        public async Task OnAppearing()
        {
            IsBusy = false;
            AllSeriesCount = await App.SeriesService.GetAllSeriesCountSync();
            ActiveIndicator = 0;
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
                            var resultList = pSeriesList.Except(seriesList, new SeriesSyncComparer()).ToList();

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
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        private static async Task ShowToast(string text)
        {
            var toast = Toast.Make(text, ToastDuration.Short, 14);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
}