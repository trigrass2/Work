using System.Collections.Generic;
using System.Windows.Forms;
using PlateStacker;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        List<Plate> plates;
        List<List<Rope>> allRopes;
        public Form1()
        {
            InitializeComponent();
            AddPlates();
            dataGridView1.DataSource = Placement.GetResult(plates, 50000);
            dataGridView1.DataSource = Placement.GetResult(plates, 50000);
        }

        private void AddPlates()
        {
            allRopes = new List<List<Rope>>();

            for(int i = 0; i < 9; i++)
            {
                allRopes.Add(new List<Rope> { new Rope { Diameter = 12, Location = "низ" } });
            }
            for (int i = 0; i < 7; i++)
            {
                allRopes.Add(new List<Rope> { new Rope { Diameter = 9, Location = "верх" } });
            }

            plates = new List<Plate>();

            for (int i = 0; i < 10; i++)
            {
                plates.Add(new Plate { Name = "Плита 1", Length = 3000, Width = 1200, Height = 180, Concrete = "бетон1", Loop = "нет", TypeNode = "Тип1", AllRopes = allRopes });
            }

            for (int i = 0; i < 10; i++)
            {
                plates.Add(new Plate { Name = "Плита 2", Length = 2500, Width = 1200, Height = 180, Concrete = "бетон1", Loop = "нет", TypeNode = "Тип3", AllRopes = allRopes });
            }

            for (int i = 0; i < 15; i++)
            {
                plates.Add(new Plate { Name = "Плита 3", Length = 2300, Width = 1200, Height = 180, Concrete = "бетон1", Loop = "нет", TypeNode = "Тип1", AllRopes = allRopes });
            }

            for (int i = 0; i < 5; i++)
            {
                plates.Add(new Plate { Name = "Плита 4", Length = 2000, Width = 1200, Height = 180, Concrete = "бетон1", Loop = "нет", TypeNode = "Тип2", AllRopes = allRopes });
            }

            for (int i = 0; i < 10; i++)
            {
                plates.Add(new Plate { Name = "Плита 5", Length = 1900, Width = 1200, Height = 180, Concrete = "бетон1", Loop = "нет", TypeNode = "Тип2", AllRopes = allRopes });
            }
        }
    }
}
