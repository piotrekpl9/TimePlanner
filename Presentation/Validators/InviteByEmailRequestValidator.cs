using Domain.Group.Models;
using FluentValidation;
using Presentation.Model;
using Presentation.Model.Requests;

namespace Presentation.Validators;

public sealed class InviteByEmailRequestValidator : AbstractValidator<InviteUserRequest>
{
    public InviteByEmailRequestValidator()
    {
        RuleFor(r => r.Email).Must((name) 
            => name.Length > 0).WithMessage("You have to provide email!");
    }
}