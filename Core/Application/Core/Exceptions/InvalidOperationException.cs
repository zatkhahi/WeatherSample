using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions
{
    public class InvalidOperationException : Exception
    {
        public InvalidOperationException(string message = null, Exception inner = null)
            : base(message, inner)
        {

        }
    }
}
