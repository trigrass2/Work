using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;

namespace UploadFiles
{
    public partial class Form1 : Form
    {
        public List<string> messeges = new List<string>();
        string[] findFiles;
        string ssss = "ddddd<Name>1В 171.273.18-1.uni</Name>  < Weight > 2079 </ Weight >  < Thickness > 180 </ Thickness >  < Height > 1710 </ Height >" +
                      "  < Width > 2730 </ Width >  < Type > 53 </ Type >  < Commiss > 001.01.01</Commiss>  <HasConsole>false</HasConsole> fffffffffffffffffffffffffffffffffffffff";

        
        public Form1()
        {
            InitializeComponent();
            
            saveFileDialog1.Filter = "Text files(*.csv)|*.csv";
            
        }

        string UploadFiles(MyFile sendFile)
        {
            IRestResponse response = null;
            try
            {
                var client = new RestClient("http://web02dsk2.dsk2.picompany.ru/api/Upload/one");
                var request = new RestRequest(Method.POST);
                
                request.RequestFormat = DataFormat.Json;

                request.AddFile("file", sendFile.FileBytes, sendFile.FileName);

                response = client.Execute(request);    
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "(UploadFiles)");
            }

            return response.Content;
        }
        public void OrdersAssemble()
        {
            List<string> ls = new List<string>();
            string r = string.Empty;

            try
            {
                if (findFiles != null && findFiles.Length != 0)
                {
                    foreach (string s in findFiles)
                    {
                        r = UploadFiles(new MyFile(File.ReadAllBytes(s), Path.GetFileName(s)));
                        if (r == null || r == "") continue;
                        var res = Cut(r);
                        if (res != null)
                        {
                            ls.Add(res);
                            listView1.Items.Add(res);
                        }
                    }
                }
                else MessageBox.Show("Нет файлов :(");

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;
                    File.WriteAllText(filename, string.Join(";\n", ls), Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "(assemble: )" + r);
            }    
            
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {                
                findFiles = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OrdersAssemble();
        }

        private string Cut(string str)
        {
            try
            {               
                var rgx = new Regex("(?<=<Name>)(.*)(?=</Name>)");
                var artmatch = rgx.Match(str);
                string Name = artmatch.Groups[0].Value; //Groups[0], а не Groups[1]

                rgx = new Regex("(?<=<HasConsole>)(.*)(?=</HasConsole>)");
                artmatch = rgx.Match(str);
                string hasConsole = artmatch.Groups[0].Value; //Groups[0], а не Groups[1]

                return Name + "  " + hasConsole;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "(function Cut -> входная строка - )" + str);
                return null;
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                findFiles = openFileDialog.FileNames;
            }
        }
    }

}
