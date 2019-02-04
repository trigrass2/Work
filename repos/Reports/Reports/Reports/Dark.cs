using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;

namespace Reports
{
    public class Dark : DataGridStyle
    {
        public Dark()
        {

        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.FromRgb(70, 68, 81);
        }

        public override Color GetHeaderForegroundColor()
        {
            return Color.FromRgb(204, 204, 204);
        }
    }   
}
