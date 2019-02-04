using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;


namespace TestCAD
{
    public partial class Form1 : Form
    {
        DataTable dt;
        DataRow[] needRow;
        string path = Directory.GetCurrentDirectory();
        MeshDrawer drawer;
        Bitmap bmp;
        DataGridForm dgForm;

        public Form1()
        {
            InitializeComponent();
            dt = new DataTable();            
            dt = DeserializeObject(path + "\\save.bin");
            dataGridView1.DataSource = dt;
            dataGridView1.Visible = false;
            drawer = new MeshDrawer();
            drawer.dimlen = 500;
            drawer.dimlen2 = 70;
            drawer.dimspace = 50;
            drawer.spacebetween = 200;
            drawer.fontsize = 7;
            drawer.pencilsize1 = 3;
            drawer.pencilsize2 = 1;
            button3.Enabled = false;
           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
        }

        //public void OpenFile(Excel.Application xlApp, Excel.Workbook xlWorkbook)
        //{
        //    xlApp = new Excel.Application();
        //    Excel.Range excelCells1;
        //    Excel.Range excelCells2;

        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        string fName = openFileDialog1.FileName;
        //        xlWorkbook = xlApp.Workbooks.Add(fName);
        //        Excel.Worksheet worksheet = (Excel.Worksheet)xlWorkbook.Sheets[1];

        //        excelCells1 = worksheet.get_Range("A1", "C1");
        //        excelCells2 = worksheet.get_Range("A3", "C3");
        //        excelCells2.Value = excelCells1.Value;

        //        excelCells1 = worksheet.get_Range("A1", "T2");
        //        excelCells1.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);

        //        var needCell = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
        //        excelCells1 = worksheet.get_Range("T1", "T" + needCell.Row);
        //        excelCells1.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);

        //        needCell = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
        //        excelCells1 = worksheet.get_Range("C1", "C" + needCell.Row);
        //        excelCells1.Delete(Excel.XlDeleteShiftDirection.xlShiftToLeft);

        //        try
        //        {
        //            List<string> strCells = new List<string>();
        //            var lastCell = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

        //            for (int i = 0; i < lastCell.Row; i++)
        //            {
        //                for (int j = 0; j < lastCell.Column; j++)
        //                {
        //                    strCells.Add(worksheet.Cells[i + 1, j + 1].Text.ToString());
        //                }
        //            }                    

        //            for (int i = 0; i < lastCell.Column; i++)
        //            {
        //                if (dt.Columns.Contains(strCells[i]))
        //                {
        //                    dt.Columns.Add(strCells[i] + " ");
        //                }
        //                else
        //                {
        //                    dt.Columns.Add(strCells[i]);
        //                }
        //            }

        //            int first = lastCell.Column;

        //            for (int i = lastCell.Column; i < strCells.Count; i += lastCell.Column)
        //            {
        //                string[] arrCells = strCells.GetRange(first, lastCell.Column).ToArray();
        //                first += lastCell.Column;
        //                dt.Rows.Add(arrCells);
        //            }

        //            dataGridView1.DataSource = dt;
        //            ColorSignal();                    

        //            xlApp.Visible = false;
        //            xlApp.UserControl = true;

        //            xlWorkbook.Close(false, Type.Missing, Type.Missing);
        //            xlApp.Quit();
        //            GC.Collect();
        //        }
        //        catch (Exception)
        //        {
        //            xlWorkbook.Close(false, Type.Missing, Type.Missing);
        //            xlApp.Quit();
        //            GC.Collect();
        //        }                
        //    }
        //}

        public void OpenCSV()
        {          

            //openFileDialog1.Filter = "Cursor Files|*.csv";

            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);               
                 
            //}
        }

       

        private void Search_Click(object sender, EventArgs e)
        {

            needRow = new DataRow[1];
            needRow = dt.Select(string.Format("[{0}] LIKE '%{1}%'", dt.Columns[1].ColumnName, textBox1.Text));
            
            if (needRow.Length == 0)
            {
                MessageBox.Show("Не найдено!");
                return;
            }

            List<string> listCells = new List<string>();
            string s = null;

            int i = 0;
            foreach (DataRow r in needRow)
            {
                foreach (var cell in r.ItemArray)
                {
                    if (i == 18) break;
                    s += " " + cell;
                    i++;
                }
            }
            listCells.Add(s);
            label1.Text = s;
                
        }


        private void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs,serializableObject);
               
            }
        }

        private DataTable DeserializeObject(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) { return default(DataTable); }
            if (!File.Exists(fileName)) { return default(DataTable); }
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter binForm = new BinaryFormatter();
                fs.Seek(0, SeekOrigin.Begin);
                DataTable obj = (DataTable)binForm.Deserialize(fs);
                return obj;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dt != null)
            {
                if(dt.Columns[0].ColumnName == "N п/п")
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][0] = i + 1;
                }
            }
                dgForm = new DataGridForm(dt);
                dgForm.ShowDialog();
            
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {            
            if (needRow != null)
            {
                if (needRow.Length != 0)
                {
                    DoDraw(needRow);
                }
            }
        }

        private void DoDraw(DataRow[] dataRow)
        {         

            char[] seps = { '/', '\\', ' ' };

            try
            {
                drawer.scale = ScaleSize.Value;
                drawer.sizes.Name = (string)dataRow[0][1];
                drawer.sizes.L = Convert.ToInt32(dataRow[0][2]);
                drawer.sizes.L1 = Convert.ToInt32(dataRow[0][3]);
                drawer.sizes.B = Convert.ToInt32(dataRow[0][4]);
                drawer.sizes.H = Convert.ToInt32(dataRow[0][5]);
                drawer.sizes.l0 = Convert.ToInt32(dataRow[0][6]);
                drawer.sizes.n = Convert.ToInt32(dataRow[0][7]);
                drawer.sizes.a = Convert.ToInt32(dataRow[0][8]);
                drawer.sizes.a1 = Convert.ToInt32(dataRow[0][9]);

                string s1 = (string)dataRow[0][10];
                string[] sb = s1.Split(seps);
                string s2 = (string)dataRow[0][11];
                string[] sb1 = s2.Split(seps);

                int b_1, b_2, b1_1, b1_2;

                if (drawer.sizes.L > drawer.sizes.L1)
                {
                    int.TryParse(sb[0], out b_1);
                    int.TryParse((sb.Length > 1) ? sb[1] : sb[0], out b_2);
                    int.TryParse(sb1[0], out b1_1);
                    int.TryParse((sb1.Length > 1) ? sb1[1] : sb1[0], out b1_2);

                    //drawer.sizes.b1 = b1_1;
                    //drawer.sizes.b = b_1;
                    //drawer.sizes.b_1 = b_2;
                    //drawer.sizes.b1_1 = b1_2;

                    if (b_1 > b_2) { drawer.sizes.b = b_1; drawer.sizes.b_1 = b_2; } else { drawer.sizes.b = b_2; drawer.sizes.b_1 = b_1; }
                    if (b1_1 > b1_2) { drawer.sizes.b1 = b1_1; drawer.sizes.b1_1 = b1_2; } else { drawer.sizes.b1 = b1_2; drawer.sizes.b1_1 = b1_1; }

                }
                else
                {
                    int.TryParse(sb[0], out b_2);
                    int.TryParse((sb.Length > 1) ? sb[1] : sb[0], out b_1);
                    int.TryParse(sb1[0], out b1_2);
                    int.TryParse((sb1.Length > 1) ? sb1[1] : sb1[0], out b1_1);

                    //drawer.sizes.b1 = b1_2;
                    //drawer.sizes.b = b_2;
                    //drawer.sizes.b_1 = b_1;
                    //drawer.sizes.b1_1 = b1_1;

                    if (b_1 > b_2) { drawer.sizes.b = b_2; drawer.sizes.b_1 = b_1; } else { drawer.sizes.b = b_1; drawer.sizes.b_1 = b_2; }
                    if (b1_1 > b1_2) { drawer.sizes.b1 = b1_2; drawer.sizes.b1_1 = b1_1; } else { drawer.sizes.b1 = b1_1; drawer.sizes.b1_1 = b1_2; }
                }
                

                drawer.sizes.d1 = (string)dataRow[0][12];
                drawer.sizes.d2 = (string)dataRow[0][13];
                drawer.sizes.d3 = (string)dataRow[0][14];

                drawer.Canvas_Paint(canvas.Width, canvas.Height, canvas.CreateGraphics());
            }
            catch (Exception)
            {
                MessageBox.Show("Недостаточно данных для чертежа!");
            }
        }

        private void Redraw(object sender, EventArgs e)
        {            
            canvas.Refresh();
        }

        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            canvas.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button3.Enabled = true;            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            drawer.pencilsize1 = 7;
            drawer.pencilsize2 = 4;
            drawer.scale = 1;

            int x = (drawer.sizes.L > drawer.sizes.L1 ? drawer.sizes.L : drawer.sizes.L1) + drawer.spacebetween + drawer.dimlen / 3 * 2 + drawer.dimlen2 * 4 + drawer.dimlen * 2;

            int shift = TextRenderer.MeasureText(drawer.sizes.d1, drawer.drawFont).Width + TextRenderer.MeasureText(drawer.sizes.d2, drawer.drawFont).Width;

            int y = drawer.sizes.H + drawer.dimlen2 * 4 + drawer.dimlen * 3 + drawer.dimspace * 2 + shift * 2;


            bmp = new Bitmap(x, y);

            drawer.Canvas_Paint(x, y, Graphics.FromImage(bmp));
            bmp.Save(saveFileDialog1.FileName + ".bmp");

            drawer.pencilsize1 = 3;
            drawer.pencilsize2 = 1;
        }
        //#region открыть csv
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    string[] str = { ";", "Эскиз", "Размеры, мм.", "Диаметр и класс арматуры", "Масса, кг.", "Общая", "Изделия", "\r", "\n", "\t" };
        //    List<string> col = new List<string>();
        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {                
        //        using (StreamReader rd = new StreamReader(openFileDialog1.FileName, Encoding.GetEncoding(1251)))
        //        {
        //            str = rd.ReadToEnd().Split(str, StringSplitOptions.RemoveEmptyEntries);
        //        }
        //        for (int i = 0; i < str.Length; i++)
        //        {
        //            col.Add(str[i]);
        //        }

        //        col.RemoveRange(2,34);

        //        for(int i = 2; i < col.Count; i+= 16)
        //        {
        //            col.RemoveAt(i);
        //        }
        //        for (int i = 18; i < col.Count; i += 16)
        //        {
        //            col.RemoveAt(i);
        //        }

        //        FilePaste(col);
        //    }
        //}
        //#endregion
        //public void FilePaste(List<string> list)
        //{

        //    if (dt == null) dt = new DataTable();          
           
        //    char[] rowSplitter = { '\r', '\n' };

        //    try
        //    {   

        //        string[] words = list.GetRange(0, 18).ToArray();

        //        if (dt.Columns.Count == 0)
        //        {
        //            foreach (string word in words)
        //            {
        //                if (dt.Columns.Contains(word))
        //                {
        //                    dt.Columns.Add(word + " ");
        //                }
        //                else
        //                {
        //                    dt.Columns.Add(word);
        //                }
        //            }                        
        //        }
        //        for (int i = 18; i <= list.Count; i++)
        //        {
        //            string[] rows = list.GetRange(i,17).ToArray();
        //            dt.Rows.Add(rows);
        //        }                 
    
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }           

        //}
    }
}
