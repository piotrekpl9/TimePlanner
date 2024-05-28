using Application.Authentication.Model;
using Domain.User.Errors;
using Domain.User.Models;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.User.Validators;

public sealed class PasswordUpdateRequestValidator : AbstractValidator<PasswordUpdateRequest>
{
    public PasswordUpdateRequestValidator(IUserRepository userRepository,IPasswordHasher<Domain.User.Entities.User> passwordHasher)
    {

        RuleFor(r => r.OldPassword).Must((oldPassword) 
            => oldPassword.Length > 0).WithMessage(UserError.OldPasswordEmpty.Description);
      
        RuleFor(r => r.NewPassword).Must((newPassword) 
            => newPassword.Length > 0).WithMessage(UserError.NewPasswordEmpty.Description);
      
    }
}