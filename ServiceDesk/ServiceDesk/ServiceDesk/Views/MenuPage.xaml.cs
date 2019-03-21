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
            //_viewModel.IsBusy = true;
            //_viewModel.IsVisibleContent = false;
            await _viewModel.UpdateStatuses();
            await _viewModel.UpdateTasksAsync(_viewModel.SelectedStatus.Status_id);
            //_viewModel.IsBusy = false;
            //_viewModel.IsVisibleContent = true;
            base.OnAppearing();
        }
	}
}