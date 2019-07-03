using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServiceDesk.ViewModels;

namespace ServiceDesk.Views
{
    /// <summary>
    /// Страница авторизации 1с
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthView : ContentPage
    {        
        public AuthView()
        {
            InitializeComponent();           

            BindingContext = new AuthViewModel() { Navigation = this.Navigation };            
        }       
        
    }
}