using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaDynamicApp.Views
{
    public partial class GridPageView : UserControl
    {
        private readonly int _rows = 10;
        private readonly int _columns = 10;
        private readonly IBrush _baseColor = Brushes.LightGray;
        private bool _stop;

        public GridPageView()
        {
            InitializeComponent();

            _rows = _columns = OperatingSystem.IsAndroid() || OperatingSystem.IsIOS() ? 6 : 10;

            InitGrid();
        }

        private void InitGrid()
        {
            CodeGrid.Height = 50 * _rows;
            CodeGrid.Width = 50 * _columns;

            CodeGrid.ShowGridLines = true;

            for (var j = 0; j < _rows; j++)
            {
                CodeGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
                CodeGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
                for (var i = 0; i < _columns; i++)
                {
                    var child = new Rectangle
                    {
                        Width = 40,
                        Height = 40,
                        Margin = new Thickness(1),
                        Fill = _baseColor,
                    };
                    CodeGrid.Children.Add(child);
                    Grid.SetRow(child, j);
                    Grid.SetColumn(child, i);
                }
            }
        }

        private async void Start_OnClick(object? sender, RoutedEventArgs e)
        {
            _stop = false;
            Rectangle? previous = null;
            foreach (var child in CodeGrid.Children)
            {
                if (_stop) break;
                if (child is not Rectangle rect) continue;
                if (previous != null) previous.Fill = _baseColor;
                rect.Fill = Brushes.Orchid;
                previous = rect;
                await Task.Delay(50);
            }
            if (previous != null) previous.Fill = _baseColor;
        }

        private void Stop_OnClick(object? sender, RoutedEventArgs e)
        {
            _stop = true;
            ClearGrid();
        }

        private void ClearGrid()
        {
            foreach (var child in CodeGrid.Children)
            {
                if (child is not Rectangle rect) continue;
                rect.Fill = _baseColor;
            }
        }

        private Dictionary<Rectangle, double> CalculateDistancesFromCenter()
        {
            int centerX = (_columns / 2) + 1;
            int centerY = (_rows / 2) - 1;
            var distances = new Dictionary<Rectangle, double>();

            foreach (var child in CodeGrid.Children)
            {
                if (child is not Rectangle rect) continue;
                int x = Grid.GetColumn(rect);
                int y = Grid.GetRow(rect);
                double distance = Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2));
                distances[rect] = Math.Abs(distance);
            }

            return distances;
        }

        private async void Start_OnClick_Ripple(object? sender, RoutedEventArgs e)
        {
            var distances = CalculateDistancesFromCenter();
            var maxDistance = distances.Values.Max();

            for (double t = 0; t <= maxDistance; t += 0.1)
            {
                foreach (var pair in distances)
                {
                    if (pair.Value <= t && pair.Value >= t - 1)
                    {
                        pair.Key.Fill = Brushes.ForestGreen;
                    }else
                        pair.Key.Fill = _baseColor;
                }
                await Task.Delay(20);
            }
            ClearGrid();
        }
    }
}
