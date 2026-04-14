using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

namespace SchemaMind.Api.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAI(this IServiceCollection services,IConfiguration config)
         {
            var aiconfigsection = config.GetSection("AI");
            string apiKey = aiconfigsection["ApiKey"]!;
            string endpoint = aiconfigsection["Endpoint"]!;
            string model = aiconfigsection["Model"]!;

            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri(endpoint)
            };

            var openAiClient = new OpenAIClient(new ApiKeyCredential(apiKey),
            options);

            services.AddSingleton<IChatClient>(
            openAiClient
                .GetChatClient(model)
                .AsIChatClient());

            return services;
        }
    }
}
