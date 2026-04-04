using AutoMapper;
using sprintFlow.Application.Projects.Commands.CreateProject;
using sprintFlow.Application.Projects.Commands.UpdateProject;
using sprintFlow.Domain.Entities;
using System.Net;

namespace sprintFlow.Application.Projects.Dto;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<CreateProjectCommand, Project>();

        CreateMap<Project, ProjectDto>();
        //    .ForMember(d => d.Dishes, opt => opt.MapFrom(src => src.Dishes == null ? null : src.Dishes));

        CreateMap<UpdateProjectCommand, Project>();

    }

}
