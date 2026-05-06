using AutoMapper;
using sprintFlow.Domain.Entities;
using sprintFlow.Application.Tasks.Dto;

public class TaskItemProfile : Profile
{
    public TaskItemProfile()
    {
        CreateMap<TaskItem, TaskItemDto>()
                .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src =>
                    src.Employee != null ? src.Employee.UserName : null))
    .ForMember(dest => dest.Status,
        opt => opt.MapFrom(src => src.Status.ToString()))
    .ForMember(dest => dest.CompletionStatus,
        opt => opt.MapFrom(src => src.CompletionStatus.ToString()));

        CreateMap<TaskItem, EmployeeTaskDto>()
            .ForMember(dest => dest.ManagerName,
                opt => opt.MapFrom(src => src.Project.Manager != null ? src.Project.Manager.UserName : null))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));
    }
}