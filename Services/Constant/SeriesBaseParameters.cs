using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace SeriesTracker.Services.Constant
{
    public static class SeriesBaseParameters
    {
        public static bool WachedFlag { get; set; } = false;
        public static bool AnimeSourceSite { get; set; } = false;
        public static readonly string FilePath = Environment.GetFolderPath(
                   Environment.SpecialFolder.LocalApplicationData);

        private static int skipItem;
        public static int SkipItem
        {
            get => skipItem;
            set
            {
                if (skipItem < 0) { skipItem = 0; }
                else { skipItem = value; }
            }
        }

        public static bool FavoriteFlag { get; set; }

        public static string QueryText { get; set; } = string.Empty;
        public static string RequestText { get; set; } = string.Empty;

        public static async Task ShowToast(string text)
        {
            CancellationTokenSource cancellationTokenSource = new();
            var toast = Toast.Make(text, ToastDuration.Short, 14);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
}
