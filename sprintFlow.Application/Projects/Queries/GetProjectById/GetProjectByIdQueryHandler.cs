using AutoMapper;
using MediatR;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Exceptions;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(IProjectRepository projectRepository , IMapper mapper , IUserRepository userRepository , IUserContext userContext) : IRequestHandler<GetProjectByIdQuery,SingleProjectDto>
{
    public async Task<SingleProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id);
        if(project == null)
        {
            throw new Exception("Project not found");
        }
        var currentUser = userContext.GetCurrentUser();

        var isAdmin = await userRepository.IsUserInRoleAsync(Guid.Parse(currentUser.Id), UserRole.Admin);
        var isLeader = await userRepository.IsUserInRoleAsync(Guid.Parse(currentUser.Id), UserRole.Leader);
        if (!isAdmin && !(isLeader && project.ManagerId == currentUser.Id))
        {
            throw new NotAuthorizedException("User", "see this project");
        }
        var singleProjectDto = mapper.Map<SingleProjectDto>(project);
        return singleProjectDto;
    }
}
