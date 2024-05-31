using Domain.User.Errors;
using Domain.User.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Presentation.Model;
using Presentation.Model.Requests;

namespace Presentation.Validators;

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