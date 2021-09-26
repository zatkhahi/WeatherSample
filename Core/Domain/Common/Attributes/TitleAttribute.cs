using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
    public class TitleAttribute : Attribute
    {
        public string Title { get; private set; }
        public TitleAttribute(string title)
        {
            Title = title;
        }
    }
}
