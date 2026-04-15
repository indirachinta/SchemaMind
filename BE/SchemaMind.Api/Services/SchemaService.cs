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
                var parts = table.Split('.');
                var schemaName = parts[0];
                var tableName = parts[1];
                var columns = await conn.QueryAsync<ColumnSchema>(
        @"SELECT COLUMN_NAME as Name,
                 DATA_TYPE as DataType
          FROM INFORMATION_SCHEMA.COLUMNS
          WHERE TABLE_SCHEMA = @schemaName AND TABLE_NAME = @tableName",
        new { schemaName, tableName });
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
                //var sql = "select FK_Name,FromTable,FromColumn,ToTable,ToColumn from ForeignKeys";
                var sql = @"
        SELECT
            fk.name AS FK_Name,
            OBJECT_SCHEMA_NAME(fk.parent_object_id) + '.' + tp.name AS FromTable,
            cp.name AS FromColumn,
            OBJECT_SCHEMA_NAME(fk.referenced_object_id) + '.' + tr.name AS ToTable,
            cr.name AS ToColumn
        FROM sys.foreign_keys fk
        INNER JOIN sys.foreign_key_columns fkc
            ON fk.object_id = fkc.constraint_object_id
        INNER JOIN sys.tables tp
            ON fkc.parent_object_id = tp.object_id
        INNER JOIN sys.columns cp
            ON fkc.parent_object_id = cp.object_id
           AND fkc.parent_column_id = cp.column_id
        INNER JOIN sys.tables tr
            ON fkc.referenced_object_id = tr.object_id
        INNER JOIN sys.columns cr
            ON fkc.referenced_object_id = cr.object_id
           AND fkc.referenced_column_id = cr.column_id
        ORDER BY FromTable, ToTable;
    ";
                var foreignkeys = await conn.QueryAsync<ForeignKeySchema>(@sql);
                return foreignkeys.ToList();
            }
        }
    }
}
