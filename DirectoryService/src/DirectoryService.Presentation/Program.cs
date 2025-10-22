using DirectoryService.Application;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Positions;
using DirectoryService.Infrastructure.Postgres;
using DirectoryService.Infrastructure.Postgres.Repositories;
using DirectoryService.Presentation.Middlewares;
using FluentValidation;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.WithExceptionDetails()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") 
                 ?? throw new ArgumentNullException("Seq"))
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DB Context
builder.Services.AddScoped<DirectoryServiceDbContext>(_ => 
    new DirectoryServiceDbContext(builder.Configuration.GetConnectionString("DirectoryServiceDb")!));

// Repositories
builder.Services.AddScoped<ILocationRepository, LocationsRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentsRepository>();
builder.Services.AddScoped<IPositionRepository, PositionsRepository>();

// Handlers
builder.Services.AddScoped<ICommandHandler<Guid, CreateLocationCommand>, CreateLocationHandler>();
builder.Services.AddScoped<ICommandHandler<Guid, CreateDepartmentCommand>, CreateDepartmentHandler>();
builder.Services.AddScoped<ICommandHandler<Guid, CreatePositionCommand>, CreatePositionHandler>();

// Logger
builder.Services.AddSerilog();

// Validator
builder.Services.AddValidatorsFromAssembly(typeof(CreateLocationCommandValidator).Assembly, ServiceLifetime.Scoped);
builder.Services.AddValidatorsFromAssembly(typeof(CreateDepartmentCommandValidator).Assembly, ServiceLifetime.Scoped);
builder.Services.AddValidatorsFromAssembly(typeof(CreatePositionCommandValidator).Assembly, ServiceLifetime.Scoped);

var app = builder.Build();

// Middleware
app.UseExceptionMiddleware();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DirectoryService"));
}

app.MapControllers();

app.Run();