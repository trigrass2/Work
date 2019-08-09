using System.Collections.Generic;
using Vertical.Models;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManualObjectsPage : ContentPage
	{
        public ManualObjectsPageViewModel ViewModel { get; set; }
        
		public ManualObjectsPage (SystemObjectModel obj = default(SystemObjectModel), string typePage = "Шаблоны")
		{
			InitializeComponent ();
            ViewModel = new ManualObjectsPageViewModel(obj, typePage) { Navigation = this.Navigation };
            BindingContext = ViewModel;
		}
	}
}