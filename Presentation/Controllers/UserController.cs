using System.Security.Claims;
using AutoMapper;
using Domain.Shared;
using Domain.User.Models.Dtos;
using Domain.User.Repositories;
using Domain.User.Services;
using Domain.User.ValueObjects;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Model.Requests;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, IUserRepository userRepository, IMapper mapper) : Controller
{
    [HttpPut("update-password")]
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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> Get()
    {   
        var userId = new UserId(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty));
        
        var result = await userRepository.GetById(userId);
        return result != null ? Results.Ok(mapper.Map<UserDto>(result)) : Results.NotFound();

    }
}