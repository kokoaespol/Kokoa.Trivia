using FastEndpoints;
using Kokoa.Trivia.Api.Database;
using Kokoa.Trivia.Api.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddFastEndpoints();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddDbContext<TriviaDbContext>();

builder.Services.AddSingleton<TriviaQuestionMapper>();

var app = builder.Build();

app.UseFastEndpoints();

app.Run();