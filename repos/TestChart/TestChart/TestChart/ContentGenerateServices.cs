using System;
using System.Collections.Generic;
using System.Text;
//using Syncfusion.ListView.XForms;
using Syncfusion.SfChart.XForms;
//using Syncfusion.SfDataGrid.XForms;
using System.Threading;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Reflection;

namespace TestChart
{
    public static class ContentGenerateServices
    {
        public static SfChart ChartByTime<T>(List<T> list, Dictionary<string,string> namesBinding, string bindingTime)
        {
            double factor = 1;
            if (namesBinding.Count <= 5)
            {
                factor = 0.3;
            }
            else factor = 0.2;
            CategoryAxis FirstXAxis = new CategoryAxis()
            {
                Interval = 1,
                LabelRotationAngle = 300,
                Title = new ChartAxisTitle()
                {
                    Text = "Дата"
                },

                ZoomFactor = factor
            };
            
            NumericalAxis FirstYAxis = new NumericalAxis()
            {
                Interval = 1,                
                Title = new ChartAxisTitle()
                {
                    Text = "Кол-во"
                }                
            };
            
            ChartLegend MyLegend = new ChartLegend()
            {
                OverflowMode = ChartLegendOverflowMode.Wrap                             
            };

            SfChart chart = new SfChart
            {
                
                ColorModel = new ChartColorModel()
                {
                    Palette = ChartColorPalette.TomatoSpectrum
                    //CustomBrushes = new ChartColorCollection()
                    //{
                    //    //UniversalStyle.GetBackgroundColorButton(),
                    //    //UniversalStyle.GetBackgroundColorSelected()
                    //}
                },

                ChartBehaviors = new ChartBehaviorCollection()
                {
                    new ChartZoomPanBehavior()
                    {
                        ZoomMode = ZoomMode.X
                    }
                },
                PrimaryAxis = FirstXAxis,
                SecondaryAxis = FirstYAxis,
                Legend = MyLegend                
            };         
                       
            ColumnSeries data;

            foreach(KeyValuePair<string,string> item in namesBinding)
            {
                data = new ColumnSeries()
                {
                    ItemsSource = list,
                    DataMarker = new ChartDataMarker(),
                    Label = item.Value,
                    XBindingPath = bindingTime,
                    YBindingPath = item.Key                                     
                };
                
                chart.Series.Add(data);
            }

            return chart;
        }
    }
}
