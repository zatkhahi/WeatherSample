using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class JobHandle
    {
        public long JobId { get; set; }
        public int CreatedBy { get; set; }
        public long CreatedAt { get; set; }
        public int Progress { get; set; } = 0;
        public bool IsCompleted => IsFailed || IsSucceeded;
        public bool IsFailed { get; set; } = false;
        public bool IsSucceeded { get; set; } = false;
        public string StatusMessage { get; set; }
        public string FailureReason { get; set; }
        public string StackTrace { get; set; }
        public List<string> Warnings { get; set; } = new List<string>();
        public string Result { get; set; }
    }
}
