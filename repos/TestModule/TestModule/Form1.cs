using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataAllocator;
using GalaSoft.MvvmLight;

namespace TestModule
{
    public partial class Form1 : Form
    {                   
        public delegate void PrinterTextBox(LastEntry lastEntry, TextBox textBox);
        public PrinterTextBox printerTextBox;
        public delegate void PrinterLabel(LastEntry state, Label label);
        public PrinterLabel printerLabel;

        public AwmRepDurToSql durToSql;
        public AwmRepToSql repToSql;
        public AwmHmimeshToSql awmHmimesh;
        public BSUErrorsToMES bsuErrorsToMES;
        public PULeitToMES leitToMES;
        public PU_Errors leitErrors;
        public BSUProdRepToSql bSUProduction;
        public AwmHmiCraneToSql awmHmiCran;

        private BackgroundWorker bw_rep;
        private BackgroundWorker bw_log;
        private BackgroundWorker bw_dur;
        private BackgroundWorker bw_bsu_errors;
        private BackgroundWorker bw_leit;
        private BackgroundWorker bw_leit_errors;
        private BackgroundWorker bw_bsu_productions;
        private BackgroundWorker bw_hmiCran;

        public Form1()
        {
            InitializeComponent();            

            printerTextBox = new PrinterTextBox(PrintInTextBox);
            printerLabel = new PrinterLabel(PrintInLable);

            awmHmiCran = new AwmHmiCraneToSql();
            leitToMES = new PULeitToMES();
            durToSql = new AwmRepDurToSql();
            repToSql = new AwmRepToSql();
            awmHmimesh = new AwmHmimeshToSql();
            bsuErrorsToMES = new BSUErrorsToMES();
            leitErrors = new PU_Errors();
            bSUProduction = new BSUProdRepToSql();

            bw_hmiCran = new BackgroundWorker();
            bw_hmiCran.DoWork += awmHmiCran.StartModule;

            bw_leit_errors = new BackgroundWorker();
            bw_leit_errors.DoWork += leitErrors.StartModule;

            bw_leit = new BackgroundWorker();
            bw_leit.DoWork += leitToMES.StartModule;

            bw_rep = new BackgroundWorker();
            bw_rep.DoWork += repToSql.StartModule;

            bw_log = new BackgroundWorker();
            bw_log.DoWork += awmHmimesh.StartModule;

            bw_dur = new BackgroundWorker();
            bw_dur.DoWork += durToSql.StartModule;

            bw_bsu_errors = new BackgroundWorker();
            bw_bsu_errors.DoWork += bsuErrorsToMES.StartModule;

            bw_bsu_productions = new BackgroundWorker();
            bw_bsu_productions.DoWork += bSUProduction.StartModule;

            PrintInLable(awmHmiCran.stateModule, labelHmiCran);
            PrintInLable(leitErrors.stateModule, labelLeitError);
            PrintInLable(leitToMES.stateModule, labelLeitState);
            PrintInLable(durToSql.stateModule, labelDurState);
            PrintInLable(repToSql.stateModule, labelRepState);
            PrintInLable(awmHmimesh.stateModule, labelLogState);
            PrintInLable(bsuErrorsToMES.stateModule, labelBSUErrorsState);
            PrintInLable(bSUProduction.stateModule, labelBsuProd);            

            PrintInTextBox(leitErrors.lastEntryOne, textBox16);
            PrintInTextBox(leitErrors.lastEntryTwo, textBox11);
            PrintInTextBox(leitErrors.lastEntryThree, textBox6);

            PrintInTextBox(durToSql.lastEntryOne, textBox1);
            PrintInTextBox(durToSql.lastEntryTwo, textBox10);
            PrintInTextBox(durToSql.lastEntryThree, textBox15);

            PrintInTextBox(repToSql.lastEntryOne, textBox2);
            PrintInTextBox(repToSql.lastEntryTwo, textBox9);
            PrintInTextBox(repToSql.lastEntryThree, textBox14);

            PrintInTextBox(awmHmimesh.lastEntryOne, textBox3);
            PrintInTextBox(awmHmimesh.lastEntryTwo, textBox8);
            PrintInTextBox(awmHmimesh.lastEntryThree, textBox13);

            PrintInTextBox(awmHmiCran.lastEntryOne, textBox20);
            PrintInTextBox(awmHmiCran.lastEntryTwo, textBox19);
            PrintInTextBox(awmHmiCran.lastEntryThree, textBox18);

            PrintInTextBox(leitToMES.lastEntryOne, textBox4);
            PrintInTextBox(leitToMES.lastEntryTwo, textBox7);
            PrintInTextBox(leitToMES.lastEntryThree, textBox12);

            PrintInTextBox(bsuErrorsToMES.lastEntry, textBox5);
            PrintInTextBox(bSUProduction.lastEntry, textBox17);            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bw_hmiCran.RunWorkerAsync();
            bw_rep.RunWorkerAsync();
            bw_log.RunWorkerAsync();
            bw_dur.RunWorkerAsync();
            bw_bsu_errors.RunWorkerAsync();
            bw_leit.RunWorkerAsync();
            bw_leit_errors.RunWorkerAsync();
            bw_bsu_productions.RunWorkerAsync();
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            awmHmiCran.StopModule();
            durToSql.StopModule();
            repToSql.StopModule();
            awmHmimesh.StopModule();
            bsuErrorsToMES.StopModule();
            leitToMES.StopModule();
            leitErrors.StopModule();
            bSUProduction.StopModule();
            bw_hmiCran.Dispose();
            bw_rep.Dispose();
            bw_log.Dispose();
            bw_dur.Dispose();
            bw_leit_errors.Dispose();
            bw_leit.Dispose();
            bw_bsu_errors.Dispose();
            bw_bsu_productions.Dispose();
            button1.Enabled = true;

        }
        
        private void PrintInTextBox(LastEntry data, TextBox textBox)
        {
            if (InvokeRequired)
            {
                Invoke(printerTextBox, data, textBox);
                return;
            }
            if (data != null)
            {
                textBox.DataBindings.Add("Text", data, "Message");
            }
        }

        private void PrintInLable(LastEntry state, Label label)
        {
            if (InvokeRequired)
            {
                Invoke(printerLabel, state, label);
                return;
            }
            if (state != null)
            {
                label.DataBindings.Add("Text", state, "Message");
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
