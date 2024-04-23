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
    [HttpPost]
    [Authorize]
    [Route("/updatePassword")]
    public async Task<IResult> SignIn(
        [FromBody] PasswordUpdateRequest updateRequest,
        IValidator<PasswordUpdateRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(updateRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        var targetUserId = new UserId(updateRequest.Guid);
        var authorisationResult = userId.Equals(targetUserId);

        if (!authorisationResult)
        {
            return Results.Forbid();
        }
        
        var result = await userService.UpdatePassword(targetUserId,updateRequest);
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }
}