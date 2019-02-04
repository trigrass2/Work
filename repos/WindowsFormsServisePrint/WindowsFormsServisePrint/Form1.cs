using System;
using System.Windows.Forms;
using PdfSeeker;

namespace WindowsFormsServisePrint
{
    public partial class Form1 : Form
    {
        private string path = Properties.Settings.Default.folderPath;
        private string printer = Properties.Settings.Default.printerName;
        private string deleteOrNo = Properties.Settings.Default.deleteOrNotDelete;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Seeker.Run(path, printer, deleteOrNo);
        }
    }
}
