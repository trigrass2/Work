using ServiceDesk.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServiceDesk.Views
{
    /// <summary>
    /// Страница просмотра заявки
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectedTaskPage : ContentPage
	{
        public TaskViewModel TaskViewModel { get; set; }

        public SelectedTaskPage(TaskViewModel viewModel)
        {
			InitializeComponent ();
            TaskViewModel = viewModel;
            TaskViewModel.Navigation = this.Navigation;
            this.BindingContext = TaskViewModel;            
        }

        protected async override void OnAppearing()
        {
            await TaskViewModel.UpdateStatuses();

            if (TaskViewModel.ServiceDesk_TaskListView.Factory_name == null || TaskViewModel.ServiceDesk_TaskListView.Factory_name == "")
            {
                TaskViewModel.IsVisibleFactory = false;
            };
            if (TaskViewModel.ServiceDesk_TaskListView.Plant_name == null || TaskViewModel.ServiceDesk_TaskListView.Plant_name == "")
            {
                TaskViewModel.IsVisiblePlant = false;
            };
            if (TaskViewModel.ServiceDesk_TaskListView.Unit_name == null || TaskViewModel.ServiceDesk_TaskListView.Unit_name == "")
            {
                TaskViewModel.IsVisibleUnit = false;
            };
            if (TaskViewModel.Attachments == null || TaskViewModel.Attachments.Count == 0)
            {
                TaskViewModel.IsVisibleAttach = false;
            };
            base.OnAppearing();
        }

        private void StatusPicker_DropDownOpen(object sender, System.EventArgs e)
        {
            if(TaskViewModel.IsEdit == false)
            TaskViewModel.IsEdit = true;
        }
    }
}