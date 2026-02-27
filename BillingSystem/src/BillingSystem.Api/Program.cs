using Asp.Versioning;
using BillingSystem.Api.Configurations.ModelBinding;
using BillingSystem.Api.Configurations.Routing;
using BillingSystem.Api.Filters;
using BillingSystem.Api.Middlewares;
using BillingSystem.Application;
using BillingSystem.Infrastructure;
using BillingSystem.Infrastructure.Data.Migrations;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(
        new KebabCaseParameterTransformer()
    ));

    options.Conventions.Add(new SnakeCaseParameterModelConvention());

    options.ModelMetadataDetailsProviders.Add(new SnakeCasePropertyBindingMetadataProvider());
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Billing System API",
        Version = "v1",
        Description = "API para gestão de clientes, produtos e sincronização de notas fiscais.",
    });
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

await DatabaseMigration.MigrateAsync(app.Services);

app.Run();
