using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class OperationAttribute : Attribute
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Resource { get; set; }


        public OperationAttribute() { }
        public OperationAttribute(string operation, string title = null, string resource = null)
        {
            if (string.IsNullOrEmpty(operation))
                throw new Exception("Invalid Operation Name");

            Name = operation;
            Resource = resource;
            Title = title;
        }
    }

}
