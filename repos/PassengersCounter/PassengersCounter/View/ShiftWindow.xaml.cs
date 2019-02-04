using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PassengersCounter.ViewModel;


namespace PassengersCounter.View
{
    /// <summary>
    /// Логика взаимодействия для ShiftWindow.xaml
    /// </summary>
    public partial class ShiftWindow : Window
    {
        public ShiftWindowViewModel ShiftWindowViewModel { get; set; }
        public ShiftWindow()
        {
            InitializeComponent();
            ShiftWindowViewModel = new ShiftWindowViewModel();
            DataContext = ShiftWindowViewModel;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        //private string[] _selectShift;
        //public string[] SelectShift
        //{
        //    get { return ListShifts.SelectedItem; }
        //    set
        //    {
        //        _selectShift = value;
        //        //RaisePropertyChanged();
        //    }
        //}
    }
}
