using GTO.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Job : BaseEntity<long>
    {
        public string RecentMessage { get; set; }
        public int? Progress { get; set; }
        public JobStatus Status { get; set; }

        public virtual ICollection<JobLog> JobLogs { get; set; }
    }

}
