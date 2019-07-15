using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Vertical.Models;
using Vertical.Services;
using Xamarin.Forms;
using static Vertical.Constants;

namespace Vertical.ViewModels
{
    public class ManualTypesObjectsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SystemObjectTypeModel> SystemObjectTypesModels { get; set; }

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.Normal;

        public ManualTypesObjectsPageViewModel()
        {
            SystemObjectTypesModels = new ObservableCollection<SystemObjectTypeModel>();
            UpdateSystemObjectTypesModels();
        }

        private void UpdateSystemObjectTypesModels()
        {
            SystemObjectTypesModels.Clear();

            foreach(var t in Api.GetDataFromServer<SystemObjectTypeModel>("System/GetSystemObjectTypes"))
            {
                SystemObjectTypesModels.Add(t);
            }
        }

    }
}
