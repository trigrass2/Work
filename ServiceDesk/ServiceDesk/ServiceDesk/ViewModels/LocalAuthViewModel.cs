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
    public class LocalAuthViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static ISettings AppSettings => CrossSettings.Current;
        public INavigation Navigation { get; set; }
        public ICommand OkCommand { get; set; }

        public static string LoginLocal
        {
            get => AppSettings.GetValueOrDefault(nameof(LoginLocal), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(LoginLocal), value);
        }
        public static string PasswordLocal
        {
            get => AppSettings.GetValueOrDefault(nameof(PasswordLocal), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(PasswordLocal), value);
        }

        public User User { get; set; }

        public LocalAuthViewModel()
        {
            User = new User { Login = LoginLocal, Password = PasswordLocal };            
            OkCommand = new Command(SignIn);
        }

        private async void SignIn()
        {

            if (ServiceDeskApi.LocalRegister(User.Login, User.Password) == System.Net.HttpStatusCode.OK)
            {
                LoginLocal = User.Login;
                PasswordLocal = User.Password;
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
