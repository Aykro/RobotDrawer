using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var mouseDownWidth = Math.Abs(_points[1].X - _points[0].X);
            var mouseDownHeight = Math.Abs(_points[1].Y - _points[0].Y);
            double radius, xCenter, yCenter;
            StylusPointCollection _circleStrokes = new StylusPointCollection();
            if (mouseDownHeight < mouseDownWidth)
            {
                radius = 1.0 / 2.0 * mouseDownHeight;
                xCenter = (_points[0].X > _points[1].X) ? _points[0].X - 1.0 / 2.0 * mouseDownHeight
                    : _points[0].X + 1.0 / 2.0 * mouseDownHeight;
                yCenter = (_points[0].Y > _points[1].Y) ? _points[0].Y - 1.0 / 2.0 * mouseDownHeight
                    : _points[0].Y + 1.0 / 2.0 * mouseDownHeight;
            }
            else
            {
                radius = 1.0 / 2.0 * mouseDownWidth;
                xCenter = (_points[0].X > _points[1].X) ? _points[0].X - 1.0 / 2.0 * mouseDownWidth
                    : _points[0].X + 1.0 / 2.0 * mouseDownWidth;
                yCenter = (_points[0].Y > _points[1].Y) ? _points[0].Y - 1.0 / 2.0 * mouseDownWidth
                    : _points[0].Y + 1.0 / 2.0 * mouseDownWidth;
            }
            for (int i = 0; i <= 20; i++)
            {
                double rad = Math.PI / 180 * 18.0 * i;
                double x = radius * Math.Cos(rad) + xCenter;
                double y = radius * Math.Sin(rad) + yCenter;
                _circleStrokes.Add(new StylusPoint(x, y));
            }
            return _circleStrokes;
        }
    }
}
