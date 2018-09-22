using System;

namespace RobotDrawer.Models.Exceptions
{
    public class InvalidOPCItemIDException : Exception
    {
        public InvalidOPCItemIDException()
        {

        }
        public InvalidOPCItemIDException(string message)
            : base(message)
        {
        }

        public InvalidOPCItemIDException(string message, Exception inner)
          : base(message, inner)
        {
        }
    }
}
