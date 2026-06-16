using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sprintFlow.Application.Common;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;

public class UpdateTaskDetailsCommandHandler(IMapper mapper,IUserContext userContext, IProjectRepository projectRepository, ITaskRepository taskRepository, INotificationService notificationService) : IRequestHandler<UpdateTaskDetailsCommand, Result<TaskItemDto>>
{
    public async Task<Result<TaskItemDto>> Handle(UpdateTaskDetailsCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            return Result<TaskItemDto>.Failure(new List<string> { "Current user not found" });
        }

        var ManagerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);
        if (ManagerId == null || currentUser.Id != ManagerId)
        {
            return Result<TaskItemDto>.Failure(
                new List<string> { "You are not authorized to update information of this task." }
            );
        }
        var task = await taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            return Result<TaskItemDto>.Failure(new List<string> { "Task not found." });
        }
        if (request.Deadline <= task.AssignedDate)
        {
            return Result<TaskItemDto>.Failure(new List<string>
    {
        "Deadline must be after the assigned date."
    });
        }

        var submittedVersion =Convert.FromBase64String(request.RowVersion);

        await taskRepository.SetOriginalRowVersion(task,submittedVersion);

        task.Title = request.Title;
        task.Description = request.Description;
        task.Deadline = request.Deadline;

        var result = await taskRepository.SaveChangesSafe();
        if (!result.Success)
        {
            var latestDto = result.Latest == null
                ? null
                : mapper.Map<TaskItemDto>(result.Latest);

            return Result<TaskItemDto>.Failure(
                new List<string>
                {
                "This record was modified by another user. Refresh and try again."
                },
                "ConcurrencyConflict",
                latestDto
            );
        }
        if (!string.IsNullOrEmpty(task.EmployeeId))
        {
            await notificationService.SendAsync(
                Guid.Parse(task.EmployeeId),
                $"The task '{task.Title}' information was updated."
            );
        }
        var dto = mapper.Map<TaskItemDto>(task);

        return Result<TaskItemDto>.Success(dto, "Task updated successfully"); ;
    }
}