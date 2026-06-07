using sprintFlow.Domain.Constants;
using System.Text.Json.Serialization;

namespace sprintFlow.Application.Users.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole? Role { get; set; }
    public string RowVersion { get; set; } = default!;

}
