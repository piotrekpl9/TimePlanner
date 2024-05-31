using Application.Authentication.Model;
using Domain.User.Errors;
using Domain.User.Repositories;
using FluentValidation;
using Presentation.Model.Requests;

namespace Presentation.Validators;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(IUserRepository userRepository)
    {
        RuleFor(r => r.Email).Must((email) 
            => email.Length != 0).WithMessage(UserError.EmailEmpty.Description);

        RuleFor(r => r.Email).MustAsync(async (email, _) 
            => await userRepository.GetByEmail(email) is null).WithMessage(UserError.AlreadyExists.Description);
        
        RuleFor(r => r.Password).Must((password) 
            => password.Length >= 6).WithMessage(UserError.PasswordToShort.Description); 
        RuleFor(r => r.Password).Must((password) 
            => password.Length <= 30).WithMessage(UserError.PasswordToLong.Description);
        
        RuleFor(r => r.Name).Must((name) 
            => name.Length > 0).WithMessage(UserError.NameEmpty.Description);
        RuleFor(r => r.Surname).Must((surname) 
            => surname.Length > 0).WithMessage(UserError.SurnameEmpty.Description);
        
        
    }
}