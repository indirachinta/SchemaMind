using Microsoft.Extensions.Caching.Memory;
using SchemaMind.Api.Models;
using SchemaMind.Api.Helpers;

namespace SchemaMind.Api.Services
{
    public class SchemaCasheService
    {
        private readonly IMemoryCache memoryCache;
        private readonly SchemaService schemaService;

        public SchemaCasheService(IMemoryCache memoryCache, SchemaService schemaService)
        {
            this.memoryCache = memoryCache;
            this.schemaService = schemaService;
        }

        public async Task<List<TableSchema>> GetTablesSchema(string sqlconnection)
        {
            var sqlkey = CacheKeyHelper.HashConnectionString(sqlconnection);

            var tableSchemas = memoryCache.Get<List<TableSchema>>($"{sqlkey}{"-"}tablesschema");
            if (tableSchemas == null)
            {
                tableSchemas = await schemaService.GetTables(sqlconnection);
                memoryCache.Set($"{sqlkey}{"-"}tablesschema", tableSchemas, TimeSpan.FromMinutes(30));

            }
           
            return tableSchemas;


            
        }

        public async Task<List<ForeignKeySchema>> GetForeignKeys(string sqlconnection)
        {
            var sqlkey = CacheKeyHelper.HashConnectionString(sqlconnection);
            var foreignKeys = memoryCache.Get<List<ForeignKeySchema>>($"{sqlkey}{"-"}foreignKeys");
            if (foreignKeys == null)
            {
                foreignKeys = await schemaService.GetForeignKeys(sqlconnection);
                memoryCache.Set($"{sqlkey}{"-"}foreignKeys", foreignKeys, TimeSpan.FromMinutes(30));

            }

            return foreignKeys;


            
        }


    }
}
