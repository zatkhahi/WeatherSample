using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Presentation.Models
{
    public class NotFound
    {
        public string Message { get; set; }

        public NotFound(string msg = "")
        {
            Message = msg;
        }
    }
}
