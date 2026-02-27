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

namespace PR5_aksenov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();
        }

        
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            
            if (!TryParseParameters(out double x0, out double xk, out double dx, out double b))
                return;

            
            List<Point> points = new List<Point>();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            
            for (double x = x0; x <= xk + 1e-9; x += dx)
            {
                double y = Math.Pow(x, 4) + Math.Cos(2 + Math.Pow(x, 3) - b);
                points.Add(new Point(x, y));
                sb.AppendLine($"x = {x:F2}\ty = {y:F4}");
            }

            
            txtResults.Text = sb.ToString();

            
            DrawGraph(points, x0, xk);
        }

        
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtResults.Clear();
            GraphCanvas.Children.Clear();
        }

       
        private bool TryParseParameters(out double x0, out double xk, out double dx, out double b)
        {
            x0 = xk = dx = b = 0;

            if (!double.TryParse(txtX0.Text, out x0))
            {
                MessageBox.Show("Неверное значение x₀", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!double.TryParse(txtXk.Text, out xk))
            {
                MessageBox.Show("Неверное значение xₖ", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!double.TryParse(txtDx.Text, out dx))
            {
                MessageBox.Show("Неверное значение dx", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!double.TryParse(txtB.Text, out b))
            {
                MessageBox.Show("Неверное значение b", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            
            if (dx <= 0)
            {
                MessageBox.Show("Шаг dx должен быть положительным", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (xk < x0)
            {
                MessageBox.Show("xₖ должно быть больше или равно x₀", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        // Метод для отрисовки графика на Canvas
        private void DrawGraph(List<Point> points, double minX, double maxX)
        {
            if (points.Count == 0) return;

            GraphCanvas.Children.Clear();

            double minY = double.MaxValue;
            double maxY = double.MinValue;
            foreach (var p in points)
            {
                if (p.Y < minY) minY = p.Y;
                if (p.Y > maxY) maxY = p.Y;
            }

            double yPadding = (maxY - minY) * 0.1;
            if (yPadding == 0) yPadding = 0.1;
            minY -= yPadding;
            maxY += yPadding;

            double canvasWidth = 600;
            double canvasHeight = 400;
            double margin = 50;

            Func<double, double> mapX = (x) =>
            {
                return margin + (x - minX) / (maxX - minX) * (canvasWidth - 2 * margin);
            };

            Func<double, double> mapY = (y) =>
            {
                return (canvasHeight - margin) - (y - minY) / (maxY - minY) * (canvasHeight - 2 * margin);
            };

            // Ось X
            double yZeroPos = mapY(0);
            if (0 < minY || 0 > maxY)
                yZeroPos = canvasHeight - margin;

            Line xAxis = new Line
            {
                X1 = margin,
                Y1 = yZeroPos,
                X2 = canvasWidth - margin,
                Y2 = yZeroPos,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            GraphCanvas.Children.Add(xAxis);

            // Ось Y
            double xZeroPos = mapX(0);
            if (0 < minX || 0 > maxX)
                xZeroPos = margin;

            Line yAxis = new Line
            {
                X1 = xZeroPos,
                Y1 = margin,
                X2 = xZeroPos,
                Y2 = canvasHeight - margin,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            GraphCanvas.Children.Add(yAxis);

            // График
            Polyline polyline = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                Points = new PointCollection()
            };

            foreach (var p in points)
            {
                polyline.Points.Add(new Point(mapX(p.X), mapY(p.Y)));
            }
            GraphCanvas.Children.Add(polyline);

            // Подписи граничных значений
            AddLabel($"{minX:F2}", margin, yZeroPos + 15, 12, HorizontalAlignment.Center);
            AddLabel($"{maxX:F2}", canvasWidth - margin, yZeroPos + 15, 12, HorizontalAlignment.Center);
            AddLabel($"{maxY:F2}", xZeroPos - 30, margin - 5, 12, HorizontalAlignment.Right);
            AddLabel($"{minY:F2}", xZeroPos - 30, canvasHeight - margin - 15, 12, HorizontalAlignment.Right);
        }

        private void AddLabel(string text, double x, double y, double fontSize, HorizontalAlignment align)
        {
            TextBlock tb = new TextBlock
            {
                Text = text,
                FontSize = fontSize,
                Foreground = Brushes.Black,
                Background = Brushes.White
            };
            Canvas.SetLeft(tb, x);
            Canvas.SetTop(tb, y);

            if (align == HorizontalAlignment.Center)
                tb.Margin = new Thickness(-15, 0, 0, 0);
            else if (align == HorizontalAlignment.Right)
                tb.Margin = new Thickness(-30, 0, 0, 0);

            GraphCanvas.Children.Add(tb);
        }
    }
}
