using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServiceDesk.ViewModels;
using Syncfusion.XForms.ComboBox;

namespace ServiceDesk.Views
{
    /// <summary>
    /// Основная страница
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{       
        public TaskListViewModel ViewModel { get; set; }
        public MenuPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            ViewModel = new TaskListViewModel() { Navigation = this.Navigation };
            BindingContext = ViewModel;
        }

        protected async override void OnAppearing()
        {
            ViewModel.Page = 0;
            base.OnAppearing();
        }
	}
}