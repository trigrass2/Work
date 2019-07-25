using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vertical.Models;
using Vertical.ViewModels;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckListPage : ContentPage
	{        
        public CheckPageViewModel ViewModel { get; set; }
        public CheckListPage(SystemObjectModel systemObjectModel)
		{
			InitializeComponent ();
            ViewModel = new CheckPageViewModel(systemObjectModel) { Navigation = this.Navigation };
            
            Content = new CheckListView(ViewModel);
		}
    }
}