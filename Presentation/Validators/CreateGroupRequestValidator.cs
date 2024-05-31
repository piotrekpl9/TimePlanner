using Domain.Group.Models;
using FluentValidation;
using Presentation.Model;
using Presentation.Model.Requests;

namespace Presentation.Validators;

public sealed class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>
{
    public CreateGroupRequestValidator()
    {
        RuleFor(r => r.Name).Must((name) 
            => name.Length > 0).WithMessage("You have to provide name!");
    }
}