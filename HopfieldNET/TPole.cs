using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace HopfieldNET
{
    class TPole
    {
        Canvas g; // холст для поля
        public int NN; // количество клеток N*N
        double dx; // ширина в пикселях клетки

        Rectangle[,] Rs; // клетки

        public TBox Box;

        public TPole(Canvas g)
        {
            this.g = g;

            NN = 5;
            dx = g.Width / NN; // ширина в пикселях клетки

            Box = new TBox(NN);

            Rs = new Rectangle[NN, NN];

            Draw(); // перерисовать поле

            g.MouseUp += Check;
        }

        private void Check(object sender, MouseButtonEventArgs e)
        {
            IsCheck(e.GetPosition(g).X, e.GetPosition(g).Y);
        }

        // отметить клетку с заданными координатами
        public void IsCheck(double x, double y)
        {
            // проверить диапазон
            if((x < 0)||(x > g.Width)) { return; }
            if ((y < 0) || (y > g.Height)) { return; }

            // расчитать номер клетки
            int i = Convert.ToInt16(Math.Floor(x / dx));
            int j = Convert.ToInt16(Math.Floor(y / dx));

            if (Rs[i, j] == null)
            {
                DrawCell(i, j);

                Box[i, j] = 1;
            }
            else
            {
                g.Children.Remove(Rs[i, j]);
                Rs[i, j] = null;

                Box[i, j] = -1;
            }
        }

        // рисовать клетку
        public void DrawCell(int i, int j)
        {
            Rs[i, j] = new Rectangle();

            Rs[i, j].Fill = Brushes.Green;
            Rs[i, j].Margin = new Thickness(i * dx, j * dx, 0, 0);
            Rs[i, j].Height = dx;
            Rs[i, j].Width = dx;
            g.Children.Add(Rs[i, j]);
        }

        // перерисовать поле
        public void Draw()
        {
            g.Children.Clear();
            g.Background = Brushes.LightGray; // фон

            for (int k = 0; k <= NN; k++)
            {
                // рисуем горизонтальные линии
                Line l = new Line(); 
                l.Stroke = Brushes.Black;
                l.X1 = 0;
                l.X2 = g.Width;
                l.Y1 = k * dx;
                l.Y2 = k * dx;
                l.StrokeThickness = 1;
                g.Children.Add(l);

                // рисуем вертикальные линии
                l = new Line();
                l.Stroke = Brushes.Black;
                l.X1 = k * dx;
                l.X2 = k * dx;
                l.Y1 = 0;
                l.Y2 = g.Height;
                l.StrokeThickness = 1;
                g.Children.Add(l);
            }

            for (int i = 0; i < NN; i++)
            {
                for (int j = 0; j < NN; j++)
                {
                    if(Box[i, j] > 0)
                    {
                        DrawCell(i, j);
                    }
                }
            }
        }
    }
}
