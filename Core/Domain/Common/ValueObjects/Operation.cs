using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class Operation
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string ResourceName { get; set; }
        // public virtual Resource Resource { get; set; }

        public static Operation Create(string scope)
        {
            if (!scope.Contains(":"))
                return new Operation()
                {
                    ResourceName = scope
                };
            var rp = scope.Split(':');
            if (rp.Length != 2)
                throw new Exception($"Invalid Scope: {scope}");
            return new Operation()
            {
                Name = rp[1],
                ResourceName = rp[0]
            };
        }


    }
}
