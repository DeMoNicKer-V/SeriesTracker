using Android.App;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Platforms.Android
{
    public class SpinDatePickerHandler : DatePickerHandler
    {
        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            var dialog = new DatePickerDialog(Context!, global::Android.Resource.Style.ThemeHoloLightDialog, (o, e) =>
            {
                if (VirtualView != null)
                {
                    VirtualView.Date = e.Date;
                }
            }, year, month, day);
            return dialog;
        }
    }
}
