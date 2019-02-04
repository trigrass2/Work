using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ReadRFID
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var entry = new Entry();
            entry.BackgroundColor = Color.LightGreen;
            entry.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false);
            entry.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
            Content = entry;            
        }
    }
}
