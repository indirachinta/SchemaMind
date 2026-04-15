namespace SchemaMind.Api.Models
{
    public class TableSchema
    {
        public string? Name { get; set; }
        public List<ColumnsSchema.ColumnSchema> Columns { get; set; } = new();
    }
}
