using System;

namespace Domain.Entities
{
    public abstract class AuditEntity
    {
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public DateTime? UpdatedAt { get; set; }
        
    }

    public abstract class AuditEntity<TUser> : AuditEntity
    {        
        public virtual TUser CreatedBy { set; get; }
        public virtual TUser UpdatedBy { set; get; }
    }
}
