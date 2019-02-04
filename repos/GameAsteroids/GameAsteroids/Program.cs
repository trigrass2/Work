using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAsteroids
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Width = 800;
            form.Height = 600;
            Game.Init(form, form.Width, form.Height);
            form.Show();
            Game.Draw();
            Application.Run(form);
        }
    }
}
