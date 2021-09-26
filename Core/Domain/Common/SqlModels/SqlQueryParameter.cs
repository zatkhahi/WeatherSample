using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.SqlModels
{
    public class SqlQueryParameter
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }
        // public QueryParameterValue ParameterValue { get; set; }
    }
}
