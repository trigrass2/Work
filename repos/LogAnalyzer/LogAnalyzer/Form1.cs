using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LogParser;

namespace LogAnalyzer
{
    public partial class Form1 : Form
    {
        List<LogFileModel> logsData;

        public Form1()
        {            
            InitializeComponent();            
        }  

        private void openFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                logsData = Controller.GetTable(openFileDialog1.FileName);                
                sfDataGrid.DataSource = logsData;
                sfDataGrid.Columns[1].Width = 175;
                sfDataGrid.Columns[4].Width = 350;
                sfDataGrid.Columns[5].Width = 350;
                getErrorsButton.Enabled = true;
                refreshButton.Enabled = true;                
            }
        }
        
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            openFileButton.Top = (splitContainer1.Panel1.Height - openFileButton.Height) / 2;            
            getErrorsButton.Top = (splitContainer1.Panel1.Height - getErrorsButton.Height) / 2;
            refreshButton.Top = (splitContainer1.Panel1.Height - refreshButton.Height) / 2;
        }

        private void getErrorsButton_Click(object sender, EventArgs e)
        {
            sfDataGrid.DataSource = logsData.FindAll(x => x.CodeServiceState != 200);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            sfDataGrid.DataSource = logsData;
        }
    }
}
