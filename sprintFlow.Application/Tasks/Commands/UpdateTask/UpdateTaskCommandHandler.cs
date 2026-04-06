using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler(ITaskRepository taskRepository , IMapper mapper, IUserContext userContext) : IRequestHandler<UpdateTaskCommand>
{
    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.TaskId) 
            ?? throw new NotFoundException(nameof(TaskItem),request.TaskId.ToString());
        var EmployeeId = task.EmployeeId;
        var currentUser = userContext.GetCurrentUser();
        if (currentUser.Id != EmployeeId)
            throw new NotAuthorizedException("User","Update the status");
        task.Status = request.Status;
        await taskRepository.UpdateAsync(task);
    }
}
