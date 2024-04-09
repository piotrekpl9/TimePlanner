using Application.Authentication;
using Domain.User.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    // GET
    [HttpPost]
    public IActionResult Login([FromBody] CreateUserDto createUserDto)
    {
        var result =  _authenticationService.Register(createUserDto);
        return Ok(result);
    }
}