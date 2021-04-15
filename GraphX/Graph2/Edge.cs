using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;

namespace Graph2
{
    public class Edge
    {
        private Vertex _from;
        private Vertex _to;
        private int _weight = 1;
        private int _ID;
        Canvas _canvas;
        private Brush color = Brushes.Black;
        private int _r = 25;
        private double divideEdge = 1.5;

        public Edge(Vertex from, Vertex to, int id, Canvas canvas, int weight = 1)
        {
            _from = from;
            _to = to;
            _ID = id;
            _weight = weight;
            _canvas = canvas;
        }

        #region Get\Set
        public Vertex GetFrom() { return _from; }

        public Vertex GetTo() { return _to; }

        public int GetWeight() { return _weight; }
        public void SetWeight(int w) { _weight = w; }

        public int GetID() => _ID;
        public Canvas GetCanvas() => _canvas;
        public void SetColor(Brush br) => color = br;
        #endregion

        public void Draw(bool orient, bool weighted)
        {
            if (orient)
            {
                if (_from.Equality(_to))
                    DrawReturnArrowEdge(_r, weighted);
                else
                    DrawArrowEdge(weighted);
            }
            else
            {

                if (_from.Equality(_to))
                {
                    DrawReturnLineEdge(_r, weighted);
                }
                else
                {
                    DrawLineEdge(weighted);
                }
            }
        }

        private void DrawArrowEdge(bool weighted)
        {
            double len1 = 10;
            double len2 = 5;
            double coef = 3;
            double X1 = _from.GetCenter().X;
            double Y1 = _from.GetCenter().Y;
            double X2 = _to.GetCenter().X;
            double Y2 = _to.GetCenter().Y;
            // Длина отрезка
            double d = Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y2 - Y1, 2));
            // Координаты вектора
            double X = X2 - X1;
            double Y = Y2 - Y1;
            //  
            double X3 = (X1 + X2 * coef) / (1 + coef);
            double Y3 = (Y1 + Y2 * coef) / (1 + coef);
            //
            double X4 = X3 - (X / d) * len1;
            double Y4 = Y3 - (Y / d) * len1;
            //
            double Xp = Y2 - Y1;
            double Yp = X1 - X2;
            //
            double X5 = X4 + (Xp / d) * len2;
            double Y5 = Y4 + (Yp / d) * len2;
            double X6 = X4 - (Xp / d) * len2;
            double Y6 = Y4 - (Yp / d) * len2;
            //
            Line mainLine = new Line
            {
                X1 = X1,
                Y1 = Y1,
                X2 = X2,
                Y2 = Y2,
                Stroke = color,
                StrokeThickness = 3,
            };
            Line leftLine = new Line
            {
                X1 = X5,
                Y1 = Y5,
                X2 = X3,
                Y2 = Y3,
                Stroke = color,
                StrokeThickness = 3,
            };
            Line rightLine = new Line
            {
                X1 = X6,
                Y1 = Y6,
                X2 = X3,
                Y2 = Y3,
                Stroke = color,
                StrokeThickness = 3,
            };
            Canvas.SetZIndex(mainLine, -3);
            Canvas.SetZIndex(rightLine, -3);
            Canvas.SetZIndex(leftLine, -3);

            if (weighted)
            {
                double xc = (X1 + divideEdge * X2) / (1.0 + divideEdge) - 15;
                double yc = (Y1 + divideEdge * Y2) / (1.0 + divideEdge) - 15;

                Ellipse el = new Ellipse
                {
                    Height = 30,
                    Width = 30,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(left: xc, top: yc, right: 0, bottom: 0),
                };
                TextBlock text = new TextBlock
                {
                    FontSize = 15,
                    Text = _weight.ToString(),
                    Margin = new Thickness(left:xc - _weight.ToString().Length*10/2+18, top: yc + 5, right: 0, bottom: 0),
                };
                Canvas.SetZIndex(el, -2);
                Canvas.SetZIndex(text, -2);
                _canvas.Children.Add(el);
                _canvas.Children.Add(text);
            }

            _canvas.Children.Add(mainLine);
            _canvas.Children.Add(leftLine);
            _canvas.Children.Add(rightLine);
        }
        private void DrawLineEdge(bool weighted)
        {
            double X1 = _from.GetCenter().X;
            double Y1 = _from.GetCenter().Y;
            double X2 = _to.GetCenter().X;
            double Y2 = _to.GetCenter().Y;
            
            Line mainLine = new Line
            {
                X1 = X1,
                Y1 = Y1,
                X2 = X2,
                Y2 = Y2,
                Stroke = color,
                StrokeThickness = 3,
            };

            if (weighted)
            {
                double xc = (X1 + divideEdge * X2) / (1.0 + divideEdge) - 15;
                double yc = (Y1 + divideEdge * Y2) / (1.0 + divideEdge) - 15;

                Ellipse el = new Ellipse
                {
                    Height = 30,
                    Width = 30,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(left: xc, top: yc, right: 0, bottom: 0),
                };
                TextBlock text = new TextBlock
                {
                    FontSize = 15,
                    Text = _weight.ToString(),
                    Margin = new Thickness(left: xc - _weight.ToString().Length * 10 / 2 + 18, top: yc + 5, right: 0, bottom: 0),
                };
                Canvas.SetZIndex(el, -2);
                Canvas.SetZIndex(text, -2);
                _canvas.Children.Add(el);
                _canvas.Children.Add(text);
            }

            Panel.SetZIndex(mainLine, -3);
            _canvas.Children.Add(mainLine);

        }

        private void DrawReturnArrowEdge(double r, bool weighted)
        {
            double X1 = _canvas.ActualWidth/2;
            double Y1 = _canvas.ActualHeight/2;
            double X2 = _to.GetCenter().X;
            double Y2 = _to.GetCenter().Y;
            // Просчёт координат центра фигуры
            double X = X2 - X1;
            double Y = Y2 - Y1;
            double length = Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y2 - Y1, 2));
            double mn = length / r;
            double xCenter = (X2 * (1 + mn) - X1) * (1 / mn);
            double yCenter = (Y2 * (1 + mn) - Y1) * (1 / mn);

            // Конструирование фигуры
            Path myPath = new Path
            {
                Stroke = color,
                StrokeThickness = 2
            };
            LineGeometry leftLine = new LineGeometry
            {
                StartPoint = new Point(xCenter + r, yCenter),
                EndPoint = new Point(xCenter + r - 7, yCenter + 10)
            };
            LineGeometry rightLine = new LineGeometry
            {
                StartPoint = new Point(xCenter + r, yCenter),
                EndPoint = new Point(xCenter + r + 5, yCenter + 10)
            };
            EllipseGeometry ellipse = new EllipseGeometry
            {
                RadiusX = r,
                RadiusY = r,
                Center = new Point(xCenter, yCenter)
            };
            GeometryGroup myFigure = new GeometryGroup();

            myFigure.Children.Add(leftLine);
            myFigure.Children.Add(rightLine);
            myFigure.Children.Add(ellipse);

            myPath.Data = myFigure;

            // Вычисление угла поворота фигуры относительно её начального положения
            double angleRotate = Math.Acos(X / Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2))) * 180 / Math.PI;
            if (Y2 - Y1 < 0) { angleRotate = 360 - angleRotate; }
            // Сборка составной фигуры 
            RotateTransform myRTr = new RotateTransform(angleRotate, xCenter, yCenter);
            myPath.RenderTransform = myRTr;

            Canvas.SetZIndex(myPath, -1);
            _canvas.Children.Add(myPath);

        }
        private void DrawReturnLineEdge(double r, bool weighted)
        {
            double X1 = _canvas.ActualWidth / 2;
            double Y1 = _canvas.ActualHeight / 2;
            double X2 = _to.GetCenter().X;
            double Y2 = _to.GetCenter().Y;
            // Просчёт координат центра фигуры
            double X = X2 - X1;
            double Y = Y2 - Y1;
            double length = Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y2 - Y1, 2));
            double mn = length / r;
            double xCenter = (X2 * (1 + mn) - X1) * (1 / mn);
            double yCenter = (Y2 * (1 + mn) - Y1) * (1 / mn);

            // Конструирование фигуры
            Path myPath = new Path
            {
                Stroke = color,
                StrokeThickness = 2,
            };
            EllipseGeometry ellipse = new EllipseGeometry
            {
                RadiusX = r,
                RadiusY = r,
                Center = new Point(xCenter, yCenter)
            };
            GeometryGroup myFigure = new GeometryGroup();
            myFigure.Children.Add(ellipse);

            myPath.Data = myFigure;

            /*if (weighted)
            {
                var coord = SearchNewCoord(_from.GetCenter().X, _from.GetCenter().Y, _from.GetR());
                Console.WriteLine($"X - {coord.x}, Y - {coord.y}");

                Ellipse el = new Ellipse
                {
                    Height = 30,
                    Width = 30,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    Margin = new Thickness(left: coord.x, top: coord.y, right: 0, bottom: 0),
                };
                TextBlock tb = new TextBlock
                {
                    Text = _weight.ToString(),
                    Margin = new Thickness(left: coord.x + _weight.ToString().Length * 5 / 2, top: coord.y + 5, right: 0, bottom: 0)
                };

                Canvas.SetZIndex(el, 3);
                Canvas.SetZIndex(tb, 3);
                _canvas.Children.Add(el);
                _canvas.Children.Add(tb);
            }*/

            Canvas.SetZIndex(myPath, -1);
            _canvas.Children.Add(myPath);

        }

        public bool Equality(Edge to) => _from.Equality(to.GetFrom()) && _to.Equality(to.GetTo());

        public override string ToString()
        {
            return String.Format("ID Edge - {0}: ID FromVertex - {1}, ID ToVertex - {2}", _ID, _from.GetID(), _to.GetID());
        }

    }
}
