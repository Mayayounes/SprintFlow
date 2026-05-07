using AutoMapper;
using FluentAssertions;
using Moq;
using sprintFlow.Application.Projects.Commands.CreateProject;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace SprintFlow.Tests.Projects.Commands.CreateProject;

public class CreateProjectCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnCreatedProjectId()
    {
        //Arrange
        var mapperMock = new Mock<IMapper>();

        var command = new CreateProjectCommand();
        var project = new Project();
        mapperMock.Setup(m => m.Map<Project>(command)).Returns(project);

        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<Project>()))
            .ReturnsAsync(Guid.NewGuid());

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("manager_id", "manager@test.com", []);
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateProjectCommandHandler(userContextMock.Object, mapperMock.Object, projectRepositoryMock.Object);

        //Act
        var result = await commandHandler.Handle(command, CancellationToken.None);
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBe(Guid.Empty); 
        project.ManagerId.Should().Be("manager_id");
        projectRepositoryMock.Verify(p => p.Create(project), Times.Once);
    }
}
