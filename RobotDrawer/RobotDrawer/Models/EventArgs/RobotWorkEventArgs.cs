using System;

namespace RobotDrawer.Models
{
    public class RobotWorkEventArgs: EventArgs
    {
        public RobotWorkEventArgs(ObjectToDraw objectToDraw)
        {
            ObjectToDraw = objectToDraw;
        }
        public ObjectToDraw ObjectToDraw { get; set; }
    }
}
