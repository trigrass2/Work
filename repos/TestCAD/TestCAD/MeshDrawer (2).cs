using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestCAD
{
    public class MeshDrawer
    {
        public class Sizes
        {
            private int _L, _L1, _B, _H, _l0, _n, _a, _a1, _b, _b1, _b_1, _b1_1;
            private string _d1, _d2, _d3, _Name;

            public int L { get => _L; set => _L = value; }
            public int L1 { get => _L1; set => _L1 = value; }
            public int B { get => _B; set => _B = value; }
            public int H { get => _H; set => _H = value; }
            public int l0 { get => _l0; set => _l0 = value; }
            public int n { get => _n; set => _n = value; }
            public int a { get => _a; set => _a = value; }
            public int a1 { get => _a1; set => _a1 = value; }
            public int b { get => _b; set => _b = value; }
            public int b1 { get => _b1; set => _b1 = value; }
            public int b_1 { get => _b_1; set => _b_1 = value; }
            public int b1_1 { get => _b1_1; set => _b1_1 = value; }
            public string d1 { get => _d1; set => _d1 = value; }
            public string d2 { get => _d2; set => _d2 = value; }
            public string d3 { get => _d3; set => _d3 = value; }
            public string Name { get => _Name; set => _Name = value; }


            public Sizes(int L, int L1, int B, int H, int l0, int n, int a, int a1, int b, int b1, int b_1, int b1_1, string d1, string d2, string d3, string Name)
            {
                _L = L;
                _L1 = L1;
                _B = B;
                _H = H;
                _l0 = l0;
                _n = n;
                _a = a;
                _a1 = a1;
                _b = b;
                _b1 = b1;
                _b_1 = b_1;
                _b1_1 = b1_1;
                _d1 = d1;
                _d2 = d2;
                _d3 = d3;
                _Name = Name;
            }
        }

        public Sizes sizes = new Sizes(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, String.Empty, String.Empty, String.Empty, String.Empty);
        public int scale = 1;
        public int dimlen = 500;
        public int dimlen2 = 70;
        public int dimspace = 50;
        public int spacebetween = 200;
        public int fontsize = 9;
        public int pencilsize1 = 3;
        public int pencilsize2 = 1;

        private Pen pencil = new Pen(Color.Black);
        public Font drawFont = null;
        public Graphics g = null;
        private SolidBrush drawBrush = new SolidBrush(Color.Red);

        private int center_x, center_y, y_L, y_L1, start_x_L, start_x_L1, end_x_L, end_x_L1, start_y_H, end_y_H, x_H;

        public MeshDrawer()
        {

        }

        public void Canvas_Paint(int width, int height, Graphics _g)
        {
            center_x = width / 2;
            center_y = height / 2;
            drawFont = new Font("Arial", fontsize * 10 / scale);
            g = _g;
            pencil.Width = pencilsize1;
            pencil.Color = Color.Black;

            DrawL();
            DrawL1();

            start_y_H = y_L1 - sizes.a1 / scale;
            end_y_H = start_y_H + sizes.H / scale;
            for (int i = 0; i <= sizes.n; i++)
                DrawCross(i);
            DrawH();
            pencil.Width = pencilsize2;
            pencil.Color = Color.Blue;

            DrawD1();
            DrawD2();
            DrawD3();

            SizeF StringSize = g.MeasureString(sizes.d1, drawFont);
            int shift = (int)StringSize.Width * scale;

            dimlen += shift;
            //Draw L1
            DrawDimensions(start_x_L1, end_x_L1, start_y_H - dimlen / scale, start_y_H - dimspace / scale, dimlen, sizes.L1.ToString(), StringAlignment.Center);
            if (sizes.b_1 != sizes.b)
            {
                DrawDimensions(start_x_L1, start_x_L + (sizes.b + sizes.l0 * 0) / scale, start_y_H - dimlen / 4 * 3 / scale, start_y_H - dimspace / scale, dimlen, sizes.b_1.ToString(), StringAlignment.Far);
            }
            if (sizes.b1_1 != sizes.b1)
            {
                DrawDimensions(start_x_L + (sizes.b + sizes.l0 * sizes.n) / scale, end_x_L1, start_y_H - dimlen / 4 * 3 / scale, start_y_H - dimspace / scale, dimlen, sizes.b1_1.ToString(), StringAlignment.Near);
            }
            DrawName();
            dimlen -= shift;


            StringSize = g.MeasureString(sizes.d2, drawFont);
            shift = (int)StringSize.Width * scale;

            dimlen += shift;
            //Draw L;
            DrawDimensions(start_x_L, end_x_L, end_y_H + dimlen / scale, end_y_H + dimspace / scale, dimlen, sizes.L.ToString(), StringAlignment.Center);
            //Draw l0;
            DrawDimensions(start_x_L + (sizes.b + sizes.l0 * 1) / scale, start_x_L + (sizes.b + sizes.l0 * 2) / scale, end_y_H + dimlen / 7 * 4 / scale, end_y_H + dimspace / scale, dimlen, sizes.l0.ToString(), StringAlignment.Center);
            //Draw n x l0;
            DrawDimensions(start_x_L + (sizes.b + sizes.l0 * 0) / scale, start_x_L + (sizes.b + sizes.l0 * sizes.n) / scale, end_y_H + dimlen / 4 * 3 / scale, end_y_H + dimspace / scale, dimlen, sizes.n.ToString() + " x " + sizes.l0.ToString(), StringAlignment.Center);
            //Draw b;
            DrawDimensions(start_x_L, start_x_L + (sizes.b + sizes.l0 * 0) / scale, end_y_H + dimlen / 4 * 3 / scale, end_y_H + dimspace / scale, dimlen, sizes.b.ToString(), StringAlignment.Far);
            //Draw b1;
            DrawDimensions(start_x_L + (sizes.b + sizes.l0 * sizes.n) / scale, end_x_L, end_y_H + dimlen / 4 * 3 / scale, end_y_H + dimspace / scale, dimlen, sizes.b1.ToString(), StringAlignment.Near);
            dimlen -= shift;

            //Draw H
            DrawDimensions(x_H + (dimspace + dimlen / 2) / scale, x_H + dimspace / scale, end_y_H, start_y_H, dimlen, sizes.H.ToString(), StringAlignment.Center);
            //Draw a
            DrawDimensions(x_H - dimspace / scale, x_H - (dimspace + dimlen / 2) / scale, end_y_H - sizes.a / scale, end_y_H, dimlen, sizes.a.ToString(), StringAlignment.Far);
            //Draw a1
            DrawDimensions(x_H - dimspace / scale, x_H - (dimspace + dimlen / 2) / scale, start_y_H, start_y_H + sizes.a1 / scale, dimlen, sizes.a1.ToString(), StringAlignment.Near);
            //Draw B
            DrawDimensions(x_H - dimspace / scale, x_H - (dimspace + dimlen / 2) / scale, start_y_H + sizes.a1 / scale, end_y_H - sizes.a / scale, dimlen, sizes.B.ToString(), StringAlignment.Center);
        }

        private void DrawDimensions(int start_x, int end_x, int start_y, int end_y, int dimlen, string text, StringAlignment alignment)
        {
            if (start_x < end_x)
            {
                Point start, end, center1, center2, startdiag1, enddiag1, startdiag2, enddiag2, ear1, ear2, ear3, ear4;
                start = new Point(start_x, end_y);
                end = new Point(end_x, end_y);
                center1 = new Point(start_x, start_y);
                center2 = new Point(end_x, start_y);
                startdiag1 = new Point(center1.X - dimlen2 / 2 / scale, center1.Y + dimlen2 / 2 / scale);
                enddiag1 = new Point(center1.X + dimlen2 / 2 / scale, center1.Y - dimlen2 / 2 / scale);
                startdiag2 = new Point(center2.X - dimlen2 / 2 / scale, center2.Y + dimlen2 / 2 / scale);
                enddiag2 = new Point(center2.X + dimlen2 / 2 / scale, center2.Y - dimlen2 / 2 / scale);
                ear1 = new Point(center1.X, (start_y > end_y) ? center1.Y + dimlen2 / scale : center1.Y - dimlen2 / scale);
                ear2 = new Point(center1.X - dimlen2 / scale, center1.Y);
                ear3 = new Point(center2.X + dimlen2 / scale, center2.Y);
                ear4 = new Point(center2.X, (start_y > end_y) ? center2.Y + dimlen2 / scale : center2.Y - dimlen2 / scale);
                Point[] points = { start, ear1, center1, ear2, center1, startdiag1, enddiag1, center1, ear3, center2, ear4, center2, startdiag2, enddiag2, center2, end };
                StringFormat drawFormat = new StringFormat
                {
                    Alignment = alignment,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawLines(pencil, points);
                switch (alignment)
                {
                    case StringAlignment.Center:
                        g.DrawString(text, drawFont, drawBrush, (center1.X + center2.X) / 2, center1.Y - drawFont.Size / 2, drawFormat);
                        break;
                    case StringAlignment.Far:
                        g.DrawString(text, drawFont, drawBrush, center1.X, center1.Y - drawFont.Size / 2, drawFormat);
                        break;
                    case StringAlignment.Near:
                        g.DrawString(text, drawFont, drawBrush, center2.X, center1.Y - drawFont.Size / 2, drawFormat);
                        break;
                }
            }
            else
            {
                Point start, end, center1, center2, startdiag1, enddiag1, startdiag2, enddiag2, ear1, ear2, ear3, ear4;
                start = new Point((start_y > end_y) ? end_x : start_x, end_y);
                end = new Point((start_y > end_y) ? end_x : start_x, start_y);
                center1 = new Point((start_y > end_y) ? start_x : end_x, end_y);
                center2 = new Point((start_y > end_y) ? start_x : end_x, start_y);
                startdiag1 = new Point(center1.X + dimlen2 / 2 / scale, center1.Y + dimlen2 / 2 / scale);
                enddiag1 = new Point(center1.X - dimlen2 / 2 / scale, center1.Y - dimlen2 / 2 / scale);
                startdiag2 = new Point(center2.X + dimlen2 / 2 / scale, center2.Y + dimlen2 / 2 / scale);
                enddiag2 = new Point(center2.X - dimlen2 / 2 / scale, center2.Y - dimlen2 / 2 / scale);
                ear1 = new Point((start_y > end_y) ? center1.X + dimlen2 / scale : center1.X - dimlen2 / scale, center1.Y);
                ear2 = new Point(center1.X, (start_y > end_y) ? center1.Y - dimlen2 / scale : center1.Y + dimlen2 / scale);
                ear3 = new Point(center2.X, (start_y > end_y) ? center2.Y + dimlen2 / scale : center2.Y - dimlen2 / scale);
                ear4 = new Point((start_y > end_y) ? center2.X + dimlen2 / scale : center2.X - dimlen2 / scale, center2.Y);
                Point[] points = { start, ear1, center1, ear2, center1, startdiag1, enddiag1, center1, ear3, center2, ear4, center2, startdiag2, enddiag2, center2, end };
                StringFormat drawFormat = new StringFormat
                {
                    Alignment = alignment,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawLines(pencil, points);
                switch (alignment)
                {
                    case StringAlignment.Center:
                        g.TranslateTransform(center1.X - drawFont.Size / 2, (center1.Y + center2.Y) / 2);
                        break;
                    case StringAlignment.Far:
                        g.TranslateTransform(center1.X - drawFont.Size / 2, center1.Y);
                        break;
                    case StringAlignment.Near:
                        g.TranslateTransform(center1.X - drawFont.Size / 2, center2.Y);
                        break;
                }
                g.RotateTransform(270);
                g.DrawString(text, drawFont, drawBrush, 0, 0, drawFormat);
                g.ResetTransform();
            }
        }

        private void DrawName()
        {
            SizeF StringSize = g.MeasureString(sizes.Name, drawFont);
            StringFormat drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawRectangle(pencil, (start_x_L1 >= start_x_L ? start_x_L : start_x_L1) - dimlen / 3 / scale - StringSize.Width, start_y_H - dimlen / scale - StringSize.Height / 2, StringSize.Width, StringSize.Height);
            g.DrawString(sizes.Name, drawFont, drawBrush, (start_x_L1 >= start_x_L ? start_x_L : start_x_L1) - dimlen / 3 / scale - StringSize.Width / 2, start_y_H - dimlen / scale, drawFormat);
        }

        private void DrawD1()
        {
            Point[] points =
            {
                new Point(start_x_L1 + (sizes.b_1 - sizes.l0 / 2 + sizes.l0 * (sizes.n / 4 * 3)) / scale, y_L1),
                new Point(start_x_L1 + (sizes.b_1 - sizes.l0 / 2 + sizes.l0 * (sizes.n / 4 * 3)) / scale, y_L1 - dimlen / 3 / scale)
            };
            g.DrawLines(pencil, points);
            SizeF StringSize = g.MeasureString(sizes.d1, drawFont);
            StringFormat drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawEllipse(pencil, points[1].X - StringSize.Width / 2, points[1].Y - StringSize.Width, StringSize.Width, StringSize.Width);
            g.DrawString(sizes.d1, drawFont, drawBrush, points[1].X, points[1].Y - StringSize.Width / 2, drawFormat);
        }

        private void DrawD2()
        {
            Point[] points =
            {
                new Point(start_x_L + (sizes.b + sizes.l0 / 2 + sizes.l0 * (sizes.n / 4 * 3)) / scale, y_L),
                new Point(start_x_L + (sizes.b + sizes.l0 / 2 + sizes.l0 * (sizes.n / 4 * 3)) / scale, y_L + dimlen / 3 / scale)
            };
            g.DrawLines(pencil, points);
            SizeF StringSize = g.MeasureString(sizes.d2, drawFont);
            StringFormat drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawEllipse(pencil, points[1].X - StringSize.Width / 2, points[1].Y, StringSize.Width, StringSize.Width);
            g.DrawString(sizes.d2, drawFont, drawBrush, points[1].X, points[1].Y + StringSize.Width / 2, drawFormat);
        }

        private void DrawD3()
        {
            Point[] points =
            {
                new Point((start_x_L1 >= start_x_L ? start_x_L : start_x_L1) + (sizes.b + sizes.l0 * sizes.n) / scale, center_y),
                new Point((start_x_L1 >= start_x_L ? start_x_L : start_x_L1) - dimlen / 3 / scale, center_y)
            };
            g.DrawLines(pencil, points);
            SizeF StringSize = g.MeasureString(sizes.d3, drawFont);
            StringFormat drawFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawEllipse(pencil, points[1].X - StringSize.Width, points[1].Y - StringSize.Width / 2, StringSize.Width, StringSize.Width);
            g.DrawString(sizes.d3, drawFont, drawBrush, points[1].X - StringSize.Width / 2, points[1].Y, drawFormat);
        }

        private void DrawH()
        {
            x_H = center_x + ((sizes.L > sizes.L1 ? sizes.L : sizes.L1) / 2 + spacebetween + dimlen / 3 * 2) / scale;
            Point[] points =
            {
                new Point(x_H, start_y_H),
                new Point(x_H, y_L1),
                new Point(x_H - (int)pencil.Width, y_L1),
                new Point(x_H, y_L1),
                new Point(x_H, y_L),
                new Point(x_H - (int)pencil.Width, y_L),
                new Point(x_H, y_L),
                new Point(x_H, end_y_H)
            };
            g.DrawLines(pencil, points);
        }

        private void DrawL()
        {
            start_x_L = center_x - sizes.L / 2 / scale;
            end_x_L = start_x_L + sizes.L / scale;
            y_L = center_y + sizes.B / 2 / scale;
            Point[] points =
            {
                new Point(start_x_L, y_L),
                new Point(end_x_L, y_L)
            };
            g.DrawLines(pencil, points);
        }

        private void DrawL1()
        {
            start_x_L1 = start_x_L + sizes.b / scale - sizes.b_1 / scale;
            end_x_L1 = start_x_L1 + sizes.L1 / scale;
            y_L1 = center_y - sizes.B / 2 / scale;
            Point[] points =
            {
                new Point(start_x_L1, y_L1),
                new Point(end_x_L1, y_L1)
            };
            g.DrawLines(pencil, points);
        }

        private void DrawCross(int x)
        {
            Point[] points =
            {
                new Point(start_x_L + sizes.b / scale + sizes.l0 * x / scale, start_y_H),
                new Point(start_x_L + sizes.b / scale + sizes.l0 * x / scale, end_y_H)
            };
            g.DrawLines(pencil, points);
        }
    }
}
