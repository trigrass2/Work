using System.Collections.Generic;
using Syncfusion.SfChart.XForms;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;

namespace Reports
{
    public static class ContentGenerateServices
    {
        public static SfChart ChartByTime<T>(List<T> itemsSourceList, Dictionary<string, string> keyIsYAxisValueIsLabel, KeyValuePair<string,string> bindingKeyXAxisValueIsName)
        {
            double factor = 1;
            if (keyIsYAxisValueIsLabel.Count <= 5)
            {
                factor = 0.3;
            }
            else factor = 0.2;

            CategoryAxis FirstXAxis = new CategoryAxis()
            {
                LabelPlacement = LabelPlacement.BetweenTicks,
                Interval = 1,
                Title = new ChartAxisTitle()
                {
                    Text = bindingKeyXAxisValueIsName.Value
                },
                LabelsIntersectAction = AxisLabelsIntersectAction.MultipleRows,
                ZoomFactor = factor
            };

            NumericalAxis FirstYAxis = new NumericalAxis()
            {
                Interval = 10,
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
                    Palette = ChartColorPalette.Custom,
                    CustomBrushes = ChartStyle.GetColorsChart(keyIsYAxisValueIsLabel.Count)
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
                
            foreach (KeyValuePair<string, string> item in keyIsYAxisValueIsLabel)
            {
                data = new ColumnSeries()
                {                         
                    ItemsSource = itemsSourceList,
                    Spacing = 0.1,
                    DataMarker = new ChartDataMarker()
                    {
                        LabelStyle = new DataMarkerLabelStyle()
                        {
                            LabelPosition = DataMarkerLabelPosition.Inner,
                            FontSize = 9,
                            Margin = new Thickness() { Top = 1, Bottom = 1, Left = 1, Right = 1 }
                        }
                    },                   
                    Label = item.Value,                    
                    XBindingPath = bindingKeyXAxisValueIsName.Key,
                    YBindingPath = item.Key,                       
                    
                };                
                chart.Series.Add(data);                
            }

            return chart;
        }

        public static SfDataGrid GridByTime<T>(List<T> list, Dictionary<string, string> namesBinding, string bindingTime)
        {
            SfDataGrid grid = new SfDataGrid
            {
                AutoGenerateColumns = false,
                ItemsSource = list,
                ColumnSizer = ColumnSizer.Auto,
                GridStyle = new Dark()                
            };

            GridDateTimeColumn timeColumn = new GridDateTimeColumn
            {
                Format = "d/MM/yyyy",
                MappingName = bindingTime,
                HeaderText = "Дата"                
            };
            grid.Columns.Add(timeColumn);

            GridNumericColumn column;

            foreach (KeyValuePair<string, string> item in namesBinding)
            {
                column = new GridNumericColumn
                {                                      
                    MappingName = item.Key,
                    HeaderText = item.Value,
                    TextAlignment = TextAlignment.Center                    
                };
                grid.Columns.Add(column);
            }

            return grid;
        }

        
    }
}
