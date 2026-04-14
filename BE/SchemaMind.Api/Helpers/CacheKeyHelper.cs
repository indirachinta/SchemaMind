using System.Security.Cryptography;
using System.Text;

namespace SchemaMind.Api.Helpers
{
    public static class CacheKeyHelper
    {
        public static string HashConnectionString(string connectionString)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(connectionString);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
