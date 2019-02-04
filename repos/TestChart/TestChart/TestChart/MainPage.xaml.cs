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

namespace TestChart
{
	public partial class MainPage : ContentPage
	{
        public List<PlanFactPlantReportsView> itemsList = new List<PlanFactPlantReportsView>();
        public Dictionary<string,string> namesAttributes = new Dictionary<string, string>();
        Random rand = new Random();                                                                                   
                                                                                                         
        public MainPage()                                                                   
        {                                                                      
            InitializeComponent();

            namesAttributes.Add("ReportPlan1", "Что-то1");
            namesAttributes.Add("ReportFact2", "Что-то2");
            namesAttributes.Add("ReportPlan3", "Что-то3");
            namesAttributes.Add("ReportFact4", "Что-то4");
            namesAttributes.Add("ReportPlan5", "Что-то5");
            namesAttributes.Add("ReportFact6", "Что-то6");
            namesAttributes.Add("ReportPlan7", "Что-то7");
            namesAttributes.Add("ReportFact8", "Что-то8");
            namesAttributes.Add("ReportPlan9", "Что-то9");
            namesAttributes.Add("ReportFact10", "Что-то10");
            namesAttributes.Add("ReportPlan11", "Что-то11");
            namesAttributes.Add("ReportFact12", "Что-то12");
            namesAttributes.Add("ReportPlan13", "Что-то13");
            namesAttributes.Add("ReportFact14", "Что-то14");
            namesAttributes.Add("ReportPlan15", "Что-то15");

            for (int i = 0, k = 0; i < 5; i++, k++)
            {
                itemsList.Add(new PlanFactPlantReportsView() {
                    ReportDateTime = DateTime.Now.AddDays(k),
                    ReportPlan1 = rand.Next(1, 1000), ReportFact2 = rand.Next(1, 20),
                    ReportFact4 = rand.Next(1, 1000),
                    ReportFact6 = rand.Next(1, 1000),
                    ReportPlan3 = rand.Next(1, 1000),
                    ReportPlan5 = rand.Next(1, 1000),
                    ReportPlan7 = rand.Next(1, 1000),
                    ReportFact8 = rand.Next(1, 1000),
                    ReportPlan9 = rand.Next(1, 1000),
                    ReportFact10 = rand.Next(1, 1000),
                    ReportPlan11 = rand.Next(1, 1000),
                    ReportFact12 = rand.Next(1, 1000),
                    ReportPlan13 = rand.Next(1, 1000),
                    ReportFact14 = rand.Next(1, 1000),
                    ReportPlan15 = rand.Next(1, 1000),
                });
            }           

            Content = ContentGenerateServices.ChartByTime(itemsList, namesAttributes, "ReportDateTime");

            
        }
        
    }
}
