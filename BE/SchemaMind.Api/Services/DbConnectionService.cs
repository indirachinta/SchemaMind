using System.Data;
using Microsoft.Data.SqlClient;

namespace SchemaMind.Api.Services
{
    public class DbConnectionService 
    {
             

        public IDbConnection GetConnection(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

       
    }
}