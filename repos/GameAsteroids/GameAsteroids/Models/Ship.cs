using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroids
{
    /// <summary>
    /// Класс "Корабль"
    /// </summary>
    public class Ship : BaseObject
    {
        const int maxEnergy = 100;
        private int _energy = maxEnergy;
        /// <summary>
        /// энергия корабля
        /// </summary>
        public int Energy => _energy;

        /// <summary>
        /// Событие смерти корабля
        /// </summary>
        public static event Message MessageDie;

        public Image imageShip = Image.FromFile(@"resources\images\ship.png");
        

        /// <summary>
        /// уменьшает количество энергии
        /// </summary>
        /// <param name="n">количество потерянной энергии</param>
        public void EnergyLow(int n)
        {
            _energy -= n;
        }

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {            
            Game.Buffer.Graphics.DrawImage(imageShip, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
        }

        /// <summary>
        /// движение вверх
        /// </summary>
        public void Left()
        {
            if (Pos.Y > 0) Pos.X = Pos.X - Dir.X;
        }
        /// <summary>
        /// движение вниз
        /// </summary>
        public void Right()
        {
            if (Pos.Y < Game.Height) Pos.X = Pos.X + Dir.X;
        }

        /// <summary>
        /// Корабль погибает
        /// </summary>
        public void Die()
        {
            MessageDie?.Invoke();
        }

        /// <summary>
        /// Лечит корабль
        /// </summary>
        /// <param name="countLife">количество единиц лечения</param>
        public void Healing(int countLife)
        {            
            if (_energy < 100)
                _energy += (maxEnergy - _energy) > countLife ? countLife : (maxEnergy - _energy);
        }

    }
}
