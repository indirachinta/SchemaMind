using Azure.Core;
using Microsoft.Extensions.AI;
using SchemaMind.Api.Models;

namespace SchemaMind.Api.Services
{
    public class AIService
    {
        private readonly IChatClient _chatClient;
        private readonly ContextBuilder _contextBuilder;
        private readonly QueryExecutorService _queryExecutorService;
        private readonly DbConnectionService dbConnectionService;

        public AIService(IChatClient chatClient, ContextBuilder contextBuilder, QueryExecutorService queryExecutorService, DbConnectionService dbConnectionService)
        {
            _chatClient = chatClient;
            _contextBuilder = contextBuilder;
            _queryExecutorService = queryExecutorService;
            this.dbConnectionService = dbConnectionService;
        }

        public async Task<QueryAndResults> GenerateSql(string question,string sqlconnection)
        {
            
            var schema = await _contextBuilder.BuildSchemaContext(question, sqlconnection);
            QueryAndResults queryAndResults = new QueryAndResults();
            var prompt = $"""
                          You are a senior SQL developer.
                          Use the following database schema:
                          {schema}

                         Rules:
                         - Use correct JOIN conditions
                         - Prefer explicit JOIN syntax
                         - Use table aliases
                         - Return only SQL

                         User request:
                         {question}

                         Output format: Should be pure sql string directly executable againt database
                        """;

            var response = await _chatClient.GetResponseAsync(prompt);
            int i = 0;
            while (i <= 5)
            {
                string fullResponse = response.ToString();
                string startMarker = "```sql";
                string endMarker = "```";

                // Find positions
                int startIndex = fullResponse.IndexOf(startMarker, StringComparison.OrdinalIgnoreCase);
                int endIndex = fullResponse.IndexOf(endMarker, startIndex + startMarker.Length, StringComparison.OrdinalIgnoreCase);

                if (startIndex == -1 || endIndex == -1)
                {
                    throw new Exception("SQL markers not found in response");
                }

                // Extract SQL between the markers
                string sqlQuery = fullResponse.Substring(
                    startIndex + startMarker.Length,
                    endIndex - (startIndex + startMarker.Length)
                ).Trim();
                try
                {
                    bool isvalid = SqlValidator.IsSafe(sqlQuery);
                    var results = await _queryExecutorService.Execute(sqlQuery,sqlconnection);
                    queryAndResults.query = sqlQuery;
                    queryAndResults.results = results;
                    return queryAndResults;
                }
                catch (Exception ex)
                {
                    prompt = prompt +
                                    $"""
                                there was an error {ex.Message} while executinting the previous response from the model
                                {response}
                                Now please correct and give the right query
                                """;
                    response = await _chatClient.GetResponseAsync(prompt);
                    i++;
                }
            }
            queryAndResults.query = @response.ToString();
            queryAndResults.results = null;
            return queryAndResults;
        }

       

      
        
    }
}
