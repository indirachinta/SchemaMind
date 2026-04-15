using SchemaMind.Api.Infra;
using SchemaMind.Api.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAI(builder.Configuration);


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

