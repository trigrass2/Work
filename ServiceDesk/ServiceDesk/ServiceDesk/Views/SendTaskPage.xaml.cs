using ServiceDesk.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServiceDesk.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendTaskPage : ContentPage
	{
        public SendTaskPage ()
		{
			InitializeComponent ();
            BindingContext = new CreateTaskViewModel() { Navigation = this.Navigation };
        }
	}
}