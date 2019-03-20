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
        TaskListViewModel _viewModel;
        public MenuPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            _viewModel = new TaskListViewModel() { Navigation = this.Navigation };
            BindingContext = _viewModel;
        }

        protected async override void OnAppearing()
        {
            await _viewModel.UpdateTasks();
            base.OnAppearing();
        }
	}
}