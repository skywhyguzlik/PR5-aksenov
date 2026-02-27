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
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private double CalculateFx(double x)
        {
            if (rbSinh.IsChecked == true)
            {
                
                return Math.Sinh(x);
            }
            else if (rbSquare.IsChecked == true)
            {
                
                return x * x;
            }
            else if (rbExp.IsChecked == true)
            {
                
                return Math.Exp(x);
            }
            return 0;
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(txtX.Text) ||
                    string.IsNullOrWhiteSpace(txtI.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля!",
                                  "Ошибка ввода",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Warning);
                    return;
                }

                
                double x = double.Parse(txtX.Text.Replace(',', '.'),
                                      System.Globalization.CultureInfo.InvariantCulture);
                int i = int.Parse(txtI.Text);

                
                double fx = CalculateFx(x);

                double result = 0;

                
                bool isOdd = (i % 2 != 0);

                

                if (isOdd && x > 0)
                {
                    
                    if (fx < 0)
                    {
                        MessageBox.Show("f(x) < 0 при условии 'i нечётное и x > 0'. Невозможно извлечь корень!",
                                      "Ошибка вычисления",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Warning);
                        return;
                    }
                    result = i * Math.Sqrt(fx);
                }
                else if (!isOdd && x < 0)
                {
                    
                    result = (i / 2.0) * Math.Sqrt(Math.Abs(fx));
                }
                else
                {
                    
                    result = Math.Sqrt(Math.Abs(i * fx));
                }

                
                if (double.IsNaN(result) || double.IsInfinity(result))
                {
                    MessageBox.Show("Результат вычисления некорректен!",
                                  "Ошибка вычисления",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    return;
                }

                txtResult.Text = result.ToString("F10");
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения!",
                              "Ошибка формата",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtX.Clear();
            txtI.Clear();
            txtResult.Clear();
            rbSinh.IsChecked = true;
            txtX.Focus();
        }
    }
}