using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ResourceAttribute : Attribute
    {
        public string Name { get; set; }
        public string Title { get; set; }


        public ResourceAttribute() { }
        public ResourceAttribute(string name, string title = null)
        {
            Name = name;
            Title = title;
        }
    }
}
