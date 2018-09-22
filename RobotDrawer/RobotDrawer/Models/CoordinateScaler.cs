using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace RobotDrawer.Models
{
    public static class CoordinateScaler
    {
        public static double[,] ToRobotCoordinates(Stroke _lastStroke,
            double heightSize, double widthSize)
        {
            double[,] _scaledCoordinates = new double[_lastStroke.StylusPoints.Count,2];
            for (int i = 0; i < _lastStroke.StylusPoints.Count; i++)
            {
                _scaledCoordinates[i,0] = Math.Round((420 * _lastStroke.StylusPoints.ElementAt(i).X / widthSize), 0);
                _scaledCoordinates[i,1] = Math.Round((297 * _lastStroke.StylusPoints.ElementAt(i).Y / heightSize), 0);
            }
            return _scaledCoordinates;
        }
    }
}
