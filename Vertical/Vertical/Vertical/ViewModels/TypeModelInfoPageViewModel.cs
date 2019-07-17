using Android.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
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
        public ICommand GoBackCommand => new Command(GoBack);
        public ICommand UnBindSystemPropertyFromObjectTypeCommand => new Command(UnBindSystemPropertyFromObjectType);

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
        public ObservableCollection<SystemObjectTypePropertyModel> SystemPropertyModels { get; set; }
        
        public int ObjectTypeID { get; set; }

        public TypeModelInfoPageViewModel(int idTypeObject)
        {
            ObjectTypeID = idTypeObject;
            SystemPropertyModels = new ObservableCollection<SystemObjectTypePropertyModel>();
            UpdateSystemPropertyModel();
        }

        public void UpdateSystemPropertyModel()
        {
            SystemPropertyModels.Clear();

            try
            {
                foreach (var s in Api.GetDataFromServer<SystemObjectTypePropertyModel>("SystemManagement/GetSystemObjectTypeProperties", new { ObjectTypeID }))
                {
                    SystemPropertyModels.Add(s);
                }
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(LogPriority.Error, "При получении списка свойств -> ", ex.Message);
            }
            
        }

        //private async void BindSystemPropertyToObjectType()
        //{
            
        //}

        private async void UnBindSystemPropertyFromObjectType(object commandParameter)
        {
            if (await Application.Current.MainPage.DisplayAlert("Подтвердите действие", "Отвязать свойство?", "Да", "Нет") == true)
            {
                var model = commandParameter as SystemObjectTypePropertyModel;
                await Api.SendDataToServerAsync("UnBindSystemPropertyFromObjectType", new { model.PropertyID, model.PropertyNum, ObjectTypeID });
            }
        }

        public async void GoBack()
        {
            await Navigation.PopModalAsync();
        }
    }
}
