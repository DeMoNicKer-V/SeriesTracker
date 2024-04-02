using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Platform;

namespace SeriesTracker;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    public override bool DispatchTouchEvent(MotionEvent e)
    {
        if (e.Action == MotionEventActions.Down ||
            e.Action == MotionEventActions.Move ||
            e.Action == MotionEventActions.Up)
        {
            // Search EditText Control
            var editors = (Window.DecorView as ViewGroup).GetChildrenOfType<EditText>();

            foreach (var editor in editors)
            {
                // Initialize EditText screen position
                int[] pos = new int[2];
                // Set EditText screen position
                editor.GetLocationOnScreen(pos);
                // Create hit test rectangle
                Rect hitRect = new Rect(pos[0], pos[1], editor.Width, editor.Height);
                // Judge hit test
                bool isHitTest = hitRect.Contains(e.GetX(), e.GetY());
                // Hit test result is OK
                if (isHitTest)
                {
                    // Touch event intercept
                    editor.Parent.RequestDisallowInterceptTouchEvent(true);
                }
            }
        }

        return base.DispatchTouchEvent(e);
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        this.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);
        this.Window.AddFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
        this.Window.AddFlags(Android.Views.WindowManagerFlags.TranslucentNavigation);
        this.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
    }
}