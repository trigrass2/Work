using Android.Util;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Vertical.Models;
using Vertical.Services;
using Vertical.Views;
using Xamarin.Forms;
using static Vertical.Constants;

namespace Vertical.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class InitializeObjectPageViewModel
    {
        public INavigation Navigation { get; set; }

        /// <summary>
        /// состояние страницы
        /// </summary>
        public States States { get; set; } = States.Loading;

        /// <summary>
        /// добавляет новый объект
        /// </summary>
        public ICommand AddNewObjectCommand => new Command(AddNewObject);

        /// <summary>
        /// отмена добавления/редактирования
        /// </summary>
        public ICommand CancelCommand => new Command(Cancel);

        /// <summary>
        /// объект редактирования
        /// </summary>
        public SystemObjectModel InputObject { get; set; }

        public ObservableCollection<SystemObjectTypeModel> SystemObjectTypeModels { get; set; }
        
        public SystemObjectTypeModel SelectedSystemObjectTypeModel { get; set; }
        
        /// <summary>
        /// результат добавления/редактирования
        /// </summary>
        public AddSystemObjectModel NewObject { get; set; }

        public bool IsEnabled { get; set; } = true;
        public InitializeObjectPageViewModel(SystemObjectModel _inputObject = default(SystemObjectModel))
        {
            
            SystemObjectTypeModels = new ObservableCollection<SystemObjectTypeModel>();
            InputObject = _inputObject;            
            UpdateTypes();
            NewObject = new AddSystemObjectModel { ParentGUID = InputObject?.GUID };
            States = States.Normal;
        }

        private void UpdateTypes()
        {
            SystemObjectTypeModels.Clear();

            foreach (var t in Api.GetDataFromServer<SystemObjectTypeModel>("System/GetSystemObjectTypes"))
            {
                SystemObjectTypeModels.Add(t);
            }
        }

        private async void AddNewObject()
        {
            NewObject.TypeID = SelectedSystemObjectTypeModel?.ID;
            IsEnabled = false;

            if (!NetworkCheck.IsInternet())
            {
                await Application.Current.MainPage.DisplayAlert("Сообщение", "Отсутствует интернет-соединение!", "Ок");
                IsEnabled = true;
                return;
            }

            if (!Api.SendDataToServer("System/AddSystemObject", NewObject))
            {
                await Application.Current.MainPage.DisplayAlert("Сообщение", "Не удалось создать.", "Ок");
                IsEnabled = true;
                return;
            }

            try
            {
                NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
                IReadOnlyList<Page> navStack = navPage.Navigation.NavigationStack;
                var manualPage = navStack[navPage.Navigation.NavigationStack.Count - 1] as ManualObjectsPage;
                manualPage.ViewModel.States = States.Loading;
                manualPage.ViewModel.UpdateSystemObjects();
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(AddNewObject)}", $"{ex.Message}");
            }            

            await Navigation.PopModalAsync();
        } 

        private async void Cancel()
        {
            IsEnabled = false;
            await Navigation.PopModalAsync();
        }
    }
}
