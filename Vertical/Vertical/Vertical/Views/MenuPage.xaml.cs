using Vertical.CustomViews;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
    /// <summary>
    /// Страница меню
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        private MenuPageViewModel ViewModel;

		public MenuPage ()
		{           
            InitializeComponent();
            //.Get<IStatusBar>().ShowStatusBar();
            ViewModel = new MenuPageViewModel() { Navigation = this.Navigation };
            BindingContext = ViewModel;           
        }        
    }
}