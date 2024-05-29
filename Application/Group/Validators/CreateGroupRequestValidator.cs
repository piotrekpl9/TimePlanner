using Application.Authentication.Model;
using Domain.Group.Models;
using FluentValidation;

namespace Application.Group.Validators;

public sealed class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>
{
    public CreateGroupRequestValidator()
    {
        RuleFor(r => r.Name).Must((name) 
            => name.Length > 0).WithMessage("You have to provide name!");
    }
}