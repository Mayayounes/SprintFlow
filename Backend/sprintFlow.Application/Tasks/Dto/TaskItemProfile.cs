using AutoMapper;
using sprintFlow.Application.Tasks.Commands.CreateTask;
using sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Domain.Entities;

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

        CreateMap<CreateTaskCommand, TaskItem>();
        CreateMap<UpdateTaskDetailsCommand, TaskItem>();
    }
}