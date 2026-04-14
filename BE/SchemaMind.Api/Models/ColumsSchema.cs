namespace SchemaMind.Api.Models
{
    public class ColumsSchema
    {
        public class ColumnSchema
        {
            public string Name { get; set; }
            public string DataType { get; set; }
            public bool IsPrimaryKey { get; set; }
        }
    }
}
