using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler(ITaskRepository taskRepository, IUserContext userContext) : IRequestHandler<UpdateTaskStatusCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
            return Result<Guid>.Failure(new List<string> { "Task not found." });

        var EmployeeId = task.EmployeeId;
        var currentUser = userContext.GetCurrentUser();
        if (currentUser!.Id != EmployeeId.ToString())
            return Result<Guid>.Failure(new List<string> { "You are not authorized to update this task." });

        if (task.Status == TaskItemStatus.ToDo && request.Status == 1)
        {
            task.StartedAt = DateTime.UtcNow;
        }

        if (task.Status == TaskItemStatus.InProgress && request.Status == 2)
        {
            task.CompletedAt = DateTime.UtcNow;
        }

        task.Status = (TaskItemStatus)request.Status!.Value;

        await taskRepository.UpdateAsync(task);
        return Result<Guid>.Success(task.Id, "Task updated successfully");

    }
}
