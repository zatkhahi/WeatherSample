using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class BaseTrackingEntity<TKey, TEntity> : TrackingAuditEnity
    {
        public TKey EntityId { get; set; }
        public virtual TEntity Entity { get; set; }
    }

    public class TrackingAuditEnity 
    {
        public long CreatedAt { get; set; }
        public int CreatedById { get; set; }
    }

    public static class BaseTrackingEntityTypeHelper
    {
        public static string[] PropertyNames
        {
            get
            {
                return new[] { "EntityId", "CreatedAt", "CreatedById" };
            }
        }
    }
}
