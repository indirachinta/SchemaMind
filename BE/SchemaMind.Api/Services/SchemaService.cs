using Dapper;
using Microsoft.Data.SqlClient;
using SchemaMind.Api.Models;
using System.Data;
using static SchemaMind.Api.Models.ColumnsSchema;

namespace SchemaMind.Api.Services
{
    public class SchemaService
    {
        private readonly DbConnectionService dbConnectionService;

        public SchemaService(DbConnectionService dbConnectionService)
        {
            this.dbConnectionService = dbConnectionService;
        }

        public async Task<List<TableSchema>> GetTables(string sqlconnection)
        {
            using var conn = dbConnectionService.GetConnection(sqlconnection);

            var tables = await conn.QueryAsync<string>(
                @"SELECT TABLE_SCHEMA +'.'+ TABLE_NAME as TableName
              FROM INFORMATION_SCHEMA.TABLES
              WHERE TABLE_TYPE='BASE TABLE'");

            var result = new List<TableSchema>();

            foreach (var table in tables)
            {
                var columns = await conn.QueryAsync<ColumnSchema>(
                    @"SELECT COLUMN_NAME as Name,
                         DATA_TYPE as DataType
                  FROM INFORMATION_SCHEMA.COLUMNS
                  WHERE TABLE_NAME=@table",
                    new { table });

                result.Add(new TableSchema
                {
                    Name = table,
                    Columns = columns.ToList()
                });
            }

            return result;
        }


        public async Task<List<ForeignKeySchema>> GetForeignKeys(string sqlconnection)
        {
            {
                using var conn = dbConnectionService.GetConnection(sqlconnection);
                var sql = "select FK_Name,FromTable,FromColumn,ToTable,ToColumn from ForeignKeys";
                var foreignkeys = await conn.QueryAsync<ForeignKeySchema>(@sql);
                return foreignkeys.ToList();
            }
        }
    }
}
