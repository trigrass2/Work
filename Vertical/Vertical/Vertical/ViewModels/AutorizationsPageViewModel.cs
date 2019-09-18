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

            try
            {
                using (UserDialogs.Instance.Loading("Авторизация", null, null, true, MaskType.Black))
                {
                    await Task.Run(async () =>
                    {

                        if (NetworkCheck.IsInternet())
                        {
                            _statusAutorization = await Api.GetToken(User.Login, User.Password);
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync("Отсутствует интернет-соединение");
                            return;
                        }
                    });

                    switch (_statusAutorization)
                    {
                        case HttpStatusCode.OK:
                            {
                                Login = User?.Login;
                                Password = User?.Password;
                                await Navigation.PushAsync(new ManualObjectsPage());
                                States = States.Normal;
                                IsEnabled = true;
                            }
                            break;

                        case HttpStatusCode.InternalServerError:
                            {
                                await UserDialogs.Instance.AlertAsync("Сервер временно не доступен");
                                States = States.Normal;
                                IsEnabled = true;
                            }
                            break;

                        case HttpStatusCode.BadRequest:
                            {
                                await UserDialogs.Instance.AlertAsync("Неверный логин или пароль");
                                States = States.Normal;
                                IsEnabled = true;
                            }
                            break;

                        case 0: IsEnabled = true; break;

                        default:
                            {
                                IsEnabled = true;
                                await UserDialogs.Instance.AlertAsync("Ошибка входа");
                            }
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                await Loger.WriteMessageAsync(Android.Util.LogPriority.Error, errorMessage: ex.Message);
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
