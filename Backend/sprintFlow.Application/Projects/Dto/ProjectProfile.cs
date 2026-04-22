using AutoMapper;
using sprintFlow.Application.Projects.Commands.CreateProject;
using sprintFlow.Application.Projects.Commands.UpdateProject;
using sprintFlow.Application.Projects.Queries.GetProjectById;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Projects.Dto;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {

        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.ManagerName,
                opt => opt.MapFrom(src => src.Manager.UserName));

        CreateMap<CreateProjectCommand, Project>();
        CreateMap<SingleProjectDto, GetProjectByIdQuery>();
        CreateMap<UpdateProjectCommand, Project>();
        CreateMap<Project, SingleProjectDto>();
        CreateMap<TaskItem, TaskItemDto>();
        
    }

}
