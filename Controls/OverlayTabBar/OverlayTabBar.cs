﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SeriesTracker.Controls.OverlayTabBar
{
    public partial class OverlayTabBar : TabBar
    {
        public static readonly BindableProperty centerViewCommandProperty = BindableProperty.Create(nameof(centerViewCommand), typeof(ICommand), typeof(OverlayTabBar), null, BindingMode.TwoWay);
        public ICommand centerViewCommand
        {
            get => (ICommand)GetValue(centerViewCommandProperty);
            set => SetValue(centerViewCommandProperty, value);
        }

        public static readonly BindableProperty centerViewImageSourceProperty = BindableProperty.Create(nameof(centerViewImageSource), typeof(ImageSource), typeof(OverlayTabBar), null, BindingMode.TwoWay);
        public ImageSource centerViewImageSource
        {
            get => (ImageSource)GetValue(centerViewImageSourceProperty);
            set => SetValue(centerViewImageSourceProperty, value);
        }

        public static readonly BindableProperty centerViewTextProperty = BindableProperty.Create(nameof(centerViewText), typeof(string), typeof(OverlayTabBar), null, BindingMode.TwoWay);
        public string centerViewText
        {
            get => (string)GetValue(centerViewTextProperty);
            set => SetValue(centerViewTextProperty, value);
        }


        public static readonly BindableProperty centerViewVisibleProperty = BindableProperty.Create(nameof(centerViewVisible), typeof(bool), typeof(OverlayTabBar), null, BindingMode.TwoWay);
        public bool centerViewVisible
        {
            get => (bool)GetValue(centerViewVisibleProperty);
            set => SetValue(centerViewVisibleProperty, value);
        }

        public static readonly BindableProperty centerViewBackgroundColorProperty = BindableProperty.Create(nameof(centerViewBackgroundColor), typeof(Color), typeof(OverlayTabBar), null, BindingMode.TwoWay);
        public Color centerViewBackgroundColor
        {
            get => (Color)GetValue(centerViewBackgroundColorProperty);
            set => SetValue(centerViewBackgroundColorProperty, value);
        }
    }
}
