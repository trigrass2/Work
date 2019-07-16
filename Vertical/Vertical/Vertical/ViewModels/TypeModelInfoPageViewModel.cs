using Android.Util;
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
    public class TypeModelInfoPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.Normal;

        private SystemObjectTypeModel _selectedSystemObjectTypeModel;
        public SystemObjectTypeModel SelectedSystemObjectTypeModel
        {
            get
            {
                return _selectedSystemObjectTypeModel;
            }
            set
            {
                
                _selectedSystemObjectTypeModel = value;
                
            }
        }
        public ObservableCollection<SystemPropertyModel> SystemPropertyModels { get; set; }

        public TypeModelInfoPageViewModel(int idTypeObject)
        {            
            SystemPropertyModels = new ObservableCollection<SystemPropertyModel>();
            UpdateSystemPropertyModel(idTypeObject);
        }

        public void UpdateSystemPropertyModel(int idTypeObject)
        {
            SystemPropertyModels.Clear();

            try
            {
                foreach (var s in Api.GetDataFromServer<SystemPropertyModel>("SystemManagement/GetSystemProperties", new { ObjectTypeID = idTypeObject}))
                {
                    SystemPropertyModels.Add(s);
                }
            }
            catch (Exception ex)
            {

                Loger.WriteMessage(LogPriority.Error, "При получении списка свойств -> ", ex.Message);
            }
            
        }
    }
}
