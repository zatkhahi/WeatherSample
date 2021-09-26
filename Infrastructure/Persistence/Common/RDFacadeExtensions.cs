using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class RDFacadeExtensions
    {
        public static RelationalDataReader ExecuteSqlQuery(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
        {
            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade
                    .GetService<IRawSqlCommandBuilder>()
                    .Build(sql, parameters);

                return rawSqlCommand
                    .RelationalCommand
                    .ExecuteReader(new RelationalCommandParameterObject(
                        databaseFacade.GetService<IRelationalConnection>(),
                        rawSqlCommand.ParameterValues, null, null, null));
            }
        }

        public static async Task<RelationalDataReader> ExecuteSqlQueryAsync(this DatabaseFacade databaseFacade,
                                                             string sql,
                                                             CancellationToken cancellationToken = default(CancellationToken),
                                                             params object[] parameters)
        {

            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade
                    .GetService<IRawSqlCommandBuilder>()
                    .Build(sql, parameters);
                //var parameterValues = parameters.ToDictionary(s => (s as SqlParameter).ParameterName,
                //    s => (s as SqlParameter).SqlValue);
                databaseFacade.SetCommandTimeout(System.TimeSpan.FromMinutes(10));
                return await rawSqlCommand
                    .RelationalCommand
                    .ExecuteReaderAsync(new RelationalCommandParameterObject(
                        databaseFacade.GetService<IRelationalConnection>(),
                        rawSqlCommand.ParameterValues, null, null, null), cancellationToken);
            }
        }
    }
}