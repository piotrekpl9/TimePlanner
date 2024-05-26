using System.Security.Claims;
using Application.Authentication;
using Application.Authentication.Model;
using Domain.Shared;
using Domain.User.Services;
using Domain.User.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : Controller
{
    [HttpPost("/update-password")]
    [Authorize]
    public async Task<IResult> Update(
        [FromBody] PasswordUpdateRequest updateRequest,
        IValidator<PasswordUpdateRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(updateRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        
        var result = await userService.UpdatePassword(userId, updateRequest);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }
}