using Xamarin.Forms;
using Xamarin.Forms.Svg;
using System.Windows.Input;
using Vertical.Views;
using Vertical.Services;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using System.ComponentModel;
using static Vertical.Constants;

namespace Vertical.ViewModels
{    
    public class AutorizationsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static ISettings AppSettings => CrossSettings.Current;
        public ICommand SignInCommand { get; set; }
        public INavigation Navigation { get; set; }
        
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
        public ImageSource PikLogoImage { get; set; }
        public States States { get; set; } = States.Normal;

        public bool IsEnabled { get; set; } = true;

        public AutorizationsPageViewModel()
        {           
            User = new User { Login = Login, Password = Password };
            PikLogoImage = SvgImageSource.FromSvgResource($"Vertical.SvgPictures.PikGroupLogo.svg", 150, 150);
            SignInCommand = new Command(SignIn);
        }

        /// <summary>
        /// открывает страницу Меню
        /// </summary>
        private async void SignIn()
        {
            IsEnabled = false;
            States = States.Loading;
            
            if (NetworkCheck.IsInternet())
            {
                switch (Api.GetToken(User.Login, User.Password))
                {
                    case System.Net.HttpStatusCode.OK:
                        {
                            Login = User?.Login;
                            Password = User?.Password;
                            await Navigation.PushAsync(new MenuPage());
                            States = States.Normal;
                            IsEnabled = true;
                        } break;

                    case System.Net.HttpStatusCode.InternalServerError:
                        {
                            await Application.Current.MainPage.DisplayAlert("Ошибка", "Сервер временно не доступен", "Ок");
                            States = States.Normal;
                            IsEnabled = true;
                        } break;

                    case System.Net.HttpStatusCode.BadRequest:
                        {
                            await Application.Current.MainPage.DisplayAlert("Ошибка", "Неверный логин или пароль", "Ок");
                            States = States.Normal;
                            IsEnabled = true;
                        } break;
                    default:
                        {
                            await Application.Current.MainPage.DisplayAlert("!", "Ошибка входа", "Ок");
                        } break;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Отсутствует интернет-соединение", "Ок");
            }
        }        
    }    
    
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Login { get; set; }
        public string Password { get; set; }
    }
}
