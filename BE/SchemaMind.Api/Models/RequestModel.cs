using System.ComponentModel.DataAnnotations;

namespace SchemaMind.Api.Models
{
    public class RequestModel
    {
        [Required]
        public string? question { get; set; }

        [Required]
        public string? connection { get; set; }
    }
}
