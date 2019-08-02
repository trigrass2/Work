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
using Acr.UserDialogs;

namespace Vertical.ViewModels
{
    public class AutorizationsPageViewModel : BaseViewModel
    {        
        public static ISettings AppSettings => CrossSettings.Current;
        public ICommand SignInCommand { get; set; }
        
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

        public ImageSource PikLogoImage { get; set; } = SvgImageSource.FromSvgResource("Vertical.SvgPictures.PikGroupLogo.svg", 120, 120);

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
            States = States.Normal;
            User = new User { Login = Login, Password = Password };
            SignInCommand = new Command(SignIn);
        }

        /// <summary>
        /// Выполняет вход в приложение
        /// </summary>
        private async void SignIn()
        {            
            IsEnabled = false;

            using(UserDialogs.Instance.Loading("Авторизация", null, null, true, MaskType.Black))
            {
                await Task.Run(() => {

                    if (NetworkCheck.IsInternet())
                    {
                        _statusAutorization = Api.GetToken(User.Login, User.Password);
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("Ошибка", "Отсутствует интернет-соединение", "Ок");
                        return;
                    }
                });

                switch (_statusAutorization)
                {
                    case HttpStatusCode.OK:
                        {
                            Login = User?.Login;
                            Password = User?.Password;
                            await Navigation.PushAsync(new ManualObjectsPage());//new MenuPage());
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
            }
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
