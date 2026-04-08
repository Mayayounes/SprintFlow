using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Queries.GetTasksForProject;

public class GetTaskForProjectQueryHandler(IUserContext userContext ,IProjectRepository projectRepository , IMapper mapper) : IRequestHandler<GetTaskForProjectQuery, Result<IEnumerable<TaskItemDto>>>
{
    public async Task<Result<IEnumerable<TaskItemDto>>> Handle(GetTaskForProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
        {
            return Result<IEnumerable<TaskItemDto>>.Failure(
                new List<string> { "Project not found." }
            );
        }
        var currentUser = userContext.GetCurrentUser();
        var ManagerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);
        if (currentUser.Id != ManagerId)
        {
            return Result<IEnumerable<TaskItemDto>>.Failure(
                new List<string> { "You are not authorized to view tasks for this project.." }
            );
        }
        var results = mapper.Map<IEnumerable<TaskItemDto>>(project.Tasks);
        return Result<IEnumerable<TaskItemDto>>.Success(results, "Tasks retrieved successfully");

    }
}
