using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroids
{
    public class Bullet : BaseObject
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillRectangle(Brushes.Yellow, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.Y -= 20;            
        }

    }
}
