using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.AssignEmployeeToTask;

public class AssignEmployeeToTaskCommandHandler(IUserContext userContext, IProjectRepository projectRepository, ITaskRepository taskRepository, IUserRepository userRepository) : IRequestHandler<AssignEmployeeToTaskCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AssignEmployeeToTaskCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
        {
            return Result<string>.Failure(
                new List<string> { "Project not found" },
                "Not Found"
            );
        }
        var task = await taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            return Result<string>.Failure(
                new List<string> { "Task not found" },
                "Not Found"
            );
        }
        if (task.ProjectId != request.ProjectId)
        {
            return Result<string>.Failure(
                new List<string> { "Task does not belong to this project" },
                "Invalid Operation"
            );
        }
        var currentUser = userContext.GetCurrentUser();
        if (project.ManagerId != currentUser.Id)
        {
            return Result<string>.Failure(
                new List<string> { "Only the project manager can assign tasks" },
                "Unauthorized"
            );
        }
        var isEmployee = await userRepository.IsUserInRoleAsync(request.EmployeeId, UserRole.Employee);
        if (!isEmployee)
        {
            return Result<string>.Failure(
                new List<string> { "Assigned user must be an Employee." },
                "Invalid Role"
            );
        }

        task.EmployeeId = request.EmployeeId.ToString();

        await taskRepository.UpdateAsync(task);

        return Result<string>.Success("Employee assigned to task successfully");

    }
}
