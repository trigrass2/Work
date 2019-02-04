using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroids
{
    public abstract class BaseObject : ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        public Rectangle Rect => new Rectangle(Pos, Size);
        public delegate void Message();

        /// <summary>
        /// Инициализирует новый экземпляр BaseObject
        /// </summary>
        /// <param name="pos">начальная позиция объекта</param>
        /// <param name="dir">направление</param>
        /// <param name="size">размер</param>
        public BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }

        /// <summary>
        /// Рисует объект
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Обновляет координаты объекта
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// определяет столкновение двух объектов
        /// </summary>
        /// <param name="o">объект столкновения</param>
        /// <returns></returns>
        public virtual bool Collision(ICollision o)
        {
            return o.Rect.IntersectsWith(this.Rect);
        }

        /// <summary>
        /// регенерирует объект после столкновения
        /// </summary>
        public virtual void Regeneration()
        {
            Pos.Y = 0;
        }

    }
}
