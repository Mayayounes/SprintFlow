using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler(ITaskRepository taskRepository ,IUserContext userContext , INotificationService notificationService): IRequestHandler<DeleteTaskCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var task = await taskRepository.GetByIdAsync(request.TaskId);

        if (task == null)
        {
            return Result<bool>.Failure(
                new List<string> { "Task not found" },
                "Delete failed",
                false
            );
        }
        var currentUser = userContext.GetCurrentUser();
        var userId = Guid.Parse(currentUser!.Id);

        if (!await taskRepository.IsProjectOwnerOfTask(request.TaskId, userId))
        {
            return Result<bool>.Failure(
                new List<string> { "Unauthorized" },
                "Only project manager can delete this task",
                false
            );
        }
    
        var assignedEmployeeId = task.EmployeeId;
        var taskTitle = task.Title;
        if (!string.IsNullOrEmpty(assignedEmployeeId))
        {
            await notificationService.SendAsync(
                Guid.Parse(assignedEmployeeId),
                $"Task '{taskTitle}' has been deleted."
            );
        }
        await taskRepository.Delete(task);
        await taskRepository.SaveChangesSafe();

        return Result<bool>.Success(true, "Task deleted successfully");
    }
}