using Application.Authentication.Model;
using Domain.Group.Models;
using FluentValidation;

namespace Application.Group.Validators;

public sealed class InviteByEmailRequestValidator : AbstractValidator<InviteUserRequest>
{
    public InviteByEmailRequestValidator()
    {
        RuleFor(r => r.Email).Must((name) 
            => name.Length > 0).WithMessage("You have to provide email!");
    }
}