using FluentAssertions;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;

namespace SprintFlow.Tests.Users;

public class CurrentUserTests
{
    //TestMethod_Scenario_ExpectResult
    //[Fact()]
    //public void IsInRole_WithMatchingRole_ShouldReturnTrue()
    //{
    //    //arrange
    //    var currentUser = new CurrentUser("1", "test@test.com", [nameof(UserRole.Admin), nameof(UserRole.Leader), nameof(UserRole.Employee)]);
    //    //act
    //    var isInRole = currentUser.IsInRole(nameof(UserRole.Admin));
    //    //assert
    //    isInRole.Should().BeTrue();
    //}

    [Fact()]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@test.com", [nameof(UserRole.Admin), nameof(UserRole.Leader)]);
        //act
        var isInRole = currentUser.IsInRole(nameof(UserRole.Employee));
        //assert
        isInRole.Should().BeFalse();
    }
    [Theory]
    [InlineData(nameof(UserRole.Admin))]
    [InlineData(nameof(UserRole.Leader))]
    [InlineData(nameof(UserRole.Employee))]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        //arrange
        var currentUser = new CurrentUser("1", "test@test.com", [nameof(UserRole.Admin), nameof(UserRole.Leader), nameof(UserRole.Employee)]);
        //act
        var isInRole = currentUser.IsInRole(roleName);
        //assert
        isInRole.Should().BeTrue();
    }
}
