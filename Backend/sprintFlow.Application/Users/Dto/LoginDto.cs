using sprintFlow.Domain.Constants;

namespace sprintFlow.Application.Users.Dto;

public class LoginDto
{
    public string? Email { get; set; }
    public string Token { get; set; } = default!;
    public UserRole Role { get; set; }
    public string UserId { get; set; } = default!;
    public string TimeZoneId { get; set; } = "UTC";

}