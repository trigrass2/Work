using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServiceDesk.ViewModels;

namespace ServiceDesk.Views
{
    /// <summary>
    /// Основная страница
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{        
		public MenuPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new TaskListViewModel() { Navigation = this.Navigation };            
        }
	}
}