using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertical.Models;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditObjectPage : ContentPage
    {
        public EditObjectPage(SystemObjectModel inputObj)
        {
            InitializeComponent();
            BindingContext = new EditObjectPageViewModel(inputObj) { Navigation = this.Navigation };
        }
    }
}