using AutoMapper;
using sprintFlow.Application.Projects.Commands.CreateProject;
using sprintFlow.Application.Projects.Commands.UpdateProject;
using sprintFlow.Application.Projects.Queries.GetProjectById;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Projects.Dto;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {

        CreateMap<Project, ProjectDto>();

        CreateMap<CreateProjectCommand, Project>();
        CreateMap<SingleProjectDto, GetProjectByIdQuery>();
        CreateMap<UpdateProjectCommand, Project>();
        CreateMap<Project, SingleProjectDto>();
        CreateMap<TaskItem, TaskItemDto>();

    }

}
