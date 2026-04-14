namespace SchemaMind.Api.Services
{
    public static class SqlValidator
    {
        public static List<String> ForbiddenWords = new List<string>()
        {
            "DROP",
            "DELETE",
            "UPDATE",
            "INSERT",
            "ALTER",
            "TRUNCATE"
        };

        public static bool IsSafe(string query)
        {

            foreach (var word in ForbiddenWords)
            {
                if (query.Contains(word))
                    throw new Exception("The sql query given by the chat assitant is not safe and containns db altering query parts");
            }
            return true;
        }
    }
}
