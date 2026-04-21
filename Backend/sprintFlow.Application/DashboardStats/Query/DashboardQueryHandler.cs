using MediatR;
using sprintFlow.Application.DashboardStats.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.DashboardStats.Query;

public class DashboardQueryHandler(IUserRepository userRepository , IProjectRepository projectRepository , IUserContext userContext) : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        var isLeader = await userRepository.IsUserInRoleAsync(
            Guid.Parse(currentUser.Id),
            UserRole.Leader
        );

        var roleCounts = await userRepository.CountUsersByRoleAsync();
        int projectsCount;

        if (isLeader)
        {
            projectsCount = await projectRepository.CountByManagerIdAsync(currentUser.Id);
        }
        else
        {
            projectsCount = await projectRepository.CountAllProjectsAsync();
        }

        return new DashboardStatsDto
        {
            Users = roleCounts.GetValueOrDefault("Admin", 0)
                   + roleCounts.GetValueOrDefault("Leader", 0)
                   + roleCounts.GetValueOrDefault("Employee", 0),

            Admins = roleCounts.GetValueOrDefault("Admin", 0),
            Leaders = roleCounts.GetValueOrDefault("Leader", 0),
            Employees = roleCounts.GetValueOrDefault("Employee", 0),

            Projects = projectsCount
        };
    }
}
