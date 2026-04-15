using Microsoft.Data.SqlClient;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using SchemaMind.Api.Infra;
using SchemaMind.Api.Services;
using System.ClientModel;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
//var aiSection = builder.Configuration.GetSection("AI");
//string apiKey = aiSection["ApiKey"]!;
//string endpoint = aiSection["Endpoint"]!;
//string model = aiSection["Model"]!;
//var options = new OpenAIClientOptions()
//{
//    Endpoint = new Uri(endpoint),

//};

//builder.Services.AddSingleton<IChatClient>(new OpenAIClient(new ApiKeyCredential(apiKey), options).GetChatClient(model).AsIChatClient());

builder.Services.AddAI(builder.Configuration);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SchemaService>();
builder.Services.AddSingleton<AIService>();
builder.Services.AddSingleton<ContextBuilder>();
builder.Services.AddSingleton<SchemaCacheService>();
builder.Services.AddSingleton<SemanticSelectorService>();
builder.Services.AddTransient<QueryExecutorService>();
//builder.Services.AddTransient <IDbConnection>(u=> new SqlConnection(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddTransient<DbConnectionService>();

builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        p => p.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapControllers();


app.Run();

