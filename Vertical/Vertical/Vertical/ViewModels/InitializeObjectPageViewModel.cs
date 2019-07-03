using PropertyChanged;
using System;
using System.Collections;
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
        public States States { get; set; } = States.Loading;
        public ICommand AddNewObjectCommand => new Command(AddNewObject);
        public ICommand CancelCommand => new Command(Cancel);
        public SystemObjectModel InputObject { get; set; }  
        public object NewObject { get; set; }
        public string TextButton { get; set; } = "Добавить";        

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
                NewObject = new AddSystemObjectModel { ParentGUID = InputObject?.GUID, TypeID = InputObject?.TypeID };
            }
            else
            {
                TextButton = "Изменить";
                NewObject = new InputEditSystemObject { ObjectGUID = InputObject?.GUID, Name = InputObject?.Name };
            }
        }

        private async void AddNewObject()
        {
            IsEnabled = false;
            if(!Api.SendData(Api.NameMetodsApi.AddSystemObject, NewObject))
            {
                await Application.Current.MainPage.DisplayAlert("Сообщение", "Не удалось создать.", "Ок");
                return;
            }
            NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
            IReadOnlyList<Page> navStack = navPage.Navigation.NavigationStack;            
            var manualPage = navStack[navPage.Navigation.NavigationStack.Count - 1] as ManualPage;
            manualPage.ViewModel.States = States.Loading;
            manualPage.ViewModel.UpdateSystemObjects();

            await Navigation.PopModalAsync();
        } 

        private async void Cancel()
        {
            IsEnabled = false;
            await Navigation.PopModalAsync();
        }
    }
}
