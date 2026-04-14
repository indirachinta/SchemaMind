using SchemaMind.Api.Models;

namespace SchemaMind.Api.Services
{
    public class SemanticSelectorService
    {
        public List<TableSchema> SelectRelevantTables(string question, List<TableSchema> tables)
        {
            var results = new List<TableSchema>();

            foreach (var table in tables)
            {
                if (question.Contains(table.Name, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(table);
                }
                else
                {
                    foreach (var column in table.Columns)
                    {
                        if (question.Contains(column.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            results.Add(table);
                            break;
                        }
                    }
                }
            }

            if (results.Count == 0)
                return null; // fallback

            return results;
        }
    }
}