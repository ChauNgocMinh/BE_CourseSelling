namespace BE_CourseSelling.Core.DTOs;

public record RegisterDto(string Email, string Password, string? Role);
public record LoginDto(string Email, string Password);
public record AuthResponseDto(string Token, DateTime ExpiresAt);
