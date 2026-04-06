using AutoMapper;
using MediatR;
using sprintFlow.Application.Tasks.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Exceptions;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Tasks.Queries.GetByIdForProject;

public class GetByIdForProjectQueryHandler(IUserContext userContext,IMapper mapper , IProjectRepository projectRepository) : IRequestHandler<GetByIdForProjectQuery, TaskItemDto>
{
    public async Task<TaskItemDto> Handle(GetByIdForProjectQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var ManagerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);
        if (currentUser.Id != ManagerId)
            throw new NotAuthorizedException("User", "See Task for project he didnt manage");

        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if(project == null)
        {
            throw new Exception("Project not found");
        }
        var task = project.Tasks.FirstOrDefault(t => t.Id == request.TaskId);
        if(task == null)
        {
            throw new Exception("Task not found");
        }
        var result = mapper.Map<TaskItemDto>(task);
        return result;
    }
}
