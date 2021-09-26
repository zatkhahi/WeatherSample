using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Infrastructure;

namespace Common.Presentation.Models
{
    public class BadRequest
    {
        public string Message { get; set; }
        public string TechnicalMessage { get; set; }
        public string StackTrace { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }

        public BadRequest(string msg = null) { Message = msg ?? "خطای نامشخص رویداده است"; }
        public BadRequest(Exception ex, string msg = null)
        {
            Message = msg ?? ex.Message ?? "خطای نامشخص رویداده است";
            TechnicalMessage = ex.FullMessage(true);
            StackTrace = ex.StackTrace;
            if (ex is Application.Exceptions.ValidationException)
            {
                Errors = (ex as Application.Exceptions.ValidationException).Failures;
            }
        }
    }
}
