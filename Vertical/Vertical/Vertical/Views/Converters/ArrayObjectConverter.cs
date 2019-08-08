using System;
using System.Collections.Generic;
using System.Globalization;
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
            try
            {
                return Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ParentGUID = (value as SystemObjectPropertyValueModel).SourceObjectParentGUID });
            }
            catch (Exception)
            {
                
                return value;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (IList<SystemObjectModel>)value;
            return value as SystemObjectPropertyValueModel;
        }
    }
}
