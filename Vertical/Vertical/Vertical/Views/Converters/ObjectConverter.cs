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
    public class ObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            try
            {
                
                return Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID = value }).Select(x => x.Name).FirstOrDefault();
            }
            catch (Exception)
            {

                return value;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as SystemObjectModel;
        }
    }
}
