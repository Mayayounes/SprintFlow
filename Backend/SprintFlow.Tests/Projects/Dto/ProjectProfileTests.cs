using AutoMapper;
using FluentAssertions;
using sprintFlow.Application.Projects.Commands.CreateProject;
using sprintFlow.Application.Projects.Commands.UpdateProject;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;

namespace SprintFlow.Tests.Projects.Dto;

public class ProjectProfileTests
{
    [Fact]
    public void CreateMap_FromProjectToProjectDto_MapCorrectly()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.ManagerName,
                    opt => opt.MapFrom(src => src.Manager.UserName))
                .ForMember(dest => dest.ProjectStatus,
                    opt => opt.MapFrom(src => ProjectStatus.Pending));
        });

        configuration.AssertConfigurationIsValid();

        var mapper = configuration.CreateMapper();

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Test Project",
            Description = "Test Description",
            ManagerId = "123",
            Manager = new User
            {
                UserName = "Maya"
            },
            ProjectStatus = ProjectStatus.Pending
        };

        // Act
        var projectDto = mapper.Map<ProjectDto>(project);

        // Assert
        projectDto.Should().NotBeNull();
        projectDto.Id.Should().Be(project.Id);
        projectDto.Name.Should().Be(project.Name);
        projectDto.Description.Should().Be(project.Description);
        projectDto.ManagerId.Should().Be(project.ManagerId);
        projectDto.ManagerName.Should().Be("Maya");
        projectDto.ProjectStatus.Should().Be(ProjectStatus.Pending);
    }
    [Fact()]
    public void CreateMap_FromProjectToSingleProjectDto_MapCorrectly()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
            cfg.CreateMap<Project, SingleProjectDto>()
    .ForMember(dest => dest.Tasks, opt => opt.Ignore())
        );

        configuration.AssertConfigurationIsValid();

        var mapper = configuration.CreateMapper();

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Test Project",
            Description = "Test Description",
            ManagerId = "123",
            ProjectStatus = ProjectStatus.Pending,
            Manager = new User
            {
                UserName = "Maya"
            }
        };

        // Act
        var result = mapper.Map<SingleProjectDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(project.Id);
        result.Name.Should().Be(project.Name);
        result.Description.Should().Be(project.Description);
        result.ManagerId.Should().Be(project.ManagerId);
        result.ProjectStatus.Should().Be(project.ProjectStatus);
    }
    [Fact]
    public void Map_CreateProjectCommand_To_Project_Should_Map_Correctly()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateProjectCommand, Project>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ManagerId, opt => opt.Ignore())
                .ForMember(dest => dest.Manager, opt => opt.Ignore())
                .ForMember(dest => dest.Tasks, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectStatus, opt => opt.Ignore());
        });

        configuration.AssertConfigurationIsValid();

        var mapper = configuration.CreateMapper();

        var command = new CreateProjectCommand
        {
            Name = "New Project",
            Description = "Test Description"
        };

        // Act
        var result = mapper.Map<Project>(command);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
        result.ManagerId.Should().BeNull();
        result.Tasks.Should().BeEmpty();
    }
    [Fact]
    public void Map_UpdateProjectCommand_To_Project_Should_Map_Correctly()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UpdateProjectCommand, Project>()
                .ForMember(dest => dest.ManagerId, opt => opt.Ignore())
                .ForMember(dest => dest.Manager, opt => opt.Ignore())
                .ForMember(dest => dest.Tasks, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectStatus, opt => opt.Ignore());
        });

        configuration.AssertConfigurationIsValid();

        var mapper = configuration.CreateMapper();

        var command = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Project",
            Description = "Updated Description"
        };

        // Act
        var result = mapper.Map<Project>(command);

        // Assert
        result.Id.Should().Be(command.Id);
        result.Name.Should().Be(command.Name);
        result.Description.Should().Be(command.Description);
    }
}