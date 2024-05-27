using Application.Authentication;
using Application.Common.Data;
using Application.Group;
using Application.Task;
using Application.User;
using Domain.Group.Services;
using Domain.Task.Services;
using Domain.User.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<Domain.User.Entities.User>,PasswordHasher<Domain.User.Entities.User>>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IGroupService,GroupService>();
        services.AddScoped<ITaskService,TaskService>();
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        return services;
    }
}

