using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskStatus;

public class UpdateTaskStatusCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository ,IUserContext userContext , INotificationService notificationService , IUserRepository userRepository) : IRequestHandler<UpdateTaskStatusCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
            return Result<Guid>.Failure(new List<string> { "Task not found." });

        var EmployeeId = task.EmployeeId;
        var currentUser = userContext.GetCurrentUser();
        if (currentUser!.Id != EmployeeId?.ToString())
            return Result<Guid>.Failure(new List<string> { "You are not authorized to update this task." });

        var newStatus = (TaskItemStatus)request.Status!.Value;

        var isValidTransition =
            (task.Status == TaskItemStatus.ToDo &&
             newStatus == TaskItemStatus.InProgress)

            ||

            (task.Status == TaskItemStatus.InProgress &&
             newStatus == TaskItemStatus.Done);

        if (!isValidTransition)
        {
            return Result<Guid>.Failure(
                new List<string>
                {
            $"Cannot change task status from {task.Status} to {newStatus}."
                });
        }

        var managerId =
          await projectRepository.GetProjectManagerIdAsync(task.ProjectId);

        var employee =
            await userRepository.GetByIdAsync(EmployeeId!);

        string? notificationMessage = null;

        // ToDo -> InProgress
        if (task.Status == TaskItemStatus.ToDo &&
            request.Status == (int)TaskItemStatus.InProgress)
        {
            task.StartedAt = DateTime.UtcNow;

            notificationMessage =
                $"{employee!.UserName} started task '{task.Title}'.";
        }

        // InProgress -> Done
        if (task.Status == TaskItemStatus.InProgress &&
            request.Status == (int)TaskItemStatus.Done)
        {
            task.CompletedAt = DateTime.UtcNow;

            notificationMessage =
                $"{employee!.UserName} completed task '{task.Title}'.";
        }

        task.Status = (TaskItemStatus)request.Status!.Value;
        await taskRepository.SaveChangesSafe(); ;
        await projectRepository.UpdateStatusAsync(task.ProjectId);

        // Notify manager after successful save
        if (!string.IsNullOrWhiteSpace(notificationMessage)
            && !string.IsNullOrWhiteSpace(managerId))
        {
            await notificationService.SendAsync(
                Guid.Parse(managerId),
                notificationMessage);
        }
        return Result<Guid>.Success(task.Id, "Task updated successfully");

    }
}
