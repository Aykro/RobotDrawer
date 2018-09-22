﻿using System.Windows.Media;

namespace RobotDrawer.Models
{
    public class ObjectToDraw
    {
        public double[,] Lines { get; private set; }
        public Color Colour { get; private set; }
        public bool EraseAll { get; private set; }
        public ObjectToDraw(double[,] Lines, Color Colour)
        {
            this.Lines = Lines;
            this.Colour = Colour;
            this.EraseAll = false;
        }
        public ObjectToDraw(double[,] Lines, Color Colour, bool EraseAll)
        {
            this.Lines = Lines;
            this.Colour = Colour;
            this.EraseAll = EraseAll;
        }
    }
}
