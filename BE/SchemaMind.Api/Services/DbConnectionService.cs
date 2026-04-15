using System.Data;
using Microsoft.Data.SqlClient;

namespace SchemaMind.Api.Services
{
    public class DbConnectionService 
    {
        private IDbConnection? connection;

        

        public IDbConnection GetConnection(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}