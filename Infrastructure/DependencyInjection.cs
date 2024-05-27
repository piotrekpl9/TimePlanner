using Application.Authentication;
using Application.Common.Data;
using Application.Common.Interfaces;
using Application.Common.Services;
using Domain.Group.Repositories;
using Domain.Task.Repositories;
using Domain.User.Repositories;
using Infrastructure.Authentication;
using Infrastructure.Authorization;
using Infrastructure.Authorization.Group;
using Infrastructure.Authorization.Task;
using Infrastructure.Common;
using Infrastructure.Services;
using Infrastructure.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IApplicationDbContext,ApplicationDbContext>();
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IGroupRepository,GroupRepository>();
        services.AddScoped<ITaskRepository,TaskRepository>();
        services.AddScoped<IAuthenticationService,AuthenticationService>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IAuthorizationHandler, GroupMemberAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, InvitationCreatorAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, InvitationTargetAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, TaskAssignedAuthorizationHandler>();
        
        return services;
    }
}