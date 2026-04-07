using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper, IUserContext userContext) : IRequestHandler<UpdateTaskCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
            return Result<Guid>.Failure(new List<string> { "Task not found." });
        var EmployeeId = task.EmployeeId;
        var currentUser = userContext.GetCurrentUser();
        if (currentUser.Id != EmployeeId)
            return Result<Guid>.Failure(new List<string> { "You are not authorized to update this task." });
        task.Status = request.Status;
        await taskRepository.UpdateAsync(task);
        return Result<Guid>.Success(task.Id, "Task updated successfully");

    }
}
