using Plugin.Settings;
using Plugin.Settings.Abstractions;
using ServiceDesk.Models;
using ServiceDesk.PikApi;
using ServiceDesk.Views;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace ServiceDesk.ViewModels
{
    public class LocalAuthViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static ISettings AppSettings => CrossSettings.Current;
        public INavigation Navigation { get; set; }
        public ICommand OkCommand { get; set; }

        public static string Login
        {
            get => AppSettings.GetValueOrDefault(nameof(Login), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Login), value);
        }
        public static string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }

        public User User { get; set; }

        public LocalAuthViewModel()
        {
            User = new User { Login = Login, Password = Password };
            OkCommand = new Command(SignIn);
        }

        private async void SignIn()
        {

            if (ServiceDeskApi.LocalRegister(User.Login, User.Password) == System.Net.HttpStatusCode.OK)
            {
                Login = User.Login;
                Password = User.Password;
                await Navigation.PushAsync(new MenuPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Ошибка", "Неверный логин или пароль", "Ок");
            }
        }
    }
}
