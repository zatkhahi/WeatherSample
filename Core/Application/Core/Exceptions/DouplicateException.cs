using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions
{
    [Serializable]
    public class DouplicateException : Exception
    {
        public DouplicateException() { }
        public DouplicateException(string message) : base(message) { }
        public DouplicateException(string message, Exception inner) : base(message, inner) { }
        protected DouplicateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
