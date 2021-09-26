using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ScopeOperation
    {
        public string ScopeName { get; set; }        
        public string Operation { get; set; }        
        public string Title { get; set; }

        public virtual ApplicationScope Scope { get; set; }

        public static ScopeOperation Create(string scope) => new ScopeOperation()
        {
            Operation = "create",
            Title = "ایجاد",
            ScopeName = scope
        };
        public static ScopeOperation Update(string scope) => new ScopeOperation()
        {
            Operation = "update",
            Title = "تغییر",
            ScopeName = scope
        };
        public static ScopeOperation Read(string scope) => new ScopeOperation()
        {
            Operation = "read",
            Title = "مشاهده",
            ScopeName = scope
        };
        public static ScopeOperation Delete(string scope) =>  new ScopeOperation()
        {
            Operation = "delete",
            Title = "حذف",
            ScopeName = scope
        };
    }
}
