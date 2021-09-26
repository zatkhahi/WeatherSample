using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    //[Resource("roles")]
    public class Resource
    {
        public string Name { get; set; }
        public string Title { get; set; }

        public List<Operation> Operaions { get; set; } = new List<Operation>();

        public Resource() { }

    }
}
