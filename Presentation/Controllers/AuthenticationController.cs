using Application.Authentication;
using Domain.User.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IAuthenticationService authenticationService) : Controller
{
    [HttpPost]
    [Route("/signUp")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto createUserDto)
    {
        var result = await authenticationService.Register(createUserDto);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }
}