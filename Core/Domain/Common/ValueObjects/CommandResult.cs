using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class CommandResult
    {
        public bool Ok { get; set; } = true;
        public List<CommandError> Warnings { get; set; } = new List<CommandError>();
        public List<CommandError> Errors { get; set; } = new List<CommandError>();

    }
    public class CommandError
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public string StackTrace { get; set; }

        public static CommandError FromException(string message, Exception e = null)
        {
            return new CommandError()
            {
                Title = message ?? e?.Message,
                Details = e?.Message + e?.InnerException?.Message + e?.InnerException?.InnerException?.Message,
                StackTrace = e?.StackTrace
            };
        }
    }
}
