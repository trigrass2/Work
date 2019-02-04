using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace TestPush
{
    /// <summary>
    /// Страница авторизации
    /// </summary>
    public class AuthPage : ContentPage
    {
        Button ButtonGo;
        public AuthPage()
        {
            ButtonGo = new Button { Text = "Go!" };
            ButtonGo.Clicked += ToMainPage;
            Content = new StackLayout
            {
                Children = {
                    ButtonGo
                }
            };
        }

        private async void ToMainPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}