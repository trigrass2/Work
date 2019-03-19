using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServiceDesk.ViewModels;

namespace ServiceDesk.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{
		public StartPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new StartPageViewModel() { Navigation = this.Navigation };

        }
	}
}