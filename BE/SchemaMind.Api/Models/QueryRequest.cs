using System.ComponentModel.DataAnnotations;

namespace SchemaMind.Api.Models
{
    public class QueryRequest
    {
        [Required]
        public string? Question { get; set; }

        [Required]
        public string? ConnectionString { get; set; }
    }
}
