using ServiceDesk.Models;
using ServiceDesk.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServiceDesk.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilterPage : ContentPage
	{
        FilterPageViewModel _filterPageViewModel;
		public FilterPage (ServiceDesk_TaskListView filter)
		{
			InitializeComponent ();
            _filterPageViewModel = new FilterPageViewModel(filter) { Navigation = this.Navigation };
            BindingContext = _filterPageViewModel;
		}
	}
}