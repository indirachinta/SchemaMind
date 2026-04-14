using SchemaMind.Api.Services;

namespace SchemaMind.Api.Models
{
    public class ForeignKeySchema
    {
        public string FromTable { get; set; }
        public string FromColumn { get; set; }

        public string ToTable { get; set; }
        public string ToColumn { get; set; }
    }
}
