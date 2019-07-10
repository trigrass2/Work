using Xamarin.Forms;
using Xamarin.Forms.Svg;
using System.Windows.Input;
using Vertical.Views;
using Vertical.Services;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using System.ComponentModel;
using static Vertical.Constants;
using System.Threading.Tasks;
using System.Net;

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

        /// <summary>
        /// Аккаунт пользователя
        /// </summary>
        public User User { get; set; }

        public ImageSource PikLogoImage { get; set; }

        /// <summary>
        /// Статус страницы
        /// </summary>
        public States States { get; set; } = States.Normal;

        /// <summary>
        /// вкл/выкл кнопки
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Запуск индикатора активности
        /// </summary>
        public bool IsRunning { get; set; } = false;

        /// <summary>
        /// статус авторизации
        /// </summary>
        private HttpStatusCode _statusAutorization { get; set; }

        public AutorizationsPageViewModel()
        {           
            User = new User { Login = Login, Password = Password };
            PikLogoImage = SvgImageSource.FromSvgResource($"Vertical.SvgPictures.PikGroupLogo.svg", 150, 150);
            SignInCommand = new Command(SignIn);
        }

        /// <summary>
        /// Выполняет вход в приложение
        /// </summary>
        private async void SignIn()
        {
            IsRunning = true;

            IsEnabled = false;

            await Task.Run(()=> {

                if (NetworkCheck.IsInternet())
                {
                    _statusAutorization = Api.GetToken(User.Login, User.Password);
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Ошибка", "Отсутствует интернет-соединение", "Ок");
                }
            });

            switch (_statusAutorization)
            {
                case HttpStatusCode.OK:
                    {
                        Login = User?.Login;
                        Password = User?.Password;
                        await Navigation.PushAsync(new MenuPage());
                        States = States.Normal;
                        IsEnabled = true;
                    }
                    break;

                case HttpStatusCode.InternalServerError:
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Сервер временно не доступен", "Ок");
                        States = States.Normal;
                        IsEnabled = true;
                    }
                    break;

                case HttpStatusCode.BadRequest:
                    {
                        await Application.Current.MainPage.DisplayAlert("Ошибка", "Неверный логин или пароль", "Ок");
                        States = States.Normal;
                        IsEnabled = true;
                    }
                    break;
                default:
                    {
                        IsEnabled = true;
                        await Application.Current.MainPage.DisplayAlert("!", "Ошибка входа", "Ок");                        
                    }
                    break;
            }

            IsRunning = false;
        }
    }    
    
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
