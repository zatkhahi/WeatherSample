using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions
{
    public class NotFoundException : Exception
    {
        //public NotFoundException(string name, object key)
        //    : base($"شیء \"{name}\" ({key}) پیدا نشد.")
        //{
        //}
        public NotFoundException(string name, params object[] keys)
            : base($"شیء \"{name}\" ({string.Join(',', keys)}) پیدا نشد.")
        {
        }
    }
}
