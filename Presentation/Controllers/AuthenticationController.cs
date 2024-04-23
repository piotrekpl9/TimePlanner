using Application.Authentication;
using Application.Authentication.Model;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IAuthenticationService authenticationService) : Controller
{
    [HttpPost]
    [Route("/signUp")]
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
    
    [HttpPost]
    [Route("/signIn")]
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