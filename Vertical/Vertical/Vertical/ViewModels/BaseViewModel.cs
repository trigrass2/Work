using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using static Vertical.Constants;

namespace Vertical.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        /// <summary>
        /// Статус страницы
        /// </summary>
        public States States { get; set; } = States.Loading;

        /// <summary>
        /// вкл/выкл кнопки
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
