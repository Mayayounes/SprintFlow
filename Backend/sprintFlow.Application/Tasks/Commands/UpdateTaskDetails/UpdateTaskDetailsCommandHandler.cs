using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;

public class UpdateTaskDetailsCommandHandler(IUserContext userContext, IMapper mapper, IProjectRepository projectRepository, ITaskRepository taskRepository) : IRequestHandler<UpdateTaskDetailsCommand, Result<TaskConcurrencyDto>>
{
    public async Task<Result<TaskConcurrencyDto>> Handle(UpdateTaskDetailsCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            return Result<TaskConcurrencyDto>.Failure(new List<string> { "Current user not found" });
        }

        var ManagerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);
        if (ManagerId == null || currentUser.Id != ManagerId)
        {
            return Result<TaskConcurrencyDto>.Failure(
                new List<string> { "You are not authorized to update information of this task." }
            );
        }
        var task = await taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            return Result<TaskConcurrencyDto>.Failure(new List<string> { "Task not found." });
        }
        if (request.Deadline <= task.AssignedDate)
        {
            return Result<TaskConcurrencyDto>.Failure(new List<string>
    {
        "Deadline must be after the assigned date."
    });
        }
        // Apply updates
        task.Title = request.Title;
        task.Description = request.Description;
        task.Deadline = request.Deadline;
        var submittedVersion = Convert.FromBase64String(request.RowVersion);
        await taskRepository.SetOriginalRowVersion(task, submittedVersion);
        try
        {
            await taskRepository.UpdateAsync(task);
            return Result<TaskConcurrencyDto>.Success(
                new TaskConcurrencyDto
                {
                    TaskId = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Deadline = task.Deadline,
                    RowVersion = Convert.ToBase64String(task.RowVersion)
                },
                "Task updated successfully");
        }
        catch (DbUpdateConcurrencyException)
        {
            var currentTask = await taskRepository.GetDatabaseValues(task);

            return Result<TaskConcurrencyDto>.Failure(
                new List<string>
                {
                    "This task was modified by another user."
                },
                "ConcurrencyConflict",
                new TaskConcurrencyDto
                {
                    TaskId = currentTask!.Id,
                    Title = currentTask.Title,
                    Description = currentTask.Description,
                    Deadline = currentTask.Deadline,
                    RowVersion = Convert.ToBase64String(currentTask.RowVersion)
                });
        }
    }
}
