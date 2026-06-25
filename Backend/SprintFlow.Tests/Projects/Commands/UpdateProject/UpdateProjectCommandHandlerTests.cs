using AutoMapper;
using FluentAssertions;
using Moq;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Projects.Commands.UpdateProject;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace SprintFlow.Tests.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidRequest_ReturnsSuccess()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var userContextMock = new Mock<IUserContext>();
        var projectRepositoryMock = new Mock<IProjectRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var command = new UpdateProjectCommand
        {
            Id = Guid.NewGuid()
        };

        var project = new Project
        {
            Id = command.Id,
            ManagerId = "manager_id"
        };

        var currentUser = new CurrentUser("manager_id", "manager@test.com", []);

        projectRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(project);

        userContextMock
            .Setup(u => u.GetCurrentUser())
            .Returns(currentUser);

        mapperMock
            .Setup(m => m.Map(command, project));

        var handler = new UpdateProjectCommandHandler(
            mapperMock.Object,
            userContextMock.Object,
            projectRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(project.Id);
        result.Message.Should().Be("Project updated successfully");

        unitOfWorkMock.Verify(
            u => u.SaveChangesAsync(),
            Times.Once);
        mapperMock.Verify(m => m.Map(command, project), Times.Once);
    }
    [Fact]
    public async Task Handle_ProjectNotFound_ReturnsFailure()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var userContextMock = new Mock<IUserContext>();
        var projectRepositoryMock = new Mock<IProjectRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var command = new UpdateProjectCommand
        {
            Id = Guid.NewGuid()
        };

        projectRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((Project?)null);

        var handler = new UpdateProjectCommandHandler(
            mapperMock.Object,
            userContextMock.Object,
            projectRepositoryMock.Object,
            unitOfWorkMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Not Found");
        result.Errors.Should().Contain("Project not found");

        unitOfWorkMock.Verify(
            u => u.SaveChangesAsync(),
            Times.Never);
        mapperMock.Verify(m => m.Map(It.IsAny<UpdateProjectCommand>(), It.IsAny<Project>()), Times.Never);
    }
    [Fact]
    public async Task Handle_UserNotManager_ReturnsForbidden()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var userContextMock = new Mock<IUserContext>();
        var projectRepositoryMock = new Mock<IProjectRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var command = new UpdateProjectCommand
        {
            Id = Guid.NewGuid()
        };

        var project = new Project
        {
            Id = command.Id,
            ManagerId = "another_manager"
        };

        var currentUser = new CurrentUser("user_1", "user@test.com", []);

        projectRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(project);

        userContextMock
            .Setup(u => u.GetCurrentUser())
            .Returns(currentUser);

        var handler = new UpdateProjectCommandHandler(
            mapperMock.Object,
            userContextMock.Object,
            projectRepositoryMock.Object,
            unitOfWorkMock.Object

        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be("Forbidden");
        result.Errors.Should().Contain("You are not allowed to update this project");

        unitOfWorkMock.Verify(
            u => u.SaveChangesAsync(),
            Times.Never);
        mapperMock.Verify(m => m.Map(It.IsAny<UpdateProjectCommand>(), It.IsAny<Project>()), Times.Never);
    }
}
