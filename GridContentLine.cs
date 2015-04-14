using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Core.UI
{
    public static class GridContentLine
    {
        public static readonly DependencyProperty GridLineProperty =
            DependencyProperty.RegisterAttached("GridLine", typeof(bool), typeof(GridContentLine),
                new PropertyMetadata(AttachOrRemoveGridLineProperty));

        private static void AttachOrRemoveGridLineProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null)
                return;
            if (e.NewValue == null)
                return;
            grid.Loaded += (sender, args) => GetValue(sender as Grid);
        }

        private static void GetValue(Grid grid)
        {
            grid.Children.Cast<UIElement>()
                .Where(element => element != null)
                .Select(element => new
                {
                    row = (int) element.GetValue(Grid.RowProperty),
                    column = (int) element.GetValue(Grid.ColumnProperty),
                    rowSpan = (int) element.GetValue(Grid.RowSpanProperty),
                    colSpan = (int) element.GetValue(Grid.ColumnSpanProperty)
                })
                .ToList()
                .ForEach(position =>
                {
                    var rectangle = new Rectangle
                    {
                        Stroke = new SolidColorBrush(Colors.Black),
                        StrokeThickness = 0.2f
                    };

                    Grid.SetRow(rectangle, position.row);
                    Grid.SetColumn(rectangle, position.column);
                    Grid.SetRowSpan(rectangle, position.rowSpan);
                    Grid.SetColumnSpan(rectangle, position.colSpan);

                    grid.Children.Add(rectangle);
                });
        }

        public static bool GetGridLine(DependencyObject obj)
        {
            return (bool)obj.GetValue(GridLineProperty);
        }

        public static void SetGridLine(DependencyObject obj, bool value)
        {
            obj.SetValue(GridLineProperty, value);
        }
    }
}
