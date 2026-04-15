using Azure.Core;
using Microsoft.Extensions.AI;
using SchemaMind.Api.Models;
using Microsoft.Extensions.Logging;

namespace SchemaMind.Api.Services
{
    public class AIService
    {
        private readonly IChatClient _chatClient;
        private readonly ContextBuilder _contextBuilder;
        private readonly QueryExecutorService _queryExecutorService;
        private readonly DbConnectionService _dbConnectionService;
        private readonly ILogger<AIService> _logger;

        public AIService(IChatClient chatClient, ContextBuilder contextBuilder, QueryExecutorService queryExecutorService, DbConnectionService dbConnectionService, ILogger<AIService> logger)
        {
            _chatClient = chatClient;
            _contextBuilder = contextBuilder;
            _queryExecutorService = queryExecutorService;
            _dbConnectionService = dbConnectionService;
            _logger = logger;
        }

        public async Task<QueryAndResults> GenerateSql(string question,string sqlconnection)
        {
            _logger.LogInformation("Received question: {Question}", question);
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
                _logger.LogInformation("Attempt {Attempt} for SQL generation", i + 1);
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
                    bool isValid = SqlValidator.IsSafe(sqlQuery);
                    if (!isValid)
                    {
                        throw new Exception("Unsafe SQL detected.");
                    }
                    var results = await _queryExecutorService.Execute(sqlQuery,sqlconnection);
                    queryAndResults.query = sqlQuery;
                    queryAndResults.results = results;
                    return queryAndResults;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing SQL on attempt {Attempt}", i + 1);
                    prompt = prompt +
                                    $"""
                                there was an error {ex.Message} while executinting the previous response from the model
                                {response}

                                You have only 5 attempts to return the correct query ,so carefully examine the schema for tables, columns,relationships and this is
                                is attempt no: {i+1} 

                                Try to get all columns like * from main table if you cross 3 attempts

                                Now please correct and give the right query

                                Output format: Should be pure sql string directly executable againt database
                                """;
                    response = await _chatClient.GetResponseAsync(prompt);
                    i++;
                }
            }
            queryAndResults.query = @response.ToString();
            queryAndResults.results = null;
            _logger.LogInformation("SQL executed successfully on attempt {Attempt}", i + 1);
            return queryAndResults;
        }

       

      
        
    }
}
