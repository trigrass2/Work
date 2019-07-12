﻿using Vertical.Models;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreatePropertyPage : ContentPage
	{
        public CreatePropertyPage (string nameMetodApi, SystemPropertyModel systemPropertyModel = default(SystemPropertyModel))
		{
			InitializeComponent ();
            BindingContext = new CreatePropertyPageViewModel(nameMetodApi, systemPropertyModel) { Navigation = this.Navigation };

        }
	}
}