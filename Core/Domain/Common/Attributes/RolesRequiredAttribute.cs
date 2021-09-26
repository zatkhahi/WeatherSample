using System;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RolesRequiredAttribute : Attribute
    {
        public string[] Roles { get; private set; }
        public RolesRequiredAttribute() { }
        public RolesRequiredAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }
}
