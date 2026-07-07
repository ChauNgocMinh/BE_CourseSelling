using BE_CourseSelling.Core.DTOs;

namespace BE_CourseSelling.Core.Interfaces.Services;

public interface IAuthService
{
    Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAsync(RegisterDto model);
    Task<AuthResponseDto?> LoginAsync(LoginDto model);
}
