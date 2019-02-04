using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DataCollector;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private MasterOfData masterOfData;
       
        public Form1()
        {
            InitializeComponent();
            masterOfData = new MasterOfData();          
                       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //durToSql.StartModule();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //durToSql.StopModule();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //repToSql.StartModule();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //repToSql.StopModule();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //awmHmimesh.StartModule();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //awmHmimesh.StopModule();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            masterOfData.StartModule();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            masterOfData.StopModule();
        }
    }
}
