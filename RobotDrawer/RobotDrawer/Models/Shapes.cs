using System;
using System.Windows.Ink;
using System.Windows.Input;

namespace RobotDrawer.Models
{
    public static class Shapes
    {
        public static StylusPointCollection DrawLine(StylusPoint[] _points)
        {
            return new StylusPointCollection()
            {
                _points[0],
                _points[1]
            };
        }
        public static StylusPointCollection DrawRectangle(StylusPoint[] _points)
        {
            return new StylusPointCollection()
            {
                new StylusPoint(_points[0].X,_points[0].Y),
                new StylusPoint(_points[0].X,_points[1].Y),
                new StylusPoint(_points[1].X, _points[1].Y),
                new StylusPoint(_points[1].X, _points[0].Y),
                new StylusPoint(_points[0].X,_points[0].Y)
            };
        }
        public static StylusPointCollection DrawCircle(StylusPoint[] _points)
        {
            double _xRadius = Math.Abs(_points[1].X - _points[0].X);
            double _yRadius = Math.Abs(_points[1].Y - _points[0].Y);
            double radius = Math.Sqrt(Math.Pow(_xRadius, 2) + Math.Pow(_yRadius, 2));
            StylusPointCollection _circleStrokes = new StylusPointCollection();
            for (int i = 0; i <= 90; i++)
            {
                double rad = Math.PI / 180 * 4.0 * i;
                double x = radius * Math.Cos(rad) + _points[0].X;
                double y = radius * Math.Sin(rad) + _points[0].Y;
                _circleStrokes.Add(new StylusPoint(x, y));
            }
            return _circleStrokes;
        }
        public static Stroke FilterPoints(Stroke InkCanvasStroke)
        {
            if (InkCanvasStroke.StylusPoints.Count > Int32.Parse(Properties.ControllerConfig.Default.robotItemArraySize))
            {
                var newStylusPoints = new StylusPointCollection();
                var accurateStep = (double)InkCanvasStroke.StylusPoints.Count / Int32.Parse(Properties.ControllerConfig.Default.robotItemArraySize);
                int step = (int)Math.Ceiling(accurateStep);
                for(int i = 0; i < InkCanvasStroke.StylusPoints.Count; i += step)
                {
                    newStylusPoints.Add(InkCanvasStroke.StylusPoints[i]);
                }
                return new Stroke(newStylusPoints);
            }
            else return InkCanvasStroke;
        }
    }
}
//public static StylusPointCollection DrawCircle(StylusPoint[] _points)
//{
//    var mouseDownWidth = Math.Abs(_points[1].X - _points[0].X);
//    var mouseDownHeight = Math.Abs(_points[1].Y - _points[0].Y);
//    double radius, xCenter, yCenter;
//    StylusPointCollection _circleStrokes = new StylusPointCollection();
//    if (mouseDownHeight < mouseDownWidth)
//    {
//        radius = 1.0 / 2.0 * mouseDownHeight;
//        xCenter = (_points[0].X > _points[1].X) ? _points[0].X - 1.0 / 2.0 * mouseDownHeight
//            : _points[0].X + 1.0 / 2.0 * mouseDownHeight;
//        yCenter = (_points[0].Y > _points[1].Y) ? _points[0].Y - 1.0 / 2.0 * mouseDownHeight
//            : _points[0].Y + 1.0 / 2.0 * mouseDownHeight;
//    }
//    else
//    {
//        radius = 1.0 / 2.0 * mouseDownWidth;
//        xCenter = (_points[0].X > _points[1].X) ? _points[0].X - 1.0 / 2.0 * mouseDownWidth
//            : _points[0].X + 1.0 / 2.0 * mouseDownWidth;
//        yCenter = (_points[0].Y > _points[1].Y) ? _points[0].Y - 1.0 / 2.0 * mouseDownWidth
//            : _points[0].Y + 1.0 / 2.0 * mouseDownWidth;
//    }
//    for (int i = 0; i <= 20; i++)
//    {
//        double rad = Math.PI / 180 * 18.0 * i;
//        double x = radius * Math.Cos(rad) + xCenter;
//        double y = radius * Math.Sin(rad) + yCenter;
//        _circleStrokes.Add(new StylusPoint(x, y));
//    }
//    return _circleStrokes;
//}