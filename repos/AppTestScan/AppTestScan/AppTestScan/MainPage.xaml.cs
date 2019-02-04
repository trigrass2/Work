using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace AppTestScan
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Document> lv_Documents = new ObservableCollection<Document>();

        public MainPage()
        {
            InitializeComponent();

            Resources["lv_DocNumber_col_width"] = "100";
            Resources["lv_DocData_col_width"] = "80";
            Resources["lv_DocValue0_col_width"] = "40";
            Resources["lv_DocValue1_col_width"] = "70";
            Resources["lv_DocValue2_col_width"] = "0";

            lv_Documents = new ObservableCollection<Document> {
                new Document { Number = "0000000001", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000002", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000003", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000001", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000002", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000003", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000001", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000002", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000003", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000001", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000002", Date = "05.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
                new Document { Number = "0000000010", Date = "06.06.2017", Value0="Balakhna", Value1 = "111", Value2 = "Smth"},
            };

            Documents.ItemsSource = lv_Documents;

            new CommonProcs().DefaultSettingsToPtopertyes();


            ToolbarItems.Add(new ToolbarItem("Button_Settings", "Settings.png", async () =>
            {
                if (((string)new CommonProcs().GetProperty("ext_SupervisorPassword")).Length == 0)
                {
                    Navigation.PushAsync(new Page_Settings(this));
                }
                else
                {
                    Navigation.PushAsync(new LoginPasswordRequest(this));
                }
            }, ToolbarItemOrder.Default, 1));

            ToolbarItems.Add(new ToolbarItem("Button_Refresh", "refresh_72_72.png", async () => {
                Device.BeginInvokeOnMainThread(UpdateList);
            }, ToolbarItemOrder.Default, 0));

            AppGlobals.refCurrentPageContext = this;

            AppGlobals.refMainPageContext = this;
        }

        private void Documents_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Navigation.PushAsync(new Page_GoodsList(this, (Document)((ListView)sender).SelectedItem));
                ((ListView)sender).SelectedItem = null;
            }
        }
        protected override void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(UpdateList);
            AppGlobals.refCurrentPageContext = this;
        }

        public async void UpdateList()
        {
            try
            {
                Exchange EE = new Exchange(this);
                bool result = await EE.GetDocumentsList();
            }
            catch (Exception e) // handle whatever exceptions you expect
            {
                //Handle exceptions
            }
        }
    }

    public class Document
    {
        public string Metadata { get; set; }
        public string UID { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public string Value0 { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }
}
