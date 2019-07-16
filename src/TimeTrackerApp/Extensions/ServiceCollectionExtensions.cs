using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerApp.Extensions
{
    // ekstenzija na IServiceCollection
    public static class ServiceCollectionExtensions
    {
       public static void AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // svi servisi za autentifikaciju
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["Tokens:Issuer"],
                        ValidAudience = configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"]))
                    };

                    options.TokenValidationParameters = tokenValidationParameters;
                });
        }

        public static void AddOpenApi(this IServiceCollection services)
        {
            services.AddSwaggerDocument(
                options =>
                {
                    options.OperationProcessors.Add(
                        new OperationSecurityScopeProcessor("jwt-token"));      // pristup preko jwt-token-a
                    options.DocumentProcessors.Add(
                        new SecurityDefinitionAppender(
                            "jwt-token", new[] { "" }, new OpenApiSecurityScheme
                            {
                                Type = OpenApiSecuritySchemeType.ApiKey,
                                Name = "Authorization",
                                Description =
                                    "Enter \"Bearer jwt-token\" as value. " +
                                    "Use https://localhost:44397/get-token to get read-only JWT token. " +
                                    "Use https://localhost:44397/get-token?admin=true to get admin (read-write) JWT token.",
                                In = OpenApiSecurityApiKeyLocation.Header
                            }));

                    options.PostProcess = document =>
                    {
                        document.Info.Version = "v1";
                        document.Info.Title = "Time Tracker API v1";
                        document.Info.Description = "An API for ASP.NET Core Workshop";
                        document.Info.TermsOfService = "Do whatever you want with it :)";
                        document.Info.Contact = new OpenApiContact
                        {
                            Name = "Nikola Blagojevic",
                            Email = string.Empty,
                            Url = "https://nikolablagojevic.com"
                        };
                        document.Info.License = new OpenApiLicense
                        {
                            Name = "MIT",
                            Url = "https://opensource.org/licenses/MIT"
                        };
                    };
                });
        }
    }
}
