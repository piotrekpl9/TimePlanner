using System.Security.Claims;
using Application.Authentication;
using Application.Authentication.Model;
using AutoMapper;
using Domain.Shared;
using Domain.User.Models;
using Domain.User.Models.Dtos;
using Domain.User.Services;
using Domain.User.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Model;
using Presentation.Model.Requests;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, IMapper mapper) : Controller
{
    [HttpPost("/update-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    public async Task<IResult> Update(
        [FromBody] PasswordUpdateRequest updateRequest,
        [FromServices] IValidator<PasswordUpdateRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(updateRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        
        var result = await userService.UpdatePassword(userId,mapper.Map<UpdatePasswordDto>( updateRequest));
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);

    }
}