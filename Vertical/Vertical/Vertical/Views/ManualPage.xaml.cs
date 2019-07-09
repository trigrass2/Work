using System.Collections.Generic;
using Vertical.Models;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManualPage : ContentPage
	{
        public ManualPageViewModel ViewModel { get; set; }
        
		public ManualPage (SystemObjectModel obj = default(SystemObjectModel))
		{
			InitializeComponent ();
            ViewModel = new ManualPageViewModel(obj) { Navigation = this.Navigation };
            BindingContext = ViewModel;
		}
	}
}