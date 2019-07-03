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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">родительский объект</param>
		public ManualPage (SystemObjectModel obj = default(SystemObjectModel))
		{
			InitializeComponent ();
            ViewModel = new ManualPageViewModel(obj) { Navigation = this.Navigation };
            BindingContext = ViewModel;
		}
	}
}