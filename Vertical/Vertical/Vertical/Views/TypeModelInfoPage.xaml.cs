using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vertical.ViewModels;
using Vertical.Models;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TypeModelInfoPage : ContentPage
	{
		public TypeModelInfoPage (SystemObjectTypeModel idTypeObj)
		{
			InitializeComponent ();
            BindingContext = new TypeModelInfoPageViewModel(idTypeObj.ID) { Navigation = this.Navigation };
		}
	}
}