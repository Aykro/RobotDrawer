using System;
using System.Linq;
using System.Windows.Ink;

namespace RobotDrawer.Models
{
    public static class CoordinateScaler
    {
        public static double[,] ToRobotCoordinates(Stroke lastStroke,
            double heightSize, double widthSize)
        {
            double[,] _scaledCoordinates = new double[lastStroke.StylusPoints.Count,2];
            for (int i = 0; i < lastStroke.StylusPoints.Count; i++)
            {
                _scaledCoordinates[i,0] = Math.Round((600 * lastStroke.StylusPoints.ElementAt(i).X / widthSize), 0);
                _scaledCoordinates[i,1] = Math.Round((300 * lastStroke.StylusPoints.ElementAt(i).Y / heightSize), 0);
            }
            return _scaledCoordinates;
        }
    }
}
