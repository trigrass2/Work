using Plugin.Settings;
using Plugin.Settings.Abstractions;
using ServiceDesk.Models;
using ServiceDesk.PikApi;
using ServiceDesk.Views;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ServiceDesk.ViewModels
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static ISettings AppSettings => CrossSettings.Current;
        public INavigation Navigation { get; set; }
        public ICommand OkCommand { get; set; }

        public User User { get; set; }
        
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

        public AuthViewModel()
        {            
            User = new User { Login = Login, Password = Password };
            OkCommand = new Command(SignIn);
        }
        
        private async void SignIn()
        {        
            
            if (ServiceDeskApi.Register1CProxy(ServiceDeskApi.ApiEnum.Register1CProxy, User.Login, User.Password) == System.Net.HttpStatusCode.OK)
            {
                Login = User.Login;
                Password = User.Password;
                LoadPage loadPage = new LoadPage();
                await Navigation.PushAsync(loadPage);
                await Navigation.PushAsync(await Task.Run(() => new MenuPage()));
                Navigation.RemovePage(loadPage);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Ошибка", "Неверный логин или пароль", "Ок");       
            }
        }
    }
}
