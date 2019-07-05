using Vertical.CustomViews;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutorizationsPage : ContentPage
	{
        
		public AutorizationsPage ()
		{
            
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
            
            BindingContext = new AutorizationsPageViewModel() { Navigation = this.Navigation };
        }

        protected override void OnAppearing()
        {
            //DependencyService.Get<IStatusBar>().HideStatusBar();
            base.OnAppearing();
        }
    }
}