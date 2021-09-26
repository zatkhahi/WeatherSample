using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public string Title { get; private set; }
        public CommandType CommandType { get; private set; }

        public CommandAttribute() { }
        public CommandAttribute(string title, CommandType commandType)
        {
            Title = title;
            CommandType = commandType;
        }

    }
}
