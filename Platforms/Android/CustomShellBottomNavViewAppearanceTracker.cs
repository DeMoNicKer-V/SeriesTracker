using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using Color = Microsoft.Maui.Graphics.Color;

namespace SeriesTracker;

internal class CustomShellBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
{
    private readonly IShellContext shellContext;

    public CustomShellBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(
        shellContext, shellItem)
    {
        this.shellContext = shellContext;
    }

    public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
    {
        base.SetAppearance(bottomView, appearance);
        if (Shell.GetTabBarIsVisible(shellContext.Shell.CurrentPage))
        {
            // Set custom font with any font file name you need as registered in MauiProgram.cs builder.ConfigureFonts()
            var font = Typeface.CreateFromAsset(shellContext.AndroidContext.Assets, "Nunito-Regular.ttf");
            // replace FONTSIZE with you needed font size
            var spanFace = new CustomTypefaceSpan("", font, 30);
            // if you need you can change the direction of the bottom navigation view
            bottomView.ItemIconSize = 70;

            var menu = bottomView.Menu;
            for (var i = 0; i < menu.Size(); i++)
            {
                // Set custom label with custom fonts
                var menuItem = menu.GetItem(i);
                if (menuItem == null) continue;
                var spannableString = new SpannableString(menuItem.TitleCondensedFormatted);
                spannableString.SetSpan(spanFace, 0, spannableString.Length(), SpanTypes.ExclusiveExclusive);
                menuItem.SetTitle(spannableString);
            }

            var layoutParams = bottomView.LayoutParameters;
            if (layoutParams is ViewGroup.MarginLayoutParams marginLayoutParams)
            {
                const int margin = 0;
                marginLayoutParams.BottomMargin = margin;
                marginLayoutParams.LeftMargin = margin;
                marginLayoutParams.RightMargin = margin;
                bottomView.LayoutParameters = layoutParams;
                bottomView.SetPadding(0, 0, 0, 100);
            }
        }
    }

    protected override void SetBackgroundColor(BottomNavigationView bottomView, Color color)
    {
        base.SetBackgroundColor(bottomView, color);
        bottomView.RootView?.SetBackgroundColor(shellContext.Shell.CurrentPage.BackgroundColor.ToPlatform());
    }
}

public class CustomTypefaceSpan : TypefaceSpan
{
    private float? FontSize { get; }
    private readonly Typeface _typeface;

    public CustomTypefaceSpan(string family, Typeface typeface, float? fontSize = null) : base(family)
    {
        FontSize = fontSize;
        _typeface = typeface;
    }

    public override void UpdateDrawState(TextPaint ds)
    {
        ApplyCustomTypeFace(ds, _typeface);
    }

    public override void UpdateMeasureState(TextPaint paint)
    {
        ApplyCustomTypeFace(paint, _typeface);
    }

    private void ApplyCustomTypeFace(Android.Graphics.Paint paint, Typeface tf)
    {
        if (FontSize != null && FontSize != 0)
            paint.TextSize = FontSize.Value;
        paint.SetTypeface(tf);
    }
}