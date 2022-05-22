using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App
{
    public partial class Form1 : Form
    {
        Point start, end, current;

        double distance_from_current_to_end = double.MaxValue;
        double angle = 0;
        int N = 15;
        public Form1()
        {
            InitializeComponent();
            start = new Point(0, 0);
            end = new Point(7000, 7000);
            current = start;
            angle = Atan(Math.Abs(end.Y - start.Y) / Math.Abs(end.X - start.X));

            Paint += Form1_Paint;
            Timer timer = new Timer { Interval = 400 };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graph = CreateGraphics();
            graph.Clear(Color.White);
            graph.ScaleTransform((float)Width / (end.X + 800), (float)Height / (end.Y + 800));

            /* Start painting static figures */
            graph.DrawEllipse(new Pen(Color.Black, 2), start.X, start.Y, 100, 100);
            graph.DrawEllipse(new Pen(Color.Black, 2), end.X, end.Y, 100, 100);
            /* End painting static figures */

            /* Next point computation */
            double step = 1000;
            Point next_point = new Point(
                (int)(current.X + step * Cos(angle)),
                (int)(current.Y + step * Sin(angle))
            );

            double new_distance = Math.Sqrt(Math.Pow(end.X - next_point.X, 2) + Math.Pow(end.Y - next_point.Y, 2));
            if (new_distance < distance_from_current_to_end)
            {
                current = next_point;
                distance_from_current_to_end = new_distance;
            }
            /* End next point computation */

            /* Path's meta-data drawing */
            graph.DrawLine(new Pen(Color.Red, 10), start, current);
            graph.DrawString(
                /*text=*/$"Distance to the end: {distance_from_current_to_end}",
                /*font=*/new Font("Arial", 100),
                /*pen=*/new SolidBrush(Color.Black),
                /*x=*/100,
                /*y=*/0
            );
        }

        double Sin(double x)
        {
            double res = 0;
            for (int i = 1; i < N; i++)
            {
                res += (Math.Pow(-1, i - 1) * (Math.Pow(x, 2 * i - 1) / Factor(2 * i - 1)));
            }
            return res;
        }

        public double Cos(double x)
        {
            double res = 0;
            for (int i = 1; i < N; i++)
            {
                res += Math.Pow(-1, i - 1) * (Math.Pow(x, 2 * i - 2) / Factor(2 * i - 2));
            }
            return res;
        }

        public double Atan(double x)
        {
            double res = 0;
            if (-1 <= x && x <= 1)
            {
                for (int i = 1; i < N + 1; i++)
                {
                    res += Math.Pow(-1, i - 1) * Math.Pow(x, 2 * i - 1) / (2 * i - 1);
                }
            }
            else
            {
                res += (Math.PI / 2) * Math.Pow(-1, (double)(1 + Convert.ToInt32(x >= 1)));
                for (int i = 0; i < N; i++)
                {
                    res -= Math.Pow(-1, i) / ((2 * i + 1) * Math.Pow(x, 2 * i + 1));
                }
            }
            return res;
        }

        int Factor(int x)
        {
            return x > 1 ? x * Factor(x - 1) : 1;
        }
    }
}
