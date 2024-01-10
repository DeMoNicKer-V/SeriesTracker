using CommunityToolkit.Maui.Core.Extensions;
using Microsoft.Maui.Platform;
#if __ANDROID__
using Android.Views;
using Android.OS;
#endif

namespace ThemeManager;

///
/// ThemeManager is a static class that provides methods for setting the colors of the status bar and navigation bar.
/// 
public static class ThemeManager
{
    ///
    /// Set the color of the status bar
    ///
    /// <param name="color">Nullable Color object representing the color to set.</param>
    /// 
    public static void SetStatusBarColor(Color? color)
    {
        if (color == null) return;

#if __ANDROID__
        // Check if the platform is Android and the API level is at least Lollipop
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        {
            // Get the current activity's window
            var window = Platform.CurrentActivity?.Window;
            if (window == null) return;

            // Set the flags and color of the status bar
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(color.ToPlatform());

            // Update the visibility of the status and navigation bars
            UpdateBarsVisibility();
        }
#endif
    }

    ///
    /// Set the color of the navigation bar.
    ///
    /// <param name="color">Nullable Color object representing the color to set.</param>
    /// 
    public static void SetNavigationBarColor(Color? color)
    {
        if (color == null) return;

#if __ANDROID__
        // Check if the platform is Android and the API level is at least Lollipop
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        {
            // Get the current activity's window
            var window = Platform.CurrentActivity?.Window;
            if (window == null) return;

            // Set the flags and color of the navigation bar
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.SetNavigationBarColor(color.ToPlatform());

            // Update the visibility of the status and navigation bars
            UpdateBarsVisibility();
        }
#endif
    }

    ///
    /// Set the color of both the status and navigation bars.
    ///
    /// <param name="color">Nullable Color object representing the color to set.</param>
    /// 
    public static void SetBarColor(Color? color)
    {
        if (color == null) return;

#if __ANDROID__
        // Check if the platform is Android and the API level is at least Lollipop
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        {
            // Get the current activity's window
            var window = Platform.CurrentActivity?.Window;
            if (window == null) return;

            // Set the flags and color of both the status and navigation bars
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(color.ToPlatform());
            window.SetNavigationBarColor(color.ToPlatform());

            // Update the visibility of the status and navigation bars
            UpdateBarsVisibility();
        }
#endif
    }

#if __ANDROID__
    ///
    /// Update the visibility of the status and navigation bars based on the colors set for them.
    /// 
    private static void UpdateBarsVisibility()
    {
        // Get the current activity's window
        var window = Platform.CurrentActivity?.Window;

        if (window == null) return;

        // Get the colors of the status and navigation bars
        var statusBarColor = window.StatusBarColor;
        var navigationBarColor = window.NavigationBarColor;

        // Check if the API level is at least R (Android 11)
        if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
        {
            // We will try to set the visibility of the status and navigation bars but since error can occur when trying to call
            // this too early after start of the app we will catch any errors and ignore them.
            try
            {
                // Get the insets controller for the window
                var insetsController = window.InsetsController;

                if (insetsController == null) return;

                // Convert the colors to Color objects
                var statusColor = Color.FromInt(statusBarColor);
                var navigationColor = Color.FromInt(navigationBarColor);

                // Check if the colors are dark and set the visibility of the status and navigation bars accordingly
                if (statusColor.IsDarkForTheEye())
                {
                    insetsController.SetSystemBarsAppearance(0, (byte)WindowInsetsControllerAppearance.LightStatusBars);
                }
                else
                {
                    insetsController.SetSystemBarsAppearance((byte)WindowInsetsControllerAppearance.LightStatusBars,
                        (byte)WindowInsetsControllerAppearance.LightStatusBars);
                }

                if (navigationColor.IsDarkForTheEye())
                {
                    insetsController.SetSystemBarsAppearance(0,
                        (byte)WindowInsetsControllerAppearance.LightNavigationBars);
                }
                else
                {
                    insetsController.SetSystemBarsAppearance((byte)WindowInsetsControllerAppearance.LightNavigationBars,
                        (byte)WindowInsetsControllerAppearance.LightNavigationBars);
                }
            }
            catch (Exception e)
            {
                // ignored
                // There is a bug in MAUI which if this is called too early it will crash the app
                // specifically if we are trying to get insetsController
                Console.WriteLine(e);
            }
        }
        else
        {
            // This is for API levels below R (Android 11)
            // Get the decor view of the window
            var decorView = window.DecorView;

            if (decorView == null) return;

            // Convert the colors to Color objects
            var statusColor = Color.FromInt(statusBarColor);
            var navigationColor = Color.FromInt(navigationBarColor);

            // Check if the colors are dark and set the visibility of the status and navigation bars accordingly
            if (statusColor.IsDarkForTheEye())
            {
                decorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
            }
            else
            {
                decorView.SystemUiVisibility = 0;
            }

            if (navigationColor.IsDarkForTheEye())
            {
                decorView.SystemUiVisibility |= (StatusBarVisibility)SystemUiFlags.LightNavigationBar;
            }
            else
            {
                decorView.SystemUiVisibility |= 0;
            }
        }
    }
#endif
}