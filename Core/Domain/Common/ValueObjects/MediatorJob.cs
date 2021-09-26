using System;
using System.Security.Principal;

namespace Domain.ValueObjects
{
    public class MediatorJob
    {
        public object Request { get; set; }
        public Type ResponseType { get; set; }
        public IPrincipal RunAs { get; set; }
        public JobHandle JobHandle { get; set; }
    }
}
