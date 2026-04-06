using AutoMapper;
using MediatR;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Queries.GetTasksForProject;

public class GetTaskForProjectQueryHandler(IUserContext userContext ,IProjectRepository projectRepository , IMapper mapper) : IRequestHandler<GetTaskForProjectQuery, IEnumerable<TaskItemDto>>
{
    public async Task<IEnumerable<TaskItemDto>> Handle(GetTaskForProjectQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var ManagerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);
        if (currentUser.Id != ManagerId)
            throw new NotAuthorizedException("User", "See Tasks for project he didnt manage");

        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
            throw new NotFoundException(nameof(Project), request.ProjectId.ToString());
        var results = mapper.Map<IEnumerable<TaskItemDto>>(project.Tasks);
        return results;
    }
}
