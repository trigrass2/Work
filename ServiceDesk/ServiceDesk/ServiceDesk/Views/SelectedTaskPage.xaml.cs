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
        public TaskViewModel TaskViewModel { get; private set; }

        public SelectedTaskPage(TaskViewModel viewModel)
        {
			InitializeComponent ();
            TaskViewModel = viewModel;
            TaskViewModel.Navigation = this.Navigation;
            BindingContext = TaskViewModel;            
        }
	}
}