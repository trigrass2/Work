using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RestApi;

namespace PersonManager
{
    public partial class Form1 : Form
    {
        private C1CPersonBindingModel person;
        private List<C1CPersonBindingModel> tempPerson { get; set; }
        private string selectedGroupID { get; set; }
        private List<C1CGroupBindingModel> AllGroup = new List<C1CGroupBindingModel>
        {
            new C1CGroupBindingModel{GUID = "outstaff-322aa5de-bbe5-4b21-8c5c-6370c7c62579", Name = "АУТСТАФФ КЗЖБК"},
            new C1CGroupBindingModel{GUID = "outstaff-83291711-d79d-4cce-a02b-ffea7734a4c3", Name = "АУТСТАФФ АЗЖБК"},
            new C1CGroupBindingModel{GUID = "outstaff-53659f3c-ebea-475a-8677-95d44991d00b", Name = "АУТСТАФФ ОЗЖБК"},
            new C1CGroupBindingModel{GUID = "outstaff-c0f2ffb8-39c2-40e9-ad9f-b26001f72b23", Name = "АУТСТАФФ НЗЖБК"}
        };

        byte[] imageArray;

        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(AllGroup.Select(x => x.Name).ToArray());
            textBox1.Text = "outstaff-" + Guid.NewGuid();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            
        }        

        private void button2_Click(object sender, EventArgs e)
        {
            person = new C1CPersonBindingModel
            {
                GUID = textBox1.Text,
                Name = textBox3.Text,
                TabNum = textBox4.Text,
                Photo = imageArray
            };
            var persons = new List<C1CPersonBindingModel>();
            
            Class1.AddOrUpdatePerson(person);

            var personGroup = new C1CPersonGroupBindingModel()
            {
                GroupGUID = textBoxGroupID.Text,
                PersonGUID = textBox1.Text,
                StatusID = textBox5.Text
            };
            Class1.AddOrUpdatePersonStatus(personGroup);
        }

        public static byte[] ConverterImageToByte(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }
        
        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedState = comboBox1.SelectedItem.ToString();
            selectedGroupID = AllGroup.Where(x => x.Name == selectedState).Select(x => x.GUID).FirstOrDefault();
            textBoxGroupID.Text = selectedGroupID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                var image = Image.FromFile(openFileDialog.FileName);
                pictureBox1.Image = image;
                MemoryStream ms = new MemoryStream();
                image.Save(ms, image.RawFormat);
                imageArray = ms.ToArray();
            }
                            
        }
    }
}
