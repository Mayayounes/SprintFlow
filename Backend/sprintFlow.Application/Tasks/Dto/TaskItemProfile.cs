using sprintFlow.Application.Tasks.Commands.CreateTask;
using sprintFlow.Domain.Entities;
using AutoMapper;
using sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;

namespace sprintFlow.Application.Tasks.Dto;

public class TaskItemProfile : Profile
{
    public TaskItemProfile()
    {
        CreateMap<CreateTaskCommand, TaskItem>();
        CreateMap<UpdateTaskDetailsCommand, TaskItem>();

        CreateMap<TaskItem, TaskItemDto>()
            .ForMember(dest => dest.ProjectName,
        opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.EmployeeName,
        opt => opt.MapFrom(src => src.Employee != null ? src.Employee.UserName : null));
    }
}
