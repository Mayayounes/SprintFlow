using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Queries.GetByIdForProject;

public class GetByIdForProjectQueryHandler(IUserContext userContext,IMapper mapper , IProjectRepository projectRepository) : IRequestHandler<GetByIdForProjectQuery, Result<TaskItemDto>>
{
    public async Task<Result<TaskItemDto>> Handle(GetByIdForProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if(project == null)
        {
            return Result<TaskItemDto>.Failure(
                new List<string> { "Project not found." }
            );
        }
        var task = project.Tasks.FirstOrDefault(t => t.Id == request.TaskId);
        if(task == null)
        {
            return Result<TaskItemDto>.Failure(
            new List<string> { "Task not found." }
        );
        }
        var currentUser = userContext.GetCurrentUser();
        var ManagerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);
        if (currentUser.Id != ManagerId)
        {

            return Result<TaskItemDto>.Failure(
                new List<string> { "You are not authorized to view this Task" }
            );
        }
        var taskDto = mapper.Map<TaskItemDto>(task);
        return Result<TaskItemDto>.Success(taskDto, "Task retrieved successfully");

    }
}
