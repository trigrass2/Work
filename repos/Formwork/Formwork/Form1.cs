using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CADUniqueIdParse;
using System.Reflection;
using System.IO;
using System.Text;
using System.Linq;

namespace Formwork
{
    public partial class Form1 : Form
    {
        private List<CADUniqueIdModel> data;
        private bool fileOpened = false;        
        
        public Form1()
        {           
            InitializeComponent();           
            
        }

        private void SetDoubleBuffered(Control c, bool value)
        {
            PropertyInfo pi = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic);
            if (pi != null)
            {
                pi.SetValue(c, value, null);
            }
        }

        private void Сalculate_Click(object sender, EventArgs e)
        {
            DataGridView dataGridView = new DataGridView() { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill};
            SetDoubleBuffered(dataGridView, true);
            var data = new List<SumCADUnique>(CADUniqueIdClass.GetSumCADUniques());

            DataTable tempTable = CADUniqueIdClass.ToDataTable(data);
            tempTable.Columns[0].ColumnName = "Наименование";
            tempTable.Columns[1].ColumnName = "Кол-во";
            tempTable.Columns[2].ColumnName = "Сумма по ширине(2000)";
            tempTable.Columns[3].ColumnName = "Сумма по высоте(2000)";
            tempTable.Columns[4].ColumnName = "Сумма по ширине(1690)";
            tempTable.Columns[5].ColumnName = "Сумма по высоте(1690)";
            tempTable.Columns[6].ColumnName = "Сумма по ширине(1400)";
            tempTable.Columns[7].ColumnName = "Сумма по высоте(1400)";
            tempTable.Columns[8].ColumnName = "Сумма по ширине(1000)";
            tempTable.Columns[9].ColumnName = "Сумма по высоте(1000)";

            BindingSource bindingSource = new BindingSource { DataSource = tempTable };
            dataGridView.DataSource = bindingSource;

            CreateNewForm(dataGridView);
        }

        private void OpenCatalog_Click(object sender, EventArgs e)
        {
            MenuItem openMenuItem = new MenuItem();
            string folderName = string.Empty;
            List<string> namesFiles = new List<string>();

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;
                string[] files = Directory.GetFiles(folderName);

                foreach(string f in files)
                {
                    namesFiles.Add(Path.GetFileNameWithoutExtension(f));
                }

                try
                {
                    data = new List<CADUniqueIdModel>(CADUniqueIdClass.GetCollectionCADUnique(namesFiles));
                }
                catch (Exception ex)
                {
                    CADUniqueIdClass.WriteError("Form1/OpenFolder/data = new List<CADUniqueIdModel>(CADUniqueIdClass.GetCollectionCADUnique(namesFiles)) -> " + ex.Message);
                    MessageBox.Show("Неверные данные!");   
                }
                
                BindingSource bindingSource = new BindingSource { DataSource = data };
                CADUniqueIdDataGridView1.DataSource = bindingSource;

                if (!fileOpened)
                {                    
                    openFileDialog1.InitialDirectory = folderName;
                    openFileDialog1.FileName = null;
                    openMenuItem.PerformClick();
                }
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            string openFileName = string.Empty;
            MenuItem closeMenuItem = new MenuItem();

            if (!fileOpened)
            {
                openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
                openFileDialog1.FileName = null;
            }

            DialogResult result = openFileDialog1.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                openFileName = openFileDialog1.FileName;
                try
                {
                    
                    List<string> needNames = File.ReadAllLines(openFileName, Encoding.Default).ToList();

                    try
                    {
                        data = new List<CADUniqueIdModel>(CADUniqueIdClass.GetCollectionCADUnique(needNames));
                    }
                    catch (Exception ex)
                    {
                        CADUniqueIdClass.WriteError("Form1/OpenFile/data = new List<CADUniqueIdModel>(CADUniqueIdClass.GetCollectionCADUnique(needNames)) -> " + ex.Message);
                        MessageBox.Show("Неверные данные!");
                    }
                    
                    BindingSource bindingSource = new BindingSource { DataSource = data };
                    CADUniqueIdDataGridView1.DataSource = bindingSource;

                    fileOpened = true;

                }
                catch (Exception ex)
                {
                    CADUniqueIdClass.WriteError("Form1/OpenFile_Click -> " + ex.Message);
                    fileOpened = false;
                }
                Invalidate();

                closeMenuItem.Enabled = fileOpened;
            }
            
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }

        private void CreateNewForm(DataGridView dataGridView)
        {
            Form tempForm = new Form
            {
                Size = new Size(939, 489)
            };

            tempForm.Controls.Add(dataGridView);

            tempForm.ShowDialog();
        }

        private void OpenDB_Click(object sender, EventArgs e)
        {
            SetDoubleBuffered(CADUniqueIdDataGridView1, true);
            data = new List<CADUniqueIdModel>(CADUniqueIdClass.GetCollectionCADUnique());
            BindingSource bindingSource = new BindingSource { DataSource = data };
            CADUniqueIdDataGridView1.DataSource = bindingSource;

            CADUniqueIdDataGridView1.Columns[0].HeaderText = "Имя";
            CADUniqueIdDataGridView1.Columns[1].HeaderText = "Ширина";
            CADUniqueIdDataGridView1.Columns[2].HeaderText = "Высота";
            CADUniqueIdDataGridView1.Columns[3].HeaderText = "Глубина";
            CADUniqueIdDataGridView1.Columns[4].HeaderText = "Процент по ширине(2000)";
            CADUniqueIdDataGridView1.Columns[5].HeaderText = "Процент по длине(2000)";
            CADUniqueIdDataGridView1.Columns[6].HeaderText = "Процент по ширине(1690)";
            CADUniqueIdDataGridView1.Columns[7].HeaderText = "Процент по длине(1690)";
            CADUniqueIdDataGridView1.Columns[8].HeaderText = "Процент по ширине(1400)";
            CADUniqueIdDataGridView1.Columns[9].HeaderText = "Процент по длине(1400)";
            CADUniqueIdDataGridView1.Columns[10].HeaderText = "Процент по ширине(1000)";
            CADUniqueIdDataGridView1.Columns[11].HeaderText = "Процент по длине(1000)";

        }
    }
    
}
