using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ClaimsRequiredAttribute : Attribute
    {
        public string Type { get; private set; }
        public string Value { get; private set; }

        public ClaimsRequiredAttribute() { }
        public ClaimsRequiredAttribute(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
