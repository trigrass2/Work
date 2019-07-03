﻿using Vertical.Models;
using Vertical.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Vertical.Constants;

namespace Vertical.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InitializeObjectPage : ContentPage
	{
		public InitializeObjectPage (IsAddOrEdit isAddOrEdit, SystemObjectModel _inputObject = default(SystemObjectModel))
		{
			InitializeComponent ();
            BindingContext = new InitializeObjectPageViewModel(isAddOrEdit,_inputObject) { Navigation = this.Navigation };
		}
	}
}