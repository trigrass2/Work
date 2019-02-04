using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCAD
{
    public partial class DataGridForm : Form
    {
        DataTable table = new DataTable();
        string path = Directory.GetCurrentDirectory();

        public DataGridForm(DataTable dt)
        {
            InitializeComponent();
            if (dt == null) dt = table;
            table = dt;
            dataGridView1.DataSource = table;
            dt = table;
           
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();            
        }

        private void DataGridForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            ColorSignal();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // table = (DataTable)dataGridView1.DataSource;            
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;            
        }

        public void ColorSignal()
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.RowCount-1; j++)
                {                    
                    if (dataGridView1[i, j].Value.ToString() == "")
                    {
                        
                        dataGridView1[i, j].Style.BackColor = Color.Coral;
                        
                    }
                }
            }
        }

        public void SaveChange()
        {            
            table = (DataTable)dataGridView1.DataSource;
            SerializeObject(table, "save.bin");
        }

        public void CopyPaste()
        {
            bool IsFirstRowAsColumnNames = true;

            if (table == null) table = new DataTable();

            if (Clipboard.ContainsData(DataFormats.Text) == true)
            {
                IDataObject dataInClipboard = Clipboard.GetDataObject();

                string stringInClipboard = (string)dataInClipboard.GetData(DataFormats.Text);

                char[] rowSplitter = { '\r', '\n' };

                string[] rowsInClipboard = stringInClipboard.Split(rowSplitter, StringSplitOptions.RemoveEmptyEntries);

                try
                {

                    if (IsFirstRowAsColumnNames)
                    {
                        string[] words = rowsInClipboard[0].Split('\t');

                        if (table.Columns.Count == 0)
                        {
                            foreach (string word in words)
                            {
                                if (table.Columns.Contains(word))
                                {
                                    table.Columns.Add(word + " ");
                                }
                                else
                                {
                                    table.Columns.Add(word);
                                }
                            }

                            for (int i = 2; i <= rowsInClipboard.Length; i++)
                            {
                                string[] rows = rowsInClipboard[i - 1].Split('\t');

                                table.Rows.Add(rows);
                                
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= rowsInClipboard.Length; i++)
                            {
                                string[] rows = rowsInClipboard[i - 1].Split('\t');
                                table.Rows.Add(rows);
                            }
                        }
                        
                    }
                    else
                    {
                        for (int colc = 1; colc <= rowsInClipboard[0].Split('\t').Length; colc++)
                        {
                            table.Columns.Add("Столбец " + colc);
                        }

                        for (int i = 1; i <= rowsInClipboard.Length; i++)
                        {
                            string[] rows = rowsInClipboard[i - 1].Split('\t');
                            table.Rows.Add(rows);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Нет данных для вставки!");
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CopyPaste();
        }

        private void DataGridForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveChange();
        }

        private void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, serializableObject);

            }
        }
    }
}
