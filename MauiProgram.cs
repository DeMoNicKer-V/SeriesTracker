using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using SeriesTracker.Controls;
using SeriesTracker.Views;

namespace SeriesTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit().ConfigureMauiHandlers(handlers =>
        {
#if ANDROID
            handlers.AddHandler<Shell, CustomShellHandler>();
#endif
        }).ConfigureFonts(fonts =>
        {
            fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
            fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");
            fonts.AddFont("Nunito-Italic.ttf", "NunitoItalic");
        }).UseMauiCommunityToolkit().ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android => android
                .OnCreate((activity, bundle) =>
                {
                    Apply();
                    Setup();
                }));
#endif
        }); ;
        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
            if (view is Editor)
            {
#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            }
        });
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
        {
            if (view is BorderlessEntry || view is Entry)
            {
#if ANDROID
                handler.PlatformView.Background = null;
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            }
        });
#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
        builder.Services.AddTransient<SettingsPage>();
        AllowMultiLineTruncationOnAndroid();

        return builder.Build();
    }

    public static void Setup()
    {
        if (Application.Current is null)
            return;

        Application.Current.RequestedThemeChanged += (s, e) =>
        {
            Apply();
        };
    }

    /// <summary>
    /// Applies theme
    /// </summary>
    public static void Apply()
    {
        if (Application.Current is null)
            return;

#if ANDROID
        AndroidX.AppCompat.App.AppCompatDelegate.DefaultNightMode = Application.Current.UserAppTheme switch
        {
            AppTheme.Light => AndroidX.AppCompat.App.AppCompatDelegate.ModeNightNo,
            AppTheme.Dark => AndroidX.AppCompat.App.AppCompatDelegate.ModeNightYes,
            _ => AndroidX.AppCompat.App.AppCompatDelegate.ModeNightFollowSystem
        };
#endif
    }

    private static void AllowMultiLineTruncationOnAndroid()
    {
#if ANDROID
        static void UpdateMaxLines(Microsoft.Maui.Handlers.LabelHandler handler, ILabel label)
        {
            var textView = handler.PlatformView;
            if (label is Label controlsLabel && textView.Ellipsize == Android.Text.TextUtils.TruncateAt.End)
            {
                textView.SetMaxLines(controlsLabel.MaxLines);
            }
        };

        Label.ControlsLabelMapper.AppendToMapping(
           nameof(Label.LineBreakMode), UpdateMaxLines);

        Label.ControlsLabelMapper.AppendToMapping(
            nameof(Label.MaxLines), UpdateMaxLines);
#endif
    }
}