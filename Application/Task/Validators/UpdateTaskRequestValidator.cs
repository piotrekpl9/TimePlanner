using Application.Authentication.Model;
using Domain.Task.Models;
using FluentValidation;

namespace Application.Task.Validators;

public sealed class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        
        RuleFor(r => r.PlannedStartHour).Must((request,plannedStartDate) 
            => plannedStartDate == null || (plannedStartDate < request.PlannedEndHour) ).WithMessage("The start hour must be less than the end date!");
        
        RuleFor(r => r.PlannedEndHour).Must((request,plannedEndDate) 
            =>  plannedEndDate == null || (plannedEndDate > request.PlannedStartHour)).WithMessage("The end date must be greater than the end date!");
    }
}