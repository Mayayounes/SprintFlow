using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace sprintFlow.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler(IUserContext userContext, IMapper mapper, IProjectRepository projectRepository, ITaskRepository taskRepository , IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            return Result<Guid>.Failure(new List<string> { "Current User not found" });
        }
        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
        {
            return Result<Guid>.Failure(new List<string> { "Project Not Found" });
        }
        if (project.ManagerId != currentUser.Id)
        {
            return Result<Guid>.Failure(new List<string>
        {
            "You are not authorized to create a task for this project."
        });
        }
        var task = mapper.Map<TaskItem>(request);
        var taskId = await taskRepository.Create(task);
        await unitOfWork.SaveChangesAsync();
        return Result<Guid>.Success(taskId, "Task created successfully");
    }
}
