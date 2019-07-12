using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Android.Util;
using Vertical.Models;
using Vertical.Services;
using Vertical.Views;
using Xamarin.Forms;
using static Vertical.Constants;

namespace Vertical.ViewModels
{
    
    public class CreatePropertyPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }

        /// <summary>
        /// состояние страницы
        /// </summary>
        public States States { get; set; } = States.Normal;

        /// <summary>
        /// добавляет новый объект
        /// </summary>
        public ICommand AddNewPropertyCommand => new Command(AddNewObject);
        
        /// <summary>
        /// отмена добавления/редактирования
        /// </summary>
        public ICommand CancelCommand => new Command(Cancel);

        /// <summary>
        /// объект редактирования
        /// </summary>
        public SystemPropertyModel NewProperty { get; set; }

        public bool IsEnabled { get; set; } = true;
        public string TextButton { get; set; } = "Добавить";
        private string _nameMetod;

        public CreatePropertyPageViewModel(string nameMetodApi, SystemPropertyModel newProperty = default(SystemPropertyModel) )
        {
            NewProperty = newProperty;
            _nameMetod = nameMetodApi;
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
            


            if (!Api.SendDataToServer($"SystemManagement/{_nameMetod}", new { ID = NewProperty?.ID, Name=NewProperty?.Name, TypeID = NewProperty?.PropertyTypeID}))
            {
                await Application.Current.MainPage.DisplayAlert("Сообщение", "Не удалось создать.", "Ок");
                IsEnabled = true;
                return;
            }

            try
            {
                NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
                IReadOnlyList<Page> navStack = navPage.Navigation.NavigationStack;
                var manualPage = navStack[navPage.Navigation.NavigationStack.Count - 1] as ManualPropertiesPage;
                manualPage.ViewModel.States = States.Loading;
                manualPage.ViewModel.UpdateSystemPropertyModels();
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(AddNewObject)}", $"{ex.Message}");
            }

            await Navigation.PopModalAsync();
        }


        private async void Cancel()
        {
            await Navigation.PopModalAsync();
        }
    }
}
