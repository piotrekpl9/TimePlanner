using Application.Authentication.Model;
using FluentValidation;

namespace Application.User.Validators;

public sealed class PasswordUpdateRequestValidator : AbstractValidator<PasswordUpdateRequest>
{
    public PasswordUpdateRequestValidator()
    {
       
        RuleFor(r => r.OldPassword).Must((oldPassword) 
            => oldPassword.Length > 0).WithMessage("You have to provide old password!");
        //Verify if old one is correct
        RuleFor(r => r.NewPassword).Must((newPassword) 
            => newPassword.Length > 0).WithMessage("You have to provide new password!");
      //Add validation if new one isnt same as old one
        
    }
}