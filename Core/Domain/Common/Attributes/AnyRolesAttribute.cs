using System;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AnyRolesAttribute : Attribute
    {
        public string[] Roles { get; private set; }
        public AnyRolesAttribute() { }
        public AnyRolesAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }
}
