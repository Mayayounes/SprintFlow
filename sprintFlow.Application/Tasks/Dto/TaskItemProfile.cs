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
    }
}
