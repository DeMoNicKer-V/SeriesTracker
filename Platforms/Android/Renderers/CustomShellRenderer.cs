using Android.Content;
using Android.Graphics;
using Android.Text.Style;
using Android.Text;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Platforms.Android.Renderers
{
    class CustomShellRenderer : ShellRenderer
    {
        public CustomShellRenderer(Context context) : base(context)
        {
        }
        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new CustomBottomNavViewAppearanceTracker(this, shellItem);
        }
    }
    public class CustomBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
    {
        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);

            // Set custom font with any font file name you need as registered in MauiProgram.cs builder.ConfigureFonts()
            var font = Typeface.CreateFromAsset(ShellContext.AndroidContext.Assets, "Nunito-Regular.ttf");
            // replace FONTSIZE with you needed font size
            var spanFace = new CustomTypefaceSpan("", font, 35);
            // if you need you can change the direction of the bottom navigation view
            bottomView.ItemIconSize = 70;
            bottomView.LayoutDirection = (global::Android.Views.LayoutDirection)LayoutDirection.RightToLeft;
            bottomView.SetPadding(10,0,10,77);
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

        }
        public CustomBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
        {
            ShellContext = shellContext;
        }
        private IShellContext ShellContext { get; }
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
