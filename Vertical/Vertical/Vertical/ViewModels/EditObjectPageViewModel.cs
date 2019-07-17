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
    public class EditObjectPageViewModel
    {
        public INavigation Navigation { get; set; }

        /// <summary>
        /// состояние страницы
        /// </summary>
        public States States { get; set; } = States.Loading;

        /// <summary>
        /// добавляет новый объект
        /// </summary>
        public ICommand EditObjectCommand => new Command(EditObject);

        /// <summary>
        /// отмена добавления/редактирования
        /// </summary>
        public ICommand CancelCommand => new Command(Cancel);

        /// <summary>
        /// объект редактирования
        /// </summary>
        public SystemObjectModel InputObject { get; set; }

        public InputEditSystemObject NewObject { get; set; }
        public bool IsEnabled { get; set; } = true;

        public EditObjectPageViewModel(SystemObjectModel _inputObject)
        {
            InputObject = _inputObject;
            NewObject = new InputEditSystemObject { ObjectGUID = InputObject?.GUID, Name = InputObject?.Name };
            States = States.Normal;
        }

        private async void EditObject()
        {            
            IsEnabled = false;

            if (!NetworkCheck.IsInternet())
            {
                await Application.Current.MainPage.DisplayAlert("Сообщение", "Отсутствует интернет-соединение!", "Ок");
                IsEnabled = true;
                return;
            }

            if (!Api.SendDataToServer("System/EditSystemObject", NewObject))
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
                Log.WriteLine(LogPriority.Error, $"{nameof(EditObject)}", $"{ex.Message}");
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
