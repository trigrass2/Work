using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Vertical.Models;
using Vertical.Services;
using Xamarin.Forms;

namespace Vertical.Views.Converters
{
    public class ArrayObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = (value as SystemObjectPropertyValueModel).SourceObjectParentGUID});
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (IList<SystemObjectModel>)value;
            return value as SystemObjectPropertyValueModel;
        }
    }
}
