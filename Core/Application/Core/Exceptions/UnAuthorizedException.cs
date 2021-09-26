using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string message = null, Exception innerException = null)
            : base(message, innerException)
        {

        }
    }
}
