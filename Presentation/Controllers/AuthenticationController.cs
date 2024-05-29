using Application.Authentication;
using Application.Authentication.Model;
using Domain.Shared;
using Domain.Task.Models.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IAuthenticationService authenticationService) : Controller
{
    [HttpPost("/sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest,Type = typeof(Error))]
    [AllowAnonymous]
    public async Task<IResult> SignUp(
        [FromBody] RegisterRequest registerRequest,
        IValidator<RegisterRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(registerRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var result = await authenticationService.Register(registerRequest);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }  
    
    [HttpPost("/sign-in")]
    [AllowAnonymous]
    public async Task<IResult> SignIn(
        [FromBody] LoginRequest loginRequest,
        IValidator<LoginRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(loginRequest);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        var result = await authenticationService.Login(loginRequest);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }
    
  
}