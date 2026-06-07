namespace sprintFlow.Application.Users.Dto;

public class UserConcurrencyDto
{
    public string UserId { get; set; } = default!;
    public string? UserName { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; } = null!;
    public string RowVersion { get; set; } = default!;

}
