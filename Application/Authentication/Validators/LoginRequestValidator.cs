using Application.Authentication.Model;
using Domain.Shared;
using Domain.User.Errors;
using Domain.User.Repositories;
using FluentValidation;

namespace Application.Authentication.Validators;

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