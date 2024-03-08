using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services
{
    public class DecreaseIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Int32)value < (Int32)parameter)
            {
                return ((Int32)value - 1);
            }
            return ((Int32)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;

            // Converting back rarely happens, a lot of the converters will throw an exception
            //throw new NotImplementedException();
        }
    }
}
