using Application.Authentication.Model;
using Domain.User.Errors;
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
        RuleFor(r => r.Guid).Must((guid) 
            => guid != new Guid()).WithMessage(UserError.GuidNotProvided.Description);
        
        RuleFor(r => r.Guid).MustAsync(async (guid, _) 
            => await userRepository.GetById(new UserId(guid)) is not null).WithMessage(UserError.DoesntExists.Description);
        
        RuleFor(r => r.OldPassword).MustAsync(async (userRequest,  oldPassword, _) =>
        {
            var user = await userRepository.GetById(new UserId(userRequest.Guid));
            if (user is null) return false;

            return passwordHasher.VerifyHashedPassword(null, user.Password, oldPassword) ==
                   PasswordVerificationResult.Success;
        }).WithMessage(UserError.BadOldPassword.Description);
        
        RuleFor(r => r.NewPassword).Must((newPassword) 
            => newPassword.Length > 0).WithMessage(UserError.NewPasswordEmpty.Description);
        
        RuleFor(r => r.NewPassword).MustAsync(async (userRequest,  newPassword, _) =>
        { 
            var user = await userRepository.GetById(new UserId(userRequest.Guid));
            if (user is null) return false;

            return passwordHasher.VerifyHashedPassword(null, user.Password, newPassword) !=
                   PasswordVerificationResult.Success;
        }).WithMessage(UserError.PasswordsIdentical.Description);
        
    }
}