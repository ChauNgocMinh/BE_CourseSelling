using BE_CourseSelling.Core.DTOs;
using BE_CourseSelling.Core.Interfaces.Services;
using BE_CourseSelling.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BE_CourseSelling.Service.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAsync(RegisterDto model)
    {
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description));
        }

        await _userManager.AddToRoleAsync(user, model.Role ?? "User");
        return (true, Array.Empty<string>());
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, roles);
        return token;
    }

    private AuthResponseDto GenerateJwtToken(ApplicationUser user, IList<string> roles)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var jwtKey = jwtSection["Key"] ?? "ReplaceWithSecureKeyShouldBeInSecrets";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var expires = DateTime.UtcNow.AddHours(6);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var written = new JwtSecurityTokenHandler().WriteToken(token);
        return new AuthResponseDto(written, expires);
    }
}
