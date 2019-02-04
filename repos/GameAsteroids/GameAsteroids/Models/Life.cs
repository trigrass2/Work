using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroids.Models
{
    public class Life : BaseObject
    {       
        public Image imageAsteroid = Image.FromFile(@"resources\images\energy.png");
        
        public Life(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(imageAsteroid, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.Y > Game.Height) Pos.Y = Size.Height;
        }

        /// <summary>
        /// регенерирует объект после столкновения
        /// </summary>
        public override void Regeneration()
        {
            Pos.Y = -200;
        }
    }
}
