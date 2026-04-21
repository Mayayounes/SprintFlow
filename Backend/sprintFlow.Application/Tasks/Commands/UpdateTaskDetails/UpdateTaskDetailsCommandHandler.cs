using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Commands.UpdateTaskDetails;

public class UpdateTaskDetailsCommandHandler(IUserContext userContext, IMapper mapper, IProjectRepository projectRepository, ITaskRepository taskRepository) : IRequestHandler<UpdateTaskDetailsCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpdateTaskDetailsCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
        {
            return Result<Guid>.Failure(new List<string> { "Current user not found" });
        }

        var ManagerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);
        if (ManagerId == null || currentUser.Id != ManagerId)
        {
            return Result<Guid>.Failure(
                new List<string> { "You are not authorized to update information of this task." }
            );
        }
        var task = await taskRepository.GetByIdAsync(request.TaskId);
        if (task == null)
        {
            return Result<Guid>.Failure(new List<string> { "Task not found." });
        }
        mapper.Map(request, task);

        await taskRepository.UpdateAsync(task);

        return Result<Guid>.Success(task.Id, "Task updated successfully");
    }
}
