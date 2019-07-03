using ServiceDesk.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServiceDesk.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendTaskPage : ContentPage
	{
        public CreateTaskViewModel viewModel;
        public SendTaskPage ()
		{
			InitializeComponent();
            viewModel = new CreateTaskViewModel() { Navigation = this.Navigation };
            BindingContext = viewModel;            
        }

        protected async override void OnAppearing()
        {
            //viewModel.IsBusy = true;
            if (viewModel._initializedTypes == false)
                await viewModel.UpdateTypes();
            if (viewModel._initializedFactorys == false)
                await viewModel.UpdateFactorys();
            if (viewModel._initializedUsers == false)
                await viewModel.UpdateUsers();
            //viewModel.IsBusy = false;
            base.OnAppearing();
        }
    }
}