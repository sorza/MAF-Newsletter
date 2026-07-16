using Newsletter.Core;
using Newsletter.Ai;
using Newsletter.Infra;
using Newsletter.Api.Workers;

var builder = WebApplication.CreateBuilder(args);

Configuration.OpenAi.ApiKey = builder.Configuration.GetValue<string>("OpenAI:ApiKey")
    ?? throw new InvalidOperationException("OpenAI API key is not configured.");

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddAgents();

builder.Services.AddHostedService<NewsletterWorker>();

var app = builder.Build();
Configuration.RootPath = app.Environment.ContentRootPath;

app.MapGet("/", () => "Hello World!");

app.Run();
