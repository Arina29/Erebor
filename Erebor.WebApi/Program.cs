using Erebor.Application.Extensions;
using Erebor.Application.Services;
using Erebor.Data;
using Erebor.Data.Extensions;
using Erebor.Domain.Models;
using Erebor.WebApi;
using Erebor.WebApi.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Host.UseSerilog((context, loggerConfig) =>
{
    var seqLink = config.GetSection("seqLink").Value;
    loggerConfig
        .WriteTo.Console()
        .Enrich.WithExceptionDetails()
        .WriteTo.Seq(seqLink ?? throw new NullReferenceException("Seq link is null"));
});

builder.Services.AddControllers();

builder.Services.AddDatabaseContext(
    builder.Configuration.GetConnectionString("EreborContext") ??
    throw new NullReferenceException("Connection string is null"));

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:6001";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("erebor", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1" });
        options.OperationFilter<AuthorizeCheckOperationFilter>();
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri("https://localhost:6001/connect/authorize"),
                    TokenUrl = new Uri("https://localhost:6001/connect/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        {"api1", "Erebor API - full access"}
                    }
                }
            }
        });
    }
);
builder.Services.AddCors();

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("X-Version"));
});

builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddAntiforgery(options =>
{
    options.SuppressXFrameOptionsHeader = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

        options.OAuthClientId("erebor_api_swagger");
        options.OAuthAppName("Erebor API - Swagger");
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseMiddleware<RequestMiddleware>();

app.MapControllers()
    .RequireAuthorization("erebor");
app.UseAuthentication();
app.UseAuthorization();

app.Run();