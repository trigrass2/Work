using Android.Util;
using PropertyChanged;
using System;
using System.Collections.Generic;
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
        
        /// <summary>
        /// результат добавления/редактирования
        /// </summary>
        public object NewObject { get; set; }

        public string TextButton { get; set; } = "Добавить";

        /// <summary>
        /// имя метода из API
        /// </summary>
        private string NameMetod { get; set; } = "System/AddSystemObject";

        public bool IsEnabled { get; set; } = true;

        public InitializeObjectPageViewModel(IsAddOrEdit isAddOrEdit, SystemObjectModel _inputObject = default(SystemObjectModel))
        {                       
            InputObject = _inputObject;
            Initialize(isAddOrEdit);
            States = States.Normal;
        }

        private void Initialize(IsAddOrEdit _isAddOrEdit)
        {
            
            if(_isAddOrEdit == IsAddOrEdit.Add)
            {
                NewObject = new InputAddSystemObject { ParentGUID = InputObject?.GUID, TypeID = InputObject?.TypeID };
            }
            else
            {
                TextButton = "Изменить";
                NameMetod = "System/EditSystemObject";
                NewObject = new InputEditSystemObject { ObjectGUID = InputObject?.GUID, Name = InputObject?.Name };
            }
        }

        private async void AddNewObject()
        {
            IsEnabled = false;

            if (!NetworkCheck.IsInternet())
            {
                await Application.Current.MainPage.DisplayAlert("Сообщение", "Отсутствует интернет-соединение!", "Ок");
                IsEnabled = true;
                return;
            }

            if (!Api.SendDataToServer(NameMetod, NewObject))
            {
                await Application.Current.MainPage.DisplayAlert("Сообщение", "Не удалось создать.", "Ок");
                IsEnabled = true;
                return;
            }

            try
            {
                NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
                IReadOnlyList<Page> navStack = navPage.Navigation.NavigationStack;
                var manualPage = navStack[navPage.Navigation.NavigationStack.Count - 1] as ManualPage;
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
