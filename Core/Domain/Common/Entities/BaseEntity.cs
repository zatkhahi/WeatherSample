using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class BaseEntity<T> : AuditEntity<ApplicationUser>
    {
        [Key]
        public T Id { set; get; }
    }

    public class BaseEntity<T, TUser> : AuditEntity<TUser>
    {
        [Key]
        public T Id { set; get; }
    }

    //public class BaseHistoryEntity<T>
    //{
    //    public T Id { get; set; }
    //    public long RevisionId { get; set; }        
    //    public int? UpdatedById { get; set; }
    //    public DateTime? UpdatedAt { get; set; }
    //    public RevisionType RevisionType { get; set; } = RevisionType.Update;

    //    public virtual ApplicationUser UpdatedBy { get; set; }
    //}

    public static class BaseEntityTypeHelper
    {
        public static string[] PropertyNames
        {
            get
            {
                return new[] { "Id", "CreatedAt", "CreatedById", "UpdatedAt", "UpdatedById" };
            }
        }
    }
}
