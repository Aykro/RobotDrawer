namespace RobotDrawer.Models
{
    public static class PointConverter
    {
        public static double[,] ToLine(double[,] robotCoordinates)
        {
            var result = new double[robotCoordinates.GetLength(0)-1, 4];
            for(int i = 0; i< robotCoordinates.GetLength(0)-1; i++)
            {
                result[i, 0] = robotCoordinates[i, 0];
                result[i, 1] = robotCoordinates[i, 1];
                result[i, 2] = robotCoordinates[i+1, 0];
                result[i, 3] = robotCoordinates[i+1, 1];
            }
            return result;
        }
    }
}
