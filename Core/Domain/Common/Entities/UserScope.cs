using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class UserScope
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ScopeName { get; set; }
        public string Operation { get; set; }
        public string EntityKey { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
