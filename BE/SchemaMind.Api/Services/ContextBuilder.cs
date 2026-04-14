using SchemaMind.Api.Models;
using System.Text;

namespace SchemaMind.Api.Services
{
    public class ContextBuilder
    {
        private readonly SchemaCasheService schemaCasheService;
        private readonly SemanticSelectorService semanticSelectorService;

        public ContextBuilder(SchemaCasheService schemaCasheService, SemanticSelectorService semanticSelectorService)
        {
            this.schemaCasheService = schemaCasheService;
            this.semanticSelectorService = semanticSelectorService;
        }

        public async Task<string> BuildSchemaContext(string question, string sqlconnection)
        {
            List<TableSchema> tables = await schemaCasheService.GetTablesSchema(sqlconnection);
            List<ForeignKeySchema> fks = await schemaCasheService.GetForeignKeys(sqlconnection);

            tables = semanticSelectorService.SelectRelevantTables(question, tables) ?? tables;
            var sb = new StringBuilder();
            sb.AppendLine("The Table Schema is :");
            foreach (var table in tables)
            {
                sb.AppendLine($"Table: {table.Name}");

                foreach (var column in table.Columns)
                {
                    sb.AppendLine($"  - {column.Name} ({column.DataType})");
                }

                sb.AppendLine();
            }
            sb.AppendLine("The Relationships are");
        
            foreach (var fk in fks)
            {
                sb.AppendLine(
                   $"{fk.FromTable} JOIN {fk.ToTable} ON {fk.FromTable}.{fk.FromColumn} = {fk.ToTable}.{fk.ToColumn}");
            }

            return sb.ToString();
        }
    }
}
