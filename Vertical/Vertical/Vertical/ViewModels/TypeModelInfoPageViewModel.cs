using Android.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Vertical.CustomViews;
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
        public ICommand BindSystemPropertyToObjectTypeCommand => new Command(BindSystemPropertyToObjectType);

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
        public ObservableCollection<GroupingModel<SystemObjectTypePropertyModel>> SystemPropertyModels { get; set; }
        
        public int ObjectTypeID { get; set; }

        public TypeModelInfoPageViewModel(int idTypeObject)
        {
            ObjectTypeID = idTypeObject;
            SystemPropertyModels = new ObservableCollection<GroupingModel<SystemObjectTypePropertyModel>>();
            UpdateSystemPropertyModel();
        }

        public void UpdateSystemPropertyModel()
        {
            SystemPropertyModels.Clear();
            var items = Api.GetDataFromServer<SystemObjectTypePropertyModel>("SystemManagement/GetSystemObjectTypeProperties", new { ObjectTypeID });                     
            var groups = items.Select(x => x.GroupName).Distinct();
            
            try
            {
                
                foreach(var s in groups.AsParallel().Select(x => GetGroup(x, items)))
                {
                    SystemPropertyModels.Add(s);
                }
                
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(LogPriority.Error, "При получении списка свойств -> ", ex.Message);
            }
            
        }
        private GroupingModel<SystemObjectTypePropertyModel> GetGroup(string nameGroup, IList<SystemObjectTypePropertyModel> items)
        {
            GroupingModel<SystemObjectTypePropertyModel> groupProperties = new GroupingModel<SystemObjectTypePropertyModel>(nameGroup);

            foreach(var i in items.Where(x => x.GroupName == nameGroup))
            {
                groupProperties.Add(i);
            }
            return groupProperties;
        }
        /// <summary>
        /// Привязывает новое свойство
        /// </summary>
        /// <param name="commandParameter"></param>
        private async void BindSystemPropertyToObjectType(object commandParameter)
        {
            var groupId = commandParameter as ObservableCollection<SystemObjectTypePropertyModel>;
            var items = await Api.GetDataFromServerAsync<SystemPropertyModel>("SystemManagement/GetSystemProperties", new { });
            
            var action = await Application.Current.MainPage
                                                  .DisplayActionSheet(
                                                  "Новое свойство свойство",
                                                  "Отмена",
                                                  null,
                                                  items.Select(x => x.Name).ToArray());

            if (action != null && action != "Отмена")
            {
                var newProperty = items.SingleOrDefault(i => i.Name == action);
                if (await Api.SendDataToServerAsync("SystemManagement/BindSystemPropertyToObjectType", new { PropertyID = newProperty.ID, ObjectTypeID, groupId[0].GroupID }) == System.Net.HttpStatusCode.OK)
                {
                    UpdateSystemPropertyModel();
                    await Application.Current.MainPage.DisplayAlert("Сообщение", "Свойство добавлено", "Ок");                    
                }
            }
            
        }

        /// <summary>
        /// Отвязывает свойство
        /// </summary>
        /// <param name="commandParameter"></param>
        private async void UnBindSystemPropertyFromObjectType(object commandParameter)
        {
            if (await Application.Current.MainPage.DisplayAlert("Подтвердите действие", "Отвязать свойство?", "Да", "Нет") == true)
            {
                var model = commandParameter as SystemObjectTypePropertyModel;
                await Api.SendDataToServerAsync("SystemManagement/UnBindSystemPropertyFromObjectType", new { model.PropertyID, model.PropertyNum, ObjectTypeID });
                UpdateSystemPropertyModel();
            }
        }

        public async void GoBack()
        {
            await Navigation.PopModalAsync();
        }
    }
}
