using System;
using System.Collections.Generic;
using System.Text;

namespace GTO.Domain.Model.Enums
{
    public enum JobStatus : byte
    {
        InProgress = 1,
        Succeeded = 2,
        Failed = 3,
        Cancelled = 4
    }
}
