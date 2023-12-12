using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using SeriesTracker.Views;
using InputKit.Handlers;
using SeriesTracker.Controls.MaterialEntry;

namespace SeriesTracker;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureMauiHandlers(handlers =>
        {
            // Add following line:
            handlers.AddInputKitHandlers(); // 👈
        }).ConfigureFonts(fonts =>
        {
            fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
            fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");
            fonts.AddFont("Nunito-Italic.ttf", "NunitoItalic");
        }).UseMauiCommunityToolkit();
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if ANDROID
				handler.PlatformView.Background = null;
				handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
				handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            }
        });
#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
        builder.Services.AddTransient<SettingsPage>();
        return builder.Build();
    }
}