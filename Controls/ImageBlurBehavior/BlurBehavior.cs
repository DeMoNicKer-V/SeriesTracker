﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Controls
{
    public partial class BlurBehavior
    {
        public static readonly BindableProperty RadiusProperty = BindableProperty.Create(nameof(Radius), typeof(float), typeof(BlurBehavior), 5f, propertyChanged: OnRadiusChanged);

        public float Radius
        {
            get => (float)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        static void OnRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (BlurBehavior)bindable;
            if (behavior.imageView is null)
            {
                return;
            }

            behavior.SetRendererEffect(behavior.imageView, Convert.ToSingle(newValue));
        }
    }
}
