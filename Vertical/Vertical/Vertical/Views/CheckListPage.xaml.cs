using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vertical.Models;
using Vertical.ViewModels;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckListPage : ContentPage
	{
		public CheckListPage(SystemObjectModel systemObjectModel)
		{
			InitializeComponent ();
            BindingContext = new CheckPageViewModel(systemObjectModel) { Navigation = this.Navigation };
		}
	}
}