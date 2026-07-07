using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BE_CourseSelling.Infrastructure.Entities;
using BE_CourseSelling.Core.Interfaces.Services;
using BE_CourseSelling.Core.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace BE_CourseSelling.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        var (succeeded, errors) = await _authService.RegisterAsync(model);
        if (!succeeded)
            return BadRequest(errors);

        return Ok(new { message = "User registered" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var result = await _authService.LoginAsync(model);
        if (result == null)
            return Unauthorized("Invalid credentials");

        return Ok(result);
    }
}
