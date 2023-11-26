using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using SeriesTracker.Views;
using InputKit.Handlers;

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
#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
        builder.Services.AddTransient<SettingsPage>();
        return builder.Build();
    }
}