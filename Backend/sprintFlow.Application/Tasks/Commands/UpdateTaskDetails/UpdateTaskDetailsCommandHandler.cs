using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;

public class UpdateTaskDetailsCommandHandler(IUserContext userContext, IProjectRepository projectRepository, ITaskRepository taskRepository, INotificationService notificationService) : IRequestHandler<UpdateTaskDetailsCommand, Result<TaskConcurrencyDto>>
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

        var submittedVersion = Convert.FromBase64String(request.RowVersion);

        await taskRepository.SetOriginalRowVersion(task, submittedVersion);

        // apply updates
        task.Title = request.Title;
        task.Description = request.Description;
        task.Deadline = request.Deadline;

        var result = await taskRepository.SaveChangesSafe();
        if (!result.Success)
        {
            return Result<TaskConcurrencyDto>.Failure(
                ["ConcurrencyConflict"],
                "ConcurrencyConflict",
                result.Latest == null ? null : new TaskConcurrencyDto
                {
                    TaskId = result.Latest.Id,
                    Title = result.Latest.Title,
                    Description = result.Latest.Description ?? "",
                    Deadline = result.Latest.Deadline,
                    AssignedDate = result.Latest.AssignedDate,
                    ProjectId = result.Latest.ProjectId,
                    RowVersion = Convert.ToBase64String(result.Latest.RowVersion)
                }
            );
        }
        if (!string.IsNullOrEmpty(task.EmployeeId))
        {
            await notificationService.SendAsync(
                Guid.Parse(task.EmployeeId),
                $"The task '{task.Title}' information was updated."
            );
        }
        return Result<TaskConcurrencyDto>.Success(
            new TaskConcurrencyDto
            {
                TaskId = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                AssignedDate = task.AssignedDate,
                ProjectId = task.ProjectId,
                RowVersion = Convert.ToBase64String(task.RowVersion)
            },
            "Task updated successfully"
        );
    }
}