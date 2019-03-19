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
            await viewModel.UpdateTypes();
            await viewModel.UpdateFactorys();
            await viewModel.UpdateUsers();
            base.OnAppearing();
        }
    }
}