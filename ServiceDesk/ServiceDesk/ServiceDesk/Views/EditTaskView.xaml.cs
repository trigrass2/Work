using ServiceDesk.Models;
using ServiceDesk.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServiceDesk.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTaskView : ContentPage
	{
        public EditTaskViewModel _viewModel;
        public ServiceDesk_TaskListView _sv { get; set; }
        public EditTaskView (ServiceDesk_TaskListView serviceDesk_TaskListView)
		{
			InitializeComponent();

            //_sv = serviceDesk_TaskListView;
            _viewModel = new EditTaskViewModel(serviceDesk_TaskListView) { Navigation = this.Navigation };
            BindingContext = _viewModel;
        }

        protected async override void OnAppearing()
        {
            await _viewModel.UpdateTypes();
            await _viewModel.UpdateFactorys();
            await _viewModel.UpdateUsers();
            await _viewModel.UpdatePlantsAsync();
            await _viewModel.UpdateUnitsAsync();
            base.OnAppearing();
        }
	}
}