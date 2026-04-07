using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;
using sprintFlow.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace sprintFlow.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler(IUserContext userContext, IMapper mapper, IProjectRepository projectRepository, ITaskRepository taskRepository, IUserRepository userRepository) : IRequestHandler<CreateTaskCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null) {
            return Result<Guid>.Failure(new List<string> { "Current User not found" });
        }
        var ManagerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);
        if (currentUser.Id != ManagerId || ManagerId == null)
            return Result<Guid>.Failure(new List<string> { "You are not authorized to create a task for this project." });

        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
            return Result<Guid>.Failure(new List<string> { "Project Not Found" });

        var isEmployee = await userRepository.IsUserInRoleAsync(request.EmployeeId, UserRole.Employee);
        if (!isEmployee)
            return Result<Guid>.Failure(new List<string> { "Assigned user must be an Employee." });

        var task = mapper.Map<TaskItem>(request);
        var taskId = await taskRepository.Create(task);

        return Result<Guid>.Success(taskId, "Task created successfully");
    }
}
