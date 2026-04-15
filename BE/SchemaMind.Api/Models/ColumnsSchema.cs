namespace SchemaMind.Api.Models
{
    public class ColumnsSchema
    {
        public class ColumnSchema
        {
            public string Name { get; set; }
            public string DataType { get; set; }
            public bool IsPrimaryKey { get; set; }
        }
    }
}
