using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using System.Security.Claims;

namespace SprintFlow.Tests.Users;

public class UserContextTests
{
    [Fact()]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        //arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier , "1"),
            new(ClaimTypes.Email , "test@test.com"),
            new(ClaimTypes.Role , nameof(UserRole.Admin)),
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims,"Test"));

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });
        var userContext = new UserContext(httpContextAccessorMock.Object);
        //act
        var currentUser = userContext.GetCurrentUser();
        //assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@test.com");
        currentUser.Roles.Should().ContainInOrder(nameof(UserRole.Admin));

    }

    [Fact()]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        //arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);
        var userContext = new UserContext(httpContextAccessorMock.Object);
        //act
        Action action = () => userContext.GetCurrentUser();
        //assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("User Context is not present");

    }
}
