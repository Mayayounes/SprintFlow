using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(IProjectRepository projectRepository, IMapper mapper, IUserRepository userRepository, IUserContext userContext) : IRequestHandler<GetProjectByIdQuery, Result<SingleProjectDto>>
{
    public async Task<Result<SingleProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id);
        if (project == null)
        {
            return Result<SingleProjectDto>.Failure(
                new List<string> { "Project not found" },
                "Not Found"
            );
        }
        var currentUser = userContext.GetCurrentUser();

        var isAdmin = await userRepository.IsUserInRoleAsync(Guid.Parse(currentUser.Id), UserRole.Admin);
        var isLeader = await userRepository.IsUserInRoleAsync(Guid.Parse(currentUser.Id), UserRole.Leader);
        if (!isAdmin && !(isLeader && project.ManagerId == currentUser.Id))
        {
            return Result<SingleProjectDto>.Failure(
                new List<string> { "You are not authorized to view this project" },
                "Forbidden"
            );
        }
        var singleProjectDto = mapper.Map<SingleProjectDto>(project);
        return Result<SingleProjectDto>.Success(singleProjectDto, "Project retrieved successfully");
    }
}
