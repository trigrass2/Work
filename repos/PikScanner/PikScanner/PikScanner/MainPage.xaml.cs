using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Symbol.Barcode;
using System.ComponentModel;

namespace PikScanner
{
    public partial class MainPage : ContentPage
    {
        //private BarcodeReader barcode_reader = new BarcodeReader();

        //private Label label = new Label();        
        //private StackLayout stackLayout = new StackLayout();
        //private Button button = new Button();

        public MainPage()
        {
            InitializeComponent();

            //stackLayout.Children.Add(label);
            //stackLayout.Children.Add(button);
            //Content = stackLayout;

            //barcode_reader.ListChanged += new ListChangedEventHandler(reader_ListChanged);
            //barcode_reader.Start();
        }

        //void reader_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    Symbol.Barcode.ReaderData nextReaderData = barcode_reader.ReaderData;
        //    if (nextReaderData.Result == Symbol.Results.SUCCESS)
        //    {
        //        //Помещаем полученное значение в текстовое поле
        //        label.Text = nextReaderData.Text;
        //    }
        //    else
        //    {
        //        label.Text = "Формат считанного Штрихового Кода - не распознан!";
        //        //Проигрываем звук - ненайденного ШК
        //        //System.Media.SystemSounds.Exclamation.Play();
        //        //System.Media.SystemSounds.Asterisk.Play();
        //        //System.Media.SystemSounds.Exclamation.Play();
        //        //System.Media.SystemSounds.Asterisk.Play();
        //    }
        //}
    }
}
