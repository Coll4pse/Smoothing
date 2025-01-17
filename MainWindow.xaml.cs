﻿using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Smoothing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TextBox> textBoxesX = new List<TextBox>();
        private List<TextBox> textBoxesY = new List<TextBox>();

        public MainWindow()
        {
            InitializeComponent();
            ChangeTable(3);
        }

        private void ChangeTable(int columns)
        {
            foreach (var textBox in textBoxesX.Concat(textBoxesY))
            {
                table.Children.Remove(textBox);
            }
            
            textBoxesX.Clear();
            textBoxesY.Clear();
            
            for (int i = 1; i < columns + 1; i++)
            {
                var textBoxX = TextBoxes.CreateTextBox();
                textBoxesX.Add(textBoxX);
                table.Children.Add(textBoxX);
                Grid.SetColumn(textBoxX, i);
                
                var textBoxY = TextBoxes.CreateTextBox();
                textBoxesY.Add(textBoxY);
                table.Children.Add(textBoxY);
                Grid.SetColumn(textBoxY, i);
                Grid.SetRow(textBoxY, 1);
            }
        }

        private void OnCounterChange(object sender, RoutedEventArgs args)
        {
            ChangeTable(counter.Value ?? 3);
        }

        private void OnClickEvaluateButton(object sender, RoutedEventArgs args)
        {
            Evaluate();
        }

        private void Evaluate()
        {
            try
            {
                var x = textBoxesX
                    .Select(box => double.Parse(box.Text.Replace(',', '.'), CultureInfo.InvariantCulture))
                    .ToArray();
                var y = textBoxesY
                    .Select(box => double.Parse(box.Text.Replace(',', '.'), CultureInfo.InvariantCulture))
                    .ToArray();
                
                var result = linear.IsChecked ?? true
                    ? LinearApproximation.Evaluate(x, y)
                    : ParabolicApproximation.Evaluate(x, y);
                answer.Text = result.ToString();
                CheckAnswer(x, y, result);
            }
            catch (FormatException)
            {
                MessageBox.Show("Неккоретные вводные данные! Должны быть только действительные числа",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private static void CheckAnswer(double[] x, double[] y, Polynomial polynomial)
        {
            var builder = new StringBuilder();
            builder.Append("x\ty\ty(x)\t(y-y(x))\u00B2\n\n");
            var yX = x.Select(polynomial.Evaluate).ToArray();
            var error = yX.Zip(y ,(yX, y) => (y - yX) * (y - yX)).ToArray();
            for (int i = 0; i < x.Length; i++)
            {
                builder.Append($"{x[i]}\t{y[i]}\t{yX[i]:F}\t{error[i]:G1}\n");
            }

            builder.Append($"\t \tSum:\t{error.Sum():G1}");
            MessageBox.Show(builder.ToString(), "Погрешности", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}