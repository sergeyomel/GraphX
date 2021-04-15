using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Graph2
{
    public class Vertex
    {
        private string _name = "top";
        private int _ID;
        private Point _center;
        private double _r;

        private int _strokeThikness = 2;
        private Brush _fillColor = new SolidColorBrush(Color.FromRgb(255, 123, 78));
        private Brush _strokeColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private Canvas _canvas;

        public Vertex(Point center, int id, Canvas canvas)
        {
            _center = center;
            _ID = id;
            _r = 25;
            _canvas = canvas;
        }
        public Vertex(string name, int id, Point center, double r, Canvas canvas)
        {
            _name = name;
            _ID = id;
            _center = center;
            _r = r;
            _canvas = canvas;
        }

        public string GetName()
        {
            return _name;
        }
        public void SetName(string name)
        {
            _name = name;
        }

        public int GetID()
        {
            return _ID;
        }

        public Point GetCenter()
        {
            return _center;
        }
        public void SetPoint(Point center)
        {
            _center = center;
        }

        public double GetR()
        {
            return _r;
        }
        public void SetR(double r)
        {
            if (r > 1)
                _r = r;
        }

        public Brush GetFillColor()
        {
            return _fillColor;
        }
        public void SetFillColor(byte red, byte green, byte blue)
        {
            _fillColor = new SolidColorBrush(Color.FromRgb(red, green, blue));
        }
        
        public Brush GetStrokeColor()
        {
            return _strokeColor;
        }
        public void SetStrokeColor(byte red, byte green, byte blue)
        {
            _strokeColor = new SolidColorBrush(Color.FromRgb(red, green, blue));
        }

        public void Draw()
        {
            Ellipse ellipse = new Ellipse
            {
                Height = _r * 2,
                Width = _r * 2,
                Fill = _fillColor,
                Stroke = _strokeColor,
                StrokeThickness = _strokeThikness,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,             
                Margin = new Thickness(left: _center.X - _r, top: _center.Y - _r, right: 0, bottom: 0),
            };
            TextBlock text = new TextBlock
            {
                FontSize = 20,
                Text = _ID.ToString(),
                Margin = new Thickness(left: _center.X - (_name.Length) - 10, top: _center.Y - 15, right: 0, bottom: 0),
            };
            Canvas.SetZIndex(ellipse, 0);
            Canvas.SetZIndex(text, 0);
            _canvas.Children.Add(ellipse);
            _canvas.Children.Add(text);
        }

        public override string ToString()
        {
            return string.Format("Name - {0}\nID - {1}\nX - {2}\nY - {3}\nR - {4}\n", _name, _ID, _center.X, _center.Y, _r);
        }

        public bool Equality(Vertex to) => _ID == to.GetID();
    }
}
