using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class HistoryAuditEntity<TUser>
    {
        public int UpdatedById { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual TUser UpdatedBy { set; get; }
    }

    public class BaseHistoryEntity<TKey> : HistoryAuditEntity<ApplicationUser>
    {
        [Key]
        public TKey Id { get; set; }        
    }

}
