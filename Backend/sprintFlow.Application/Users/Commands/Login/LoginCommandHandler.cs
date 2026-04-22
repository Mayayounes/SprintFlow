using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sprintFlow.Application.Users.Commands.Login;

public class LoginCommandHandler(UserManager<User> userManager, IConfiguration configuration) : IRequestHandler<LoginCommand, Result<LoginDto>>
{
    public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.email);

        if (user == null || !await userManager.CheckPasswordAsync(user, request.password))
        {
            return Result<LoginDto>.Failure(new List<string> { "User Not Found." });
        }
        var roles = await userManager.GetRolesAsync(user);
        var roleString = roles.FirstOrDefault();

        if (!Enum.TryParse<UserRole>(roleString, true, out var roleEnum))
        {
            return Result<LoginDto>.Failure(new List<string>
    {
        "Invalid role assigned to user."
    });
        }
        var token = GenerateJwtToken(user, roles);

        return Result<LoginDto>.Success(new LoginDto
        {
            Token = token,
            Role = roleEnum,
            Email = user.Email!,
            UserId = user.Id
        });
    }

    private string GenerateJwtToken(IdentityUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),

        };

        foreach (var r in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, r));

        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}