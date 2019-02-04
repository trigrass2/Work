using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroids
{
    public class Asteroid : BaseObject
    {
        public int Power { get; set; }
        public Image imageAsteroid = Image.FromFile(@"resources\images\asteroid.png");

        /// <summary>
        /// Событие уничтожения астероида
        /// </summary>
        public static event Message MessageDie;

        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
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
            MessageDie?.Invoke();
        }

    }
}
