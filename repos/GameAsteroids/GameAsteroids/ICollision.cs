using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAsteroids
{
    public interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }

    }
}
