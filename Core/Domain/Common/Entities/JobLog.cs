using GTO.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class JobLog
    {
        public long Id { get; set; }
        public JobLogLevel Level { get; set; }
        public string Message { get; set; }
        public long JobId { get; set; }
        public virtual Job Job { get; set; }
    }
}
