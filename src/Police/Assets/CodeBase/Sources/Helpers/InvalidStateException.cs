using System;

namespace Helpers
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException(string message) : base(message)
        {
            
        }
    }
}