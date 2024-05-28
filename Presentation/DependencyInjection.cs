using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Common.Services;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Authorization.Group;
using Infrastructure.Authorization.Task;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
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
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
     
        services.AddControllers(config =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        });
        
        services.AddAuthorizationBuilder()
                    .AddPolicy("GroupAccessPolicy", policy =>
                policy.Requirements.Add(new IsGroupMemberRequirement()))
                    .AddPolicy("InvitationOperationPolicy", policy =>
                policy.Requirements.Add(new IsInvitationTargetRequirement()))
                    .AddPolicy("InvitationCancelPolicy", policy =>
                policy.Requirements.Add(new IsInvitationCreatorRequirement()))
                    .AddPolicy("TaskAssignedPolicy", policy =>
                policy.Requirements.Add(new IsAssignedToTaskAuthorizationRequirement()));

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