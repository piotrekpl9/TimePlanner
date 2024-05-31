using Application.Authentication.Model;
using FluentValidation;
using Presentation.Model.Requests;

namespace Presentation.Validators;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
       
        RuleFor(r => r.Password).Must((name) 
            => name.Length > 0).WithMessage("You have to provide password!");
        RuleFor(r => r.Email).Must((surname) 
            => surname.Length > 0).WithMessage("You have to provide e-mail!");
        
        
    }
}