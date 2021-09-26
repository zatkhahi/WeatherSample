using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class RoleScope
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string ScopeName { get; set; }
        public string Operation { get; set; }
        public string EntityKey { get; set; }

        public virtual ApplicationRole Role { get; set; }
        // public virtual ApplicationScope Scope { get; set; }

    }
}
