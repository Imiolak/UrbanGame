using System;

namespace UrbanGame.Exceptions
{
    public class InvalidClueCodeException : Exception
    {
        public InvalidClueCodeException() { }

        public InvalidClueCodeException(Exception innerException) : base("", innerException) { }
    }
}
