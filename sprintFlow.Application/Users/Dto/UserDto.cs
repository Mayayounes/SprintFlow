using sprintFlow.Domain.Constants;
using System.Text.Json.Serialization;

namespace sprintFlow.Application.Users.Dto;

public class UserDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRoles Role { get; set; }
}
