using ServiceDesk.PikApi;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServiceDesk.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoogleAuthView : ContentPage
    {        
        public GoogleAuthView()
        {
            InitializeComponent();

            var authRequest =
                "https://accounts.google.com/o/oauth2/v2/auth"
                + "?response_type=token"
                + "&scope=profile email"
                + "&redirect_uri=" + "https://apiinfo.pik-industry.ru/signin-google"
                + "&client_id=" + "1043677311551-kjl4mqjfm0dj7i1himvku9301onr7r4p.apps.googleusercontent.com"
                + "&prompt=" + "select_account";

            var webView = new WebView
            {
                Source = authRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;
            Content = webView;
        }
        
        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {            
            
            var access_token = ExtractCodeFromUrl(e.Url);
            string accessToken = string.Empty;
            if (access_token != "")
            {
                if (ServiceDeskApi.LoginExternalService(access_token) == System.Net.HttpStatusCode.OK)
                {
                    LoadPage loadPage = new LoadPage();
                    await Navigation.PushAsync(loadPage);
                    await Navigation.PushAsync(await Task.Run(() => new MenuPage()));
                    Navigation.RemovePage(loadPage);
                    Navigation.RemovePage(this);                    
                }else await DisplayAlert("Ошибка", "Неверный логин или пароль", "Ок");
            }
        }

        private string ExtractCodeFromUrl(string url)
        {
            if (url.Contains("access_token="))
            {
                var attributes = url.Split('&');
                var access_token = attributes.FirstOrDefault(s => s.Contains("access_token=")).Split('=')[1];
                return access_token;
            }
            return string.Empty;
        }
    }
}