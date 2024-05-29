using Application.Authentication.Model;
using Domain.Task.Models;
using FluentValidation;

namespace Application.Task.Validators;

public sealed class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(r => r.Name).Must((name) 
            => name.Length > 0).WithMessage("You have to provide name!");
        
        RuleFor(r => r.PlannedStartHour).Must((request,plannedStartDate) 
            => plannedStartDate < request.PlannedEndHour).WithMessage("The start hour must be less than the end date!");
        
        RuleFor(r => r.PlannedEndHour).Must((request,plannedEndDate) 
            => plannedEndDate > request.PlannedStartHour).WithMessage("The end date must be greater than the end date!");
    }
}