using Syncfusion.Windows.Forms.Barcode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDczMjhAMzEzNjJlMzMyZTMwUDdkL2FpeVcxdFJ1VW01NHBtYlJ5cktmNHNTMTh2a1REMlh1NlpTRVNtdz0=;NDczMjlAMzEzNjJlMzMyZTMwaGVDTXRQbGJxb3RyZjMyRXc0L2F1akhCdlQ3OGNkUlNNMElqNWQ0YW9qND0=");
            InitializeComponent();

            Code39Setting code39Settings = new Code39Setting();
            code39Settings.BarHeight = 100;
            code39Settings.NarrowBarWidth = 1;
            SfBarcode sfBarcode = new SfBarcode
            {                
                SymbologySettings = code39Settings,
                Text = "1234"
            };
            this.Controls.Add(sfBarcode);
        }
    }
}
