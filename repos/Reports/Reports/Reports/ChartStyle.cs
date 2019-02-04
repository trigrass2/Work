using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;

namespace Reports
{
    public static class ChartStyle
    {
        public static IList<Color> GetColorsChart(int countColumns)
        {
            Random rand = new Random();            
            List<Color> colors = new List<Color>();
            Color tempColor = Color.FromRgb(70, 68, 81);

            for (int i = 0; i < countColumns;i++)
            {
                if (!colors.Contains(tempColor))
                {
                    colors.Add(tempColor);
                }
                else i--;
                
                tempColor = Color.FromRgb(rand.Next(50, 170), rand.Next(50, 168), rand.Next(50, 181));
            }

            return colors;
        }
       
    }
}
