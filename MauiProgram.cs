using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using SeriesTracker.Views;
using SeriesTracker.Controls.MaterialEntry;
using Microsoft.Maui.LifecycleEvents;
using SeriesTracker.Behaviors;

namespace SeriesTracker;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit().ConfigureMauiHandlers(handlers =>
        {

        }).ConfigureFonts(fonts =>
        {
            fonts.AddFont("Nunito-Regular.ttf", "NunitoRegular");
            fonts.AddFont("Nunito-Bold.ttf", "NunitoBold");
            fonts.AddFont("Nunito-Italic.ttf", "NunitoItalic");
        }).ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android => android
                .OnCreate((activity, bundle) =>
                {
                    Apply();
                    Setup();
                }));
#elif IOS
            events.AddiOS(ios => ios
                .OnActivated((app) =>
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
        AllowMultiLineTruncationOnAndroid();

        return builder.Build();
    }

    public static void Setup()
    {
        if (Application.Current is null)
            return;

        Application.Current.RequestedThemeChanged += (s, e) => Apply();
    }

    /// <summary>
    /// Applies theme
    /// </summary>
    public static void Apply()
    {
        if (Application.Current is null)
            return;

#if ANDROID
        //AppCompatDelegate.DefaultNightMode = (int)UiNightMode.Yes;
        AndroidX.AppCompat.App.AppCompatDelegate.DefaultNightMode = Application.Current.UserAppTheme switch
        {
            AppTheme.Light => AndroidX.AppCompat.App.AppCompatDelegate.ModeNightNo,
            AppTheme.Dark => AndroidX.AppCompat.App.AppCompatDelegate.ModeNightYes,
            _ => AndroidX.AppCompat.App.AppCompatDelegate.ModeNightFollowSystem
        };
#elif IOS
            Platform.GetCurrentUIViewController().OverrideUserInterfaceStyle = Application.Current.UserAppTheme switch
            {
                AppTheme.Light => UIKit.UIUserInterfaceStyle.Light,
                AppTheme.Dark => UIKit.UIUserInterfaceStyle.Dark,
                _ => UIKit.UIUserInterfaceStyle.Unspecified
            };

            //UIKit.UIApplication.SharedApplication.Windows[0].OverrideUserInterfaceStyle = UIKit.UIUserInterfaceStyle.Dark;
#endif
    }

    static void AllowMultiLineTruncationOnAndroid()
    {
#if ANDROID

        /* 
		 * The default Controls handling of LineBreakMode and MaxLines on Android
		 * only allows single lines when using text truncation. However, combining
		 * setMaxLines() and TextUtils.TruncateAt.END _is_ supported on Android 
		 * (see https://developer.android.com/reference/android/widget/TextView#setEllipsize(android.text.TextUtils.TruncateAt))
		 * 
		 * The following code updates the mappings for Label on Android to support
		 * this scenario. Truncation and max lines both affect the platform setting
		 * of maximum lines, so we need to modify the mappings for both properties. 
		 * We append a second mapping that checks for our target situation (end truncation)
		 * and sets the maximum lines to the target value.
		*/

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