using System;
using System.Globalization;
using Xamarin.Forms;

namespace Vertical.Views.Converters
{
    public class IsVisibleTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value != null && value as string != "") ? 40 : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
