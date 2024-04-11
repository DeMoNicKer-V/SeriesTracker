using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using SeriesTracker.Controls.CustomPopUp;

namespace SeriesTracker.Services.Constant
{
    public static class SeriesBaseParameters
    {
        public static bool WachedFlag { get; set; } = false;
        public static readonly string FilePath = Environment.GetFolderPath(
                   Environment.SpecialFolder.LocalApplicationData);

        private static int skipItem;
        public static int SkipItem
        {
            get => skipItem;
            set
            {
                skipItem = value;
                if (skipItem < 0) { skipItem = 0; }
            }
        }

        public static bool FavoriteFlag { get; set; }

        public static string QueryText { get; set; } = string.Empty;

        public static async Task ShowToast(string text)
        {
            CancellationTokenSource cancellationTokenSource = new();
            var toast = Toast.Make(text, ToastDuration.Long, 13);
            await toast.Show(cancellationTokenSource.Token);
        }

        public static async Task ShowErrorAlert(Shell page, string text)
        {
            var resultError = await page.DisplayAlert("Произошла ошибка", text, "Копировать ошибку", "Закрыть");
            if (resultError == true)
            {
                await Clipboard.SetTextAsync(text);
            }
        }
    }
}