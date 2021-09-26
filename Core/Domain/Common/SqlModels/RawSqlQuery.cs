using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.SqlModels
{
    public class RawSqlQuery
    {
        public string Sql { get; set; }
        public List<SqlQueryParameter> SqlParameters { get; set; } = new List<SqlQueryParameter>();
    }
}
