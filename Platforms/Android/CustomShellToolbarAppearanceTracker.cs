using Android.Graphics.Drawables;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace SeriesTracker;
internal class CustomShellToolbarAppearanceTracker : ShellToolbarAppearanceTracker
    {
        private readonly IShellContext shellContext;

        public CustomShellToolbarAppearanceTracker(IShellContext shellContext) : base(shellContext)
        {
            this.shellContext = shellContext;
        }

        public override void SetAppearance(Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            base.SetAppearance(toolbar, toolbarTracker, appearance);
            if (Shell.GetNavBarIsVisible(shellContext.Shell.CurrentPage))
            {
                var backgroundDrawable = new GradientDrawable();
                backgroundDrawable.SetShape(ShapeType.Rectangle);
                backgroundDrawable.SetCornerRadius(30);
                backgroundDrawable.SetColor(appearance.BackgroundColor.ToPlatform());
                toolbar.SetBackground(backgroundDrawable);

                var layoutParams = toolbar.LayoutParameters;
                if (layoutParams is ViewGroup.MarginLayoutParams marginLayoutParams)
                {
                    var margin = 0;
                    marginLayoutParams.TopMargin = margin;
                    marginLayoutParams.BottomMargin = margin;
                    marginLayoutParams.LeftMargin = margin;
                    marginLayoutParams.RightMargin = margin;
                    toolbar.LayoutParameters = layoutParams;
                }
            }
        }
    }
