using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Presentation.Models
{
    public class UnauthorizedRequest
    {
        public string Message { get; set; }

        public UnauthorizedRequest(string msg = "")
        {
            Message = msg;
        }
    }
}
