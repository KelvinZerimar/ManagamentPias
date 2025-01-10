using Asp.Versioning;
using ManagamentPias.App.Common.Services;
using ManagamentPias.Infra.Shared.Authentication.Settings;
using ManagamentPias.Infra.Shared.Services;
using ManagamentPias.WebApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace ManagamentPias.WebApi.Extensions;

public static class ServiceExtensions
{

    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Clean Architecture - ManagamentPias.WebApi",
                Description = "This Api will be responsible for overall data distribution and authorization.",
                Contact = new OpenApiContact
                {
                    Name = "Kramirez",
                    Email = "kramirez@outlook.com",
                    Url = new Uri("https://kramirez.com"),
                }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
        });
    }

    public static void AddControllersExtension(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            })
            ;
    }

    //Configure CORS to allow any origin, header and method. 
    //Change the CORS policy based on your requirements.
    //More info see: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.0

    public static void AddCorsExtension(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });
    }


    //public static void AddVersionedApiExplorerExtension(this IServiceCollection services)
    //{
    //    services.AddVersionedApiExplorer(o =>
    //    {
    //        o.GroupNameFormat = "'v'VVV";
    //        o.SubstituteApiVersionInUrl = true;
    //    });
    //}
    public static void AddApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            // Specify the default API Version as 1.0
            config.DefaultApiVersion = new ApiVersion(1, 0);
            // If the client hasn't specified the API version in the request, use the default API version number 
            config.AssumeDefaultVersionWhenUnspecified = true;
            // Advertise the API versions supported for the particular endpoint
            config.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public static void AddJWTAuthentication(this IServiceCollection services, AuthenticationSettings authSettings)
    {
        // Prevents the mapping of sub claim into archaic SOAP NameIdentifier.
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
#if DEBUG
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = ctx =>
                    {
                        // Break here to debug JWT authentication.
                        return Task.FromResult(true);
                    }
                };
#endif

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authSettings.JwtIssuer,

                    ValidateAudience = true,
                    ValidAudience = authSettings.JwtIssuer,

                    // Validate signing key instead of asking authority if signing is valid,
                    // since we're skipping on separate identity provider for the purpose of this simple showcase API.
                    // For the same reason we're using symmetric key, while in case of a separate identity provider - even if we wanted local key validation - we'd have only the public key of a public/private keypair.
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(authSettings.JwtSigningKey),
                    ClockSkew = TimeSpan.FromMinutes(5),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });
    }

    //public static void AddAuthorizationPolicies(this IServiceCollection services, IConfiguration configuration)
    //{
    //    string admin = configuration["ApiRoles:AdminRole"],
    //            manager = configuration["ApiRoles:ManagerRole"], employee = configuration["ApiRoles:EmployeeRole"];

    //    services.AddAuthorization(options =>
    //    {
    //        options.AddPolicy(AuthorizationConsts.AdminPolicy, policy => policy.RequireRole(admin));
    //        options.AddPolicy(AuthorizationConsts.ManagerPolicy, policy => policy.RequireRole(manager, admin));
    //        options.AddPolicy(AuthorizationConsts.EmployeePolicy, policy => policy.RequireRole(employee, manager, admin));
    //    });
    //}
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Core
        services.AddScoped<ITokenService, JwtTokenService>();
        services.RegisterMyOptions<AuthenticationSettings>();
        //ConfigureLocalJwtAuthentication(services, configuration.GetMyOptions<AuthenticationSettings>());
    }
}
