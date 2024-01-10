using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Controls.OverlayTabBar
{
    class OverlayShellItemRenderer : ShellItemRenderer
    {
        public OverlayShellItemRenderer(IShellContext context) : base(context)
        {
        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            if (Context is not null && ShellItem is OverlayTabBar { centerViewVisible: true } tabbar)
            {
                var rootLayout = new FrameLayout(Context)
                {
                    LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                };

                rootLayout.AddView(view);
                const int middleViewSize = 150;
                var middleViewLayoutParams = new FrameLayout.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent,
                    GravityFlags.CenterHorizontal | GravityFlags.Bottom)
                {
                    BottomMargin = 100,
                    Width = middleViewSize,
                    Height = middleViewSize
                };
                var middleView = new Android.Widget.Button(Context)
                {
                    LayoutParameters = middleViewLayoutParams
                };
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
    }