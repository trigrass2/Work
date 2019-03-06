using Plugin.FilePicker;
using ServiceDesk.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ServiceDesk.Views
{	
	public partial class EditTaskPage : ContentPage
	{
		public EditTaskPage()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new TaskListViewModel() { Navigation = this.Navigation };
        }
        
    }
}