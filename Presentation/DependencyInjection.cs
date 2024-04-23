using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Common.Services;
using Infrastructure.Authentication;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
    {
       
        var jwtSettings = configuration.GetRequiredSection(JwtSettings.SectionName); 
        var dateTimeProvider = new DateTimeProvider();
        
        services.AddEndpointsApiExplorer();
        
        services.Configure<JwtSettings>(jwtSettings);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>(provider => dateTimeProvider);
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtOptions =>
        {
            var secret = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);
           
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings["Issuer"], 
                ValidAudience = jwtSettings["Audience"], 
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });

        services.AddAuthorization();
        return services;
    }
}