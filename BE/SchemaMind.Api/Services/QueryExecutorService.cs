using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
namespace SchemaMind.Api.Services
{
    public class QueryExecutorService
    {
        private readonly DbConnectionService dbConnectionService;

        public QueryExecutorService(DbConnectionService dbConnectionService)
        {
            this.dbConnectionService = dbConnectionService;
        }

        public async Task<IEnumerable<dynamic>> Execute(string sql,string sqlconnection)
        {
            var dbConnection = dbConnectionService.GetConnection(sqlconnection);
            return await dbConnection.QueryAsync(@sql);
        }
    }
}
