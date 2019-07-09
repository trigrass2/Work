using Vertical.Models;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InfoPage : ContentPage
	{
		public InfoPage (SystemObjectModel obj)
		{
			InitializeComponent ();
            BindingContext = new InfoPageViewModel(obj) { Navigation = this.Navigation};
		}
	}
}