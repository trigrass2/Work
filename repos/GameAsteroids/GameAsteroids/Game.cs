using GameAsteroids.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameAsteroids
{
    public class Game
    {
        /// <summary>
        /// графический контекст
        /// </summary>
        private static BufferedGraphicsContext _context;
        /// <summary>
        /// графический буфер
        /// </summary>
        public static BufferedGraphics Buffer;

        /// <summary>
        /// ширина игрового поля
        /// </summary>
        public static int Width { get; set; }
        /// <summary>
        /// высота игрового поля
        /// </summary>
        public static int Height { get; set; }

        /// <summary>
        /// Космический корабль
        /// </summary>
        private static Ship _ship = new Ship(new Point(400, 500), new Point(10, 10), new Size(50, 50));

        /// <summary>
        /// таймер обновления объектов
        /// </summary>
        private static Timer _timer = new Timer { Interval = 100 };        

        public static Random Rnd = new Random();

        static Game()
        {
        }

        /// <summary>
        /// запускает игровое поле
        /// </summary>
        /// <param name="form">форма для вывода игры</param>
        public static void Init(Form form, int width, int height)
        {
            if (width > 1000 || height > 1000 || width < 0 || height < 0)
            {
                throw new ArgumentOutOfRangeException("размеры экрана не должны превышать 1000 или иметь отрицательное значение!");
            }

            Width = width;
            Height = height;
            Graphics g;

            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();

            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;
            Asteroid.MessageDie += Sound.HittingAnAsteroid;
            Load();

            
            _timer.Start();
            _timer.Tick += Timer_Tick;
        }

        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("Game Over", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }

        /// <summary>
        /// управляет кораблем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _bullet = new Bullet(new Point(_ship.Rect.X + 23, _ship.Rect.Y + 4),
                                    new Point(4, 0),
                                    new Size(3, 6));
                Sound.ShipShot();
            }
            if (e.KeyCode == Keys.Left) _ship.Left();
            if (e.KeyCode == Keys.Right) _ship.Right();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// прорисовывает объекты игры
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);

            foreach (BaseObject obj in _objs)
            {
                obj?.Draw();
            }

            foreach (var a in _asteroids)
            {
                a?.Draw();
            }

            _bullet?.Draw();
            _ship?.Draw();
            _life?.Draw();
            if(_ship != null)
            {
                Buffer.Graphics.DrawString($"Energy:{_ship.Energy}", SystemFonts.DefaultFont, Brushes.White, 0, 0);
            }
            Buffer.Render();
        }


        public static BaseObject[] _objs;
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        private static Life _life = new Life(new Point(Rnd.Next(0, 800), -200), new Point(5,5), new Size(35,35));

        /// <summary>
        /// инициализирует объекты игры
        /// </summary>
        public static void Load()
        {
            _objs = new BaseObject[30];
            _asteroids = new Asteroid[3];            

            for (var i = 0; i < _objs.Length; i++)
            {
                int r = Rnd.Next(5, 50);
                _objs[i] = new Star(new Point(Rnd.Next(0, Width), -200), new Point(r, r), new Size(7, 7));
            }

            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = Rnd.Next(15, 70);
                
                _asteroids[i] = new Asteroid(new Point(Rnd.Next(0, Width), -200), new Point(r, r/5), new Size(r, r));
            }

        }

        /// <summary>
        /// изменяет состояния объектов
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject obj in _objs)
            {
                obj.Update();
            }
            
            for(int i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                if (_bullet != null && _bullet.Collision(_asteroids[i]))
                {
                    Sound.HittingAnAsteroid();
                    _asteroids[i].Regeneration();
                    _bullet = null;
                    continue;
                }
                if (!_ship.Collision(_asteroids[i])) continue;               
                _ship?.EnergyLow(Rnd.Next(1,10));
                Sound.ShipDamaged();
                _asteroids[i].Regeneration();
                if (_ship.Energy <= 0) _ship?.Die();
            }
            if (_ship.Collision(_life))
            {
                _ship.Healing(Rnd.Next(5, 20));
            }
            _bullet?.Update();
            _life?.Update();
        }


    }
}
