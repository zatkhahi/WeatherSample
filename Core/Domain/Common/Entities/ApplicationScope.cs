using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ApplicationScope
    {
        // public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string EntityType { get; set; }
        public string EntityTypeTitle { get; set; }
        public string EntityTypeRequest { get; set; }
        public string EntityDbSetName { get; set; }
        public string EntityKeyProperty { get; set; } = "Id";
        public string EntityNameProperty { get; set; } = "Name";
        public virtual ICollection<ScopeOperation> Operations { get; set; }
        // public virtual ICollection<RoleScope> Scopes { get; set; }
    }
}
