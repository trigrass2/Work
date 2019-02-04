using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Reports
{
    public partial class MainPage : ContentPage
    {
        public List<PlanFactPlantReportsView> itemsList = new List<PlanFactPlantReportsView>();
        public Dictionary<string, string> namesAttributes = new Dictionary<string, string>();
        public KeyValuePair<string, string> xAxis = new KeyValuePair<string, string>("ReportDateTime", "Дата");        

        Random rand = new Random();
        
        public MainPage()
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgb(70, 68, 81);
            
            namesAttributes.Add("ReportPlan1", "Что-то1");
            namesAttributes.Add("ReportFact2", "Что-то2");
            namesAttributes.Add("ReportPlan3", "Что-то3");
            namesAttributes.Add("ReportFact4", "Что-то4");
            namesAttributes.Add("ReportPlan5", "Что-то5");

            for (int i = 0, k = 0; i < 5; i++, k++)
            {
                itemsList.Add(new PlanFactPlantReportsView()
                {
                    ReportDateTime = DateTime.Now.AddDays(k),
                    ReportPlan1 = rand.Next(1, 100),
                    ReportFact2 = rand.Next(1, 100),
                    ReportFact4 = rand.Next(1, 100),                    
                    ReportPlan3 = rand.Next(1, 100),
                    ReportPlan5 = rand.Next(1, 100)
                });
            }

            Button toChartBtn = new Button
            {
                Text = "Chart",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.FromHex("#5990c6"),
                CornerRadius = 5
            };
            toChartBtn.Clicked += ToViewDataPage;

            Button toDataGridBtn = new Button
            {
                Text = "DataGrid",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.FromHex("#5990c6"),
                CornerRadius = 5
            };
            toDataGridBtn.Clicked += ToViewDataPage;            

            Content = new StackLayout { Children = { toChartBtn, toDataGridBtn } };
        }

        private async void ToViewDataPage(object sender, EventArgs e)
        {            
            Button clickedButton = (Button)sender;           

            switch (clickedButton.Text)
            {
                case "Chart": await Navigation.PushAsync(new ViewDataPage(ContentGenerateServices.ChartByTime(itemsList, namesAttributes, xAxis))); break;
                case "DataGrid": await Navigation.PushAsync(new ViewDataPage(ContentGenerateServices.GridByTime(itemsList, namesAttributes, "ReportDateTime"))); break;                
            }
        }

    }
}
