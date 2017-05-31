using System;

namespace UrbanGame.Exceptions
{
    public class InvalidObjectiveCodeException : Exception
    {
        public InvalidObjectiveCodeException() { }

        public InvalidObjectiveCodeException(Exception innerException) : base("", innerException) { }
    }
}
