using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FillTrack;
using System.Linq;
using System.Drawing.Printing;
using System.IO;
using System.Xml.Serialization;
using Excel = Microsoft.Office.Interop.Excel;
using ElematicV2.Properties;
using Office = Microsoft.Office.Core;

namespace ElematicV2
{
    public partial class Form1 : Form
    {
        private Plate plate;
        private List<Plate> plates;
        private List<List<Rope>> allRopes;
        private Controller cntr;
        private List<Track> sortTracks;
        private List<Track> finishTracks;
        private List<Track> listFinishTrack;
        private List<AligmentPlate> aligmentPlate;
        private int idTrack = 1;
        private int current_row = 0;
        private int current_col = 0;
        private int tempRow1, tempCol1, tempRow2, tempCol2;
        private List<List<Rope>> tempRopes;
        private string typeNode = null;
        private List<string> countTypeNode;
        Excel.Application excelapp;
        Excel.Workbook excelappworkbook;
        Excel.Worksheet worksheet;
        Excel.Range excelCells;
        List<string> signalName;
        List<Report> reports;

        public Form1()
        {
            InitializeComponent();
            allRopes = new List<List<Rope>>();
            plates = new List<Plate>();
            finishTracks = new List<Track>();
            listFinishTrack = new List<Track>();
            tempRopes = new List<List<Rope>>();
            plates = new List<Plate>();
            cntr = new Controller(Convert.ToInt32(textBox10.Text));            
            button1.BackColor = Color.Khaki;
            button2.BackColor = Color.Khaki;
            button3.BackColor = Color.LightCoral;
            countTypeNode = new List<string>();
            signalName = new List<string>();
            reports = new List<Report>();

        }
        private void PlaseButton_Click(object sender, EventArgs e)
        {
            
            if (plates.Count <= 0)
            {
                MessageBox.Show("Данные отсутствуют.");
            }
            else
            {
                idTrack = 1;
                dataGridView.DataSource = null;
                GetResult(countTypeNode);
            }

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (radioButton1.Checked == true)
                {
                    typeNode = "НУ 180-круг";
                    if (!countTypeNode.Contains(typeNode))
                    {
                        countTypeNode.Add(typeNode);
                    }
                    
                }
                if (radioButton2.Checked == true)
                {
                    typeNode = "НУ 160-череп";
                    if (!countTypeNode.Contains(typeNode))
                    {
                        countTypeNode.Add(typeNode);
                    }

                }
                if (radioButton3.Checked == true)
                {
                    typeNode = "НУ 220-эллипс";
                    if (!countTypeNode.Contains(typeNode))
                    {
                        countTypeNode.Add(typeNode);
                    }

                }

                allRopes = new List<List<Rope>>(tempRopes);
                tempRopes.Clear();

                for (int i = 0; i < Convert.ToInt32(textBox9.Text); i++)
                {
                    plates.Add(new Plate(textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox6.Text), typeNode, textBox7.Text, textBox8.Text, allRopes));
                }

                ShowPlates(plates, listView1);
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат данных!");
            }

        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            idTrack = 1;
            listView1.Items.Clear();
            plate = null;
            plates.Clear();
            dataGridView.DataSource = null;          

        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {    
            
            try
            {              

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {                   
                    
                    plate = new Plate();                    

                    string foldername = folderBrowserDialog1.SelectedPath;

                    foreach (string xFile in Directory.GetFiles(foldername)) {
                        plate = DeserializeObject<Plate>(xFile);
                        if (countTypeNode.Count == 0)
                        {
                            countTypeNode.Add(plate.TypeNode);
                        }
                        else
                        {
                            if (!countTypeNode.Contains(plate.TypeNode))
                            {
                                countTypeNode.Add(plate.TypeNode);
                            }
                        }
                        plates.Add(plate);
                    }

                    ShowPlates(plates, listView1);

                    

                    GetResult(countTypeNode);                    

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region function GetResult definition
        private void GetResult(List<string> counTypeNode)
        {
            cntr = new Controller(Convert.ToInt32(textBox10.Text));
            aligmentPlate = new List<AligmentPlate>();

            button1.Enabled = false;

            try
            {
                dataGridView.DataSource = null;
                sortTracks = new List<Track>();

                for(int i = 0; i < counTypeNode.Count; i++)
                {
                    sortTracks.Add(new Track(countTypeNode[i], Convert.ToInt32(textBox10.Text)));
                }

                finishTracks.Clear();
                listFinishTrack.Clear();

                cntr.GetSortTypeNodePlate(plates, sortTracks);

                PlatesHandler(sortTracks, listFinishTrack);

                aligmentPlate.Clear();

                aligmentPlate.AddRange(GetPlateData(listFinishTrack));
                
                dataGridView.DataSource = aligmentPlate;
                dataGridView.Columns[0].HeaderText = "Дорожка";
                dataGridView.Columns[1].HeaderText = "Тип узла";
                dataGridView.Columns[2].HeaderText = "Количество плит";
                dataGridView.Columns[3].HeaderText = "Расположение плит на дорожке";

                #region color change DataGridView.Rows

                for (int i = 0; i < dataGridView.RowCount;)
                {

                    foreach (Track t in listFinishTrack)
                    {
                        if (ChangeSignal(t) == true)
                        {
                            dataGridView.Rows[i].Cells[0].Style.BackColor = Color.Orange;
                            
                            dataGridView.Rows[i].Cells[1].Style.BackColor = Color.Orange;
                           
                            dataGridView.Rows[i].Cells[2].Style.BackColor = Color.Orange;
                           
                            dataGridView.Rows[i].Cells[3].Style.BackColor = Color.Orange;
                           

                            if (i == dataGridView.RowCount) break;
                            i++;

                        }
                        else
                        {
                            if (i == dataGridView.RowCount) break;
                            i++;
                        }
                    }
                 
                      
                }
                #endregion

                #region отчет о переармировании
                for(int i = 0; i < listFinishTrack.Count; i++)
                {
                    if (ChangeSignal(listFinishTrack[i]) == true)
                    {
                        for (int j = 0; j < listFinishTrack[i].Plates.Count; j++)
                        {
                            if (listFinishTrack[i].Plates[0].AllRopes != listFinishTrack[i].Plates[j].AllRopes)
                            {
                                for (int k = 0; k < listFinishTrack[i].Plates[0].AllRopes.Count; k++)
                                {
                                    if (listFinishTrack[i].Plates[0].AllRopes[k] != listFinishTrack[i].Plates[j].AllRopes[k])
                                    {
                                        for (int g = 0; g < listFinishTrack[i].Plates[0].AllRopes[k].Count; g++)
                                        {
                                            if (listFinishTrack[i].Plates[0].AllRopes[k][g].Diameter > listFinishTrack[i].Plates[j].AllRopes[k][g].Diameter && listFinishTrack[i].Plates[0].AllRopes[k][g].Location == listFinishTrack[i].Plates[j].AllRopes[k][g].Location)
                                            {
                                                reports.Add(new Report { NamePlate = listFinishTrack[i].Plates[j].Name, NameTrack = "Дорожке " + (i + 1), OriginDiameterRope = listFinishTrack[i].Plates[0].AllRopes[k][g].Diameter, ThisDiameterRope = listFinishTrack[i].Plates[j].AllRopes[k][g].Diameter });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            button1.Enabled = true;
        }
        #endregion

        #region function DeletePlate definition
        private void DeletePlate(List<Plate> bestPlates, List<Plate> plates)
        {
            if (bestPlates.Count != 0)
            {
                if (bestPlates.SequenceEqual(plates))
                {
                    plates.Clear();
                }
                else
                {
                    int i = 0, j = 0;

                    while (i < bestPlates.Count)
                    {

                        if (bestPlates.Contains(plates[j]))
                        {
                            plates.Remove(plates[j]);
                            i++;

                        }
                        else
                        {
                            j++;
                            if (j > plates.Count - 1) break;
                        }

                    }
                }             
                

            }
        }
        #endregion        

        #region function ChangeSignal definition
        private bool ChangeSignal(Track track) 
        {
            foreach (Plate p in track.Plates)
            {
                for (int i = 0; i < p.AllRopes.Count; i++)
                {
                    for (int j = 0; j < p.AllRopes[i].Count; j++)
                    {
                        try
                        {
                            if (track.Plates[0].AllRopes[i][j].Diameter != p.AllRopes[i][j].Diameter || track.Plates[0].AllRopes[i].Count != p.AllRopes[i].Count)
                            {
                                
                                return true;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            return false;

        }
        #endregion

        #region function PlatesHandlerAsync definition
        private void  PlatesHandler(List<Track> sortTracks, List<Track> finishTracks)
        {
            Track tempTrack;

            foreach (Track t in sortTracks)
            {
                int countPlates = t.Plates.Count;
                tempTrack = new Track(t.TypeNode, Convert.ToInt32(textBox10.Text));

                while (countPlates > 0)
                {

                    tempTrack.Plates = new List<Plate>(cntr.CheckSet(t.Plates));
                    finishTracks.Add(new Track(tempTrack.LengthTrack, tempTrack.TypeNode, tempTrack.Plates));
                    DeletePlate(tempTrack.Plates, t.Plates);

                    countPlates = t.Plates.Count;

                }
            }
                cntr.bestPlate = null;
        }
        #endregion

        #region function GetPlateData definition
        private List<AligmentPlate> GetPlateData(List<Track> tracks)
        {
            List<AligmentPlate> result = new List<AligmentPlate>();
            string listPlate = "";
           
            for(int i = 0; i < tracks.Count; i++)
            {                              
                for (int j = 0; j < tracks[i].Plates.Count; j++)
                {
                   listPlate += tracks[i].Plates[j].Name + " ";
                }

                if (listPlate == "") break;
                if (ChangeSignal(tracks[i]) == true)
                {
                    result.Add(new AligmentPlate("Дорожка " + idTrack + "*", tracks[i].TypeNode, tracks[i].Plates.Count, listPlate));
                }
                else
                {
                    result.Add(new AligmentPlate("Дорожка " + idTrack, tracks[i].TypeNode, tracks[i].Plates.Count, listPlate));
                }
                idTrack++;
                listPlate = "";              

            }
            return result;
        }
        #endregion

        #region function ShowPlates definition
        private void ShowPlates(List<Plate> plates, ListView plateListView)
        {
            plateListView.Items.Clear();           

            for (int i = 0; i < plates.Count; i++)
            {                
                plateListView.Items.Add(new ListViewItem(new string[] { plates[i].Name, plates[i].Length.ToString(), plates[i].Width.ToString(), plates[i].TypeNode.ToString(), plates[i].AllRopes.Count.ToString()/*, Convert.ToString((plates[i].AllRopes[0].Diameter + "/" + plates[i].AllRopes[plates[i].AllRopes.Count-1].Diameter))*/ }));
               
            }

        }
        #endregion
        
        private void printPreview_PrintClick(object sender, EventArgs e)
        {
            try
            {
                printDialog1.Document = printDocument1;
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ToString());
            }
        }
        

        
        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AddRopes_Click(object sender, EventArgs e)
        {
            
            try
            {
                // низ
                for(int i = 0; i < Convert.ToInt32(textBox4.Text); i++)
                {
                    tempRopes.Add(new List<Rope> { new Rope { Diameter = Convert.ToInt32(textBox5.Text), Location = "низ" } });
                }
                // верх
                for (int i = 0; i < Convert.ToInt32(textBox12.Text); i++)
                {
                    tempRopes.Add(new List<Rope> { new Rope { Diameter = Convert.ToInt32(textBox13.Text), Location = "верх" } });
                }



                label8.Text = "Количество канатов: " + (Convert.ToInt32(textBox4.Text) + Convert.ToInt32(textBox12.Text));
                
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверный формат данных!");
            }
        }
        private void SerializeButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked == true)
                {
                    typeNode = "НУ 180-круг";
                    if (!countTypeNode.Contains(typeNode))
                    {
                        countTypeNode.Add(typeNode);
                    }

                }
                if (radioButton2.Checked == true)
                {
                    typeNode = "НУ 160-череп";
                    if (!countTypeNode.Contains(typeNode))
                    {
                        countTypeNode.Add(typeNode);
                    }

                }
                if (radioButton3.Checked == true)
                {
                    typeNode = "НУ 220-эллипс";
                    if (!countTypeNode.Contains(typeNode))
                    {
                        countTypeNode.Add(typeNode);
                    }

                }

                Plate plateToXml = new Plate(textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox6.Text), typeNode, textBox7.Text, textBox8.Text, allRopes);

                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;

                int countCopy = Convert.ToInt32(textBox11.Text);
                string fileName = saveFileDialog1.FileName;

                for (int i = 0; i < countCopy; i++)
                {                                        
                    SerializeObject(plateToXml, fileName + "_" + (i+1) + ".xml");                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не все поля заполнены!");
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }      

       
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
          
        }

        private void Print_Click(object sender, EventArgs e)
        {
            tempRow1 = 0; tempRow2 = 0; tempCol1 = 0;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1;
            ToolStripButton b = new ToolStripButton();
            b.Image = ((ToolStrip)(printPreviewDialog1.Controls[1])).ImageList.Images[0];
            b.DisplayStyle = ToolStripItemDisplayStyle.Image;
            b.Click += printPreview_PrintClick;
            ((ToolStrip)(printPreviewDialog1.Controls[1])).Items.RemoveAt(0);
            ((ToolStrip)(printPreviewDialog1.Controls[1])).Items.Insert(0, b);
            printPreviewDialog1.ShowDialog();            

        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {

                Graphics g = e.Graphics;
                int x = 10;
                int y = 100;
                int cell_height = 0;
                current_col = tempCol1;
                tempCol2 = tempCol1;
                current_row = tempRow1;
                int colCount = dataGridView.ColumnCount;
                int rowCount = dataGridView.RowCount;
                int width = dataGridView[current_col, current_row].Size.Width;
                int height = dataGridView[current_col, current_row].Size.Height;
                Rectangle cell_border;

                StringFormat str = new StringFormat();
                str.Alignment = StringAlignment.Near;
                str.LineAlignment = StringAlignment.Center;
                str.Trimming = StringTrimming.EllipsisCharacter;
                cell_height = dataGridView[current_col, current_row].Size.Height + 10;

                while (current_col < colCount)
                {
                    if (x > e.MarginBounds.Width)
                    {
                        e.HasMorePages = true;
                        break;
                    }
                    g.FillRectangle(Brushes.LightGray,
                                new Rectangle(x, y, dataGridView.Columns[current_col].Width,
                                    cell_height));
                    g.DrawRectangle(Pens.Black,
                        x, y, dataGridView.Columns[current_col].Width,
                        cell_height);
                    g.DrawString(dataGridView.Columns[current_col].HeaderText,
                        new Font("Tahoma", 9, FontStyle.Bold, GraphicsUnit.Point),
                        Brushes.Black,
                        new RectangleF(x, y, dataGridView.Columns[current_col].Width,
                            cell_height), str);
                    x += dataGridView[current_col, current_row].Size.Width;
                    current_col++;
                }

                y += cell_height; x = 0; current_col = tempCol1;
                while (current_row < rowCount)
                {
                    if (y > e.MarginBounds.Height + 0)
                    {
                        tempRow1 = tempRow2;
                        if (x < e.MarginBounds.Width)
                            tempCol1 = 0;
                        else
                            tempRow2 = current_row;
                        e.HasMorePages = true;
                        y = 100;
                        return;
                    }
                    x = 10;
                    while (current_col < colCount)
                    {
                        if (x > e.MarginBounds.Width)
                        {
                            tempCol1 = current_col;
                            e.HasMorePages = true;
                            break;
                        }
                        cell_height = dataGridView[current_col, current_row].Size.Height + 10;
                        cell_border = new Rectangle(x, y, width, height);
                        g.DrawRectangle(Pens.Black, x, y,
                            dataGridView.Columns[current_col].Width,
                            cell_height);                        
                        g.DrawString(dataGridView.Rows[current_row].Cells[current_col].Value.ToString(),
                            dataGridView.Font, Brushes.Black,
                            new RectangleF(x, y,
                                dataGridView.Columns[current_col].Width,
                                cell_height),
                            str);
                        x += dataGridView[current_col, current_row].Size.Width;
                        current_col++;
                    }
                    current_col = tempCol2;
                    current_row++;
                    y += cell_height;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Нет данных для вывода!");
            }
        }

        private void ToExcel_Click(object sender, EventArgs e)
        {

            excelapp = new Excel.Application();


            excelappworkbook = excelapp.Workbooks.Add(Template: GetUri());

            FillXlTable();

            excelapp.Visible = true;
            excelapp.UserControl = true;
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
        private void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(fs, serializableObject);
            }
        }

        private T DeserializeObject<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) { return default(T); }
            if (!File.Exists(fileName)) { return default(T); }
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(fs);
            }
        }

        private void DrowXlTable(int x, int y, string s)
        {
            Excel.Range tempExcelCells;
            Excel.Range cell;

            //дорожка
            excelCells = worksheet.get_Range("A" + x, "A" + x);

            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 37;
                excelCells.EntireRow.Font.Bold = true;
            }
            tempExcelCells = worksheet.get_Range("A" + Convert.ToString(x - 1), "A" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            //дренаж
            excelCells = worksheet.get_Range("B" + x, "B" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 6;
            }
            tempExcelCells = worksheet.get_Range("B" + Convert.ToString(x - 1), "B" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            //план
            excelCells = worksheet.get_Range("C" + x, "C" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            cell = worksheet.get_Range("C" + x, "C" + x);
            cell.EntireColumn.Font.Bold = true;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 15;
            }
            else
            {
                excelCells.Interior.ColorIndex = 35;
            }
            tempExcelCells = worksheet.get_Range("C" + Convert.ToString(x - 1), "C" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            //заказчик
            excelCells = worksheet.get_Range("D" + x, "D" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 15;
            }
            tempExcelCells = worksheet.get_Range("D" + Convert.ToString(x - 1), "D" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            //кол-во канатов
            excelCells = worksheet.get_Range("E" + x, "E" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 15;
            }
            tempExcelCells = worksheet.get_Range("E" + Convert.ToString(x - 1), "E" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            excelCells = worksheet.get_Range("F" + x, "F" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 15;
            }
            tempExcelCells = worksheet.get_Range("F" + Convert.ToString(x - 1), "F" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            excelCells = worksheet.get_Range("G" + x, "G" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 15;
            }
            tempExcelCells = worksheet.get_Range("G" + Convert.ToString(x - 1), "G" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            //длина
            excelCells = worksheet.get_Range("H" + x, "H" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 15;
            }
            tempExcelCells = worksheet.get_Range("H" + Convert.ToString(x - 1), "H" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            //ширина
            excelCells = worksheet.get_Range("I" + x, "I" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 15;
            }
            tempExcelCells = worksheet.get_Range("I" + Convert.ToString(x - 1), "I" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            //стержень петли
            excelCells = worksheet.get_Range("J" + x, "J" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 15;
            }
            tempExcelCells = worksheet.get_Range("J" + Convert.ToString(x - 1), "J" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            //высота
            excelCells = worksheet.get_Range("K" + x, "K" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;
            if (s == "header")
            {
                excelCells.Interior.ColorIndex = 15;
            }
            tempExcelCells = worksheet.get_Range("K" + Convert.ToString(x - 1), "K" + x);
            excelCells.ColumnWidth = tempExcelCells.ColumnWidth;
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            //раскладка канатов
            Excel.Range lCells = worksheet.get_Range("L" + x, "L" + x);
            excelCells.HorizontalAlignment = Excel.Constants.xlCenter;
            excelCells.VerticalAlignment = Excel.Constants.xlCenter;

            tempExcelCells = worksheet.get_Range("L" + Convert.ToString(x - 1), "L" + x);
            lCells.ColumnWidth = tempExcelCells.ColumnWidth;
            lCells.Borders.Weight = Excel.XlBorderWeight.xlThin;


        }

        private void FillXlTable()
        {            
            worksheet = (Excel.Worksheet)excelappworkbook.Sheets[1];
            int x = 5, y = 1;
            List<Plate> namesPlate = new List<Plate>();
            List<Rope> ropesUp = new List<Rope>();
            List<Rope> ropesDown = new List<Rope>();

            for (int j = 0; j < listFinishTrack.Count; j++)
            {
                string filePic = "C:\\Users\\Ильгар\\Pictures\\layout_180.PNG";
                DrowXlTable(x, y, "header");
                excelCells = worksheet.get_Range("A" + x, "L" + (x + 3)).Rows;
                excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;
                worksheet.Cells[x, y] = "Дорожка " + Convert.ToString(j + 1);

                if (ChangeSignal(listFinishTrack[j]) == true)
                {                  
                    excelCells = worksheet.get_Range("A" + x, "A" + x);
                    excelCells.Interior.ColorIndex = 6;                   
                    
                }

                excelCells = worksheet.get_Range("L" + x, "L" + (x + 3)).Columns;
                excelCells.Merge(Type.Missing);

                x++;

                DrowXlTable(x, y, "margin");
                worksheet.Cells[x, y] = listFinishTrack[j].Plates[0].Name;
                namesPlate = listFinishTrack[j].Plates.FindAll(k => k.Name == listFinishTrack[j].Plates[0].Name);
                worksheet.Cells[x, y + 2] = namesPlate.Count;

                for (int g = 0; g < namesPlate[0].AllRopes.Count; g++)
                {
                    for (int l = 0; l < namesPlate[0].AllRopes[g].Count; l++)
                    {
                        if(namesPlate[0].AllRopes[g][l].Location == "низ")
                        {
                            ropesDown.Add(namesPlate[0].AllRopes[g][l]);
                        }
                        else
                        {
                            ropesUp.Add(namesPlate[0].AllRopes[g][l]);
                        }
                    }
                }               

                switch (ropesUp[0].Diameter)
                {
                    case 12: worksheet.Cells[x, y + 4] = ropesUp.Count; break;
                    case 9: worksheet.Cells[x, y + 5] = ropesUp.Count; break;
                    case 5: worksheet.Cells[x, y + 6] = ropesUp.Count; break;
                }
                switch (ropesDown[0].Diameter)
                {
                    case 12: worksheet.Cells[x, y + 4] = ropesDown.Count; break;
                    case 9: worksheet.Cells[x, y + 5] = ropesDown.Count; break;
                    case 5: worksheet.Cells[x, y + 6] = ropesDown.Count; break;
                }
                ropesDown.Clear();
                ropesUp.Clear();
                worksheet.Cells[x, y + 7] = namesPlate[0].Length;
                worksheet.Cells[x, y + 8] = namesPlate[0].Width;
                worksheet.Cells[x, y + 9] = namesPlate[0].Loop;
                worksheet.Cells[x, y + 10] = namesPlate[0].Height;

                if(namesPlate[0].Height == 180)
                {                   

                    excelCells = worksheet.get_Range("L" + x, "L" + (x+3));
                    float il, it, iw, ih;
                    float zExcelPixel = 0.746835443f;
                    Image im = Resources.layout_180;
                    var resource = new { Name = "layout_180.PNG", Buff = Resources.layout_180 };
                    il = (float)(double)excelCells.Left+7;
                    it = (float)(double)excelCells.Top;
                    iw = zExcelPixel * im.Width;
                    ih = zExcelPixel * im.Height;
                    
                    
                    worksheet.Shapes.AddPicture(filePic, Office.MsoTriState.msoFalse, Office.MsoTriState.msoCTrue, il, it, iw, ih);

                }

                if (listFinishTrack[j].Plates.Count > namesPlate.Count)
                {
                    for (int i = 0; i < listFinishTrack[j].Plates.Count - 1; i++)
                    {
                        if (listFinishTrack[j].Plates[i].Name != listFinishTrack[j].Plates[i + 1].Name)
                        {
                            x++;
                            DrowXlTable(x, y, "margin");
                            worksheet.Cells[x, y] = listFinishTrack[j].Plates[i + 1].Name;
                            namesPlate = listFinishTrack[j].Plates.FindAll(k => k.Name == listFinishTrack[j].Plates[i + 1].Name);
                            worksheet.Cells[x, y + 2] = namesPlate.Count;
                            
                            for (int g = 0; g < namesPlate[0].AllRopes.Count; g++)
                            {
                                for (int l = 0; l < namesPlate[0].AllRopes[g].Count; l++)
                                {
                                    if (namesPlate[0].AllRopes[g][l].Location == "низ")
                                    {
                                        ropesDown.Add(namesPlate[0].AllRopes[g][l]);
                                    }
                                    else
                                    {
                                        ropesUp.Add(namesPlate[0].AllRopes[g][l]);
                                    }
                                }
                            }

                            switch (ropesUp[0].Diameter)
                            {
                                case 12: worksheet.Cells[x, y + 4] = ropesUp.Count; break;
                                case 9: worksheet.Cells[x, y + 5] = ropesUp.Count; break;
                                case 5: worksheet.Cells[x, y + 6] = ropesUp.Count; break;
                            }
                            switch (ropesDown[0].Diameter)
                            {
                                case 12: worksheet.Cells[x, y + 4] = ropesDown.Count; break;
                                case 9: worksheet.Cells[x, y + 5] = ropesDown.Count; break;
                                case 5: worksheet.Cells[x, y + 6] = ropesDown.Count; break;
                            }
                            ropesDown.Clear();
                            ropesUp.Clear();

                            worksheet.Cells[x, y + 7] = namesPlate[0].Length;
                            worksheet.Cells[x, y + 8] = namesPlate[0].Width;
                            worksheet.Cells[x, y + 9] = namesPlate[0].Loop;
                            worksheet.Cells[x, y + 10] = namesPlate[0].Height;
                            //break;
                        }
                    }
                    excelCells = worksheet.get_Range("A" + x, "L" + (x + 2));
                    excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    x = x + 2;

                }
                else
                {
                    excelCells = worksheet.get_Range("A" + x, "L" + (x + 3));
                    excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    x = x + 3;

                }
            }
            x = x + 2;
            excelCells = worksheet.get_Range("A" + x, "L" + x).Rows;
            excelCells.Merge(Type.Missing);
            excelCells.Borders.Weight = Excel.XlBorderWeight.xlThin;
            excelCells.Value = reports[0].NamePlate + " на " + reports[0].NameTrack + " диаметр изменен с " + reports[0].ThisDiameterRope + " на " + reports[0].OriginDiameterRope;

            for(int i = 0; i < reports.Count-1; i++)
            {
                if(reports[i] != reports[i + 1])
                {
                    x++;
                    excelCells.Value = reports[i+1].NamePlate + " на " + reports[i+1].NameTrack + " диаметр изменен с " + reports[i+1].ThisDiameterRope + " на " + reports[i+1].OriginDiameterRope;
                }
            }
        }

        private string GetUri()
        {
            var resource = new { Name = "template.xlsx", Buff = Resources.template };

            var tempDirectory = Path.GetDirectoryName(Path.GetTempFileName());

            var path = string.Format("{0}\\{1}", tempDirectory, resource.Name);

            if (!File.Exists(path) || File.ReadAllBytes(path).Length.Equals(0))
            {
                var stream = new MemoryStream(resource.Buff);

                using (var file = new FileStream(path, FileMode.Create))
                {
                    var buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, bytesRead);
                    }
                }
            }

            return path;
        }

        //private string GetUriPicture()
        //{
        //    var resource = new { Name = "layout_180.PNG", Buff = Resources.layout_180 };
            
        //    var tempDirectory = Path.GetDirectoryName(Path.GetTempFileName());

        //    var path = string.Format("{0}\\{1}", tempDirectory, resource.Name);

        //    if (!File.Exists(path) || File.ReadAllBytes(path).Length.Equals(0))
        //    {
        //        //var stream = new MemoryStream(resource.Buff);

        //        using (var file = new FileStream(path, FileMode.CreateNew))
        //        {
        //            var buffer = new byte[4096];
        //            //int bytesRead;

        //            //while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        //            //{
        //            //    file.Write(buffer, 0, bytesRead);
        //            //}
        //        }
        //    }

        //    return path;
        //}
    }
}
