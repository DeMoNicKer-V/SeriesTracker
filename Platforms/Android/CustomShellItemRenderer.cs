using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using Button = Android.Widget.Button;
using View = Android.Views.View;

namespace SeriesTracker;
internal class CustomShellItemRenderer : ShellItemRenderer
{
    public CustomShellItemRenderer(IShellContext context) : base(context)
    {
    }

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        var view = base.OnCreateView(inflater, container, savedInstanceState);
        if (Context is not null && ShellItem is CustomTabBar { centerViewVisible: true } tabbar)
        {
            var rootLayout = new FrameLayout(Context)
            {
                LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
            };

            rootLayout.AddView(view);
            const int middleViewSize = 120;
            var middleViewLayoutParams = new FrameLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent,
                GravityFlags.CenterHorizontal | GravityFlags.Bottom)
            {
                BottomMargin = 100,
                Width = middleViewSize,
                Height = middleViewSize
            };
            var middleView = new Button(Context)
            {
                LayoutParameters = middleViewLayoutParams
            };
            WeakReferenceMessenger.Default.Register<TabbarChangedMessage>(this, (r, m) =>
            {


                if (m.Value.Equals(false))
                {
                    middleView.Visibility = ViewStates.Invisible;
                }
                else
                {
                    middleView.Visibility = ViewStates.Visible;


                }


            });
            middleView.Click += delegate
            {
                tabbar.centerViewCommand?.Execute(null);
            };
            middleView.SetText(tabbar.centerViewText, TextView.BufferType.Normal);
            middleView.SetPadding(0, 0, 0, 0);
            if (tabbar.centerViewBackgroundColor is not null)
            {
                var backgroundDrawable = new GradientDrawable();
                backgroundDrawable.SetShape(ShapeType.Rectangle);
                backgroundDrawable.SetCornerRadius(middleViewSize / 2f);
                backgroundDrawable.SetColor(tabbar.centerViewBackgroundColor.ToPlatform(Colors.Transparent));
                middleView.SetBackground(backgroundDrawable);
            }

            tabbar.centerViewImageSource?.LoadImage(Application.Current!.MainPage!.Handler!.MauiContext!, result =>
            {
                middleView.SetBackground(result?.Value);
                middleView.SetMinimumHeight(0);
                middleView.SetMinimumWidth(0);
            });

            rootLayout.AddView(middleView);
            return rootLayout;
        }
        return view;
    }
}

