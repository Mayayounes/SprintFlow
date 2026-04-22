using MediatR;
using sprintFlow.Application.DashboardStats.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.DashboardStats.Query;

public class DashboardQueryHandler(IUserRepository userRepository , IProjectRepository projectRepository , IUserContext userContext) : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        var userId = Guid.Parse(currentUser!.Id);

        var isLeader = await userRepository.IsUserInRoleAsync(userId, UserRole.Leader);

        var roleCounts = await userRepository.CountUsersByRoleAsync();

        var totalUsers =
            roleCounts.GetValueOrDefault("Admin", 0) +
            roleCounts.GetValueOrDefault("Leader", 0) +
            roleCounts.GetValueOrDefault("Employee", 0);

        var projects = isLeader
            ? await projectRepository.GetByManagerIdWithTasksAsync(currentUser.Id)
            : await projectRepository.GetAllWithTasksAsync();

        var projectsCount = projects.Count;

        var doneProjects = projects.Count(p =>
            p.Tasks.Any() &&
            p.Tasks.All(t => t.Status == TaskItemStatus.Done)
        );

        var pendingProjects = projectsCount - doneProjects;

        return new DashboardStatsDto
        {
            Users = totalUsers,
            Admins = roleCounts.GetValueOrDefault("Admin", 0),
            Leaders = roleCounts.GetValueOrDefault("Leader", 0),
            Employees = roleCounts.GetValueOrDefault("Employee", 0),

            Projects = projectsCount,
            DoneProjects = doneProjects,
            PendingProjects = pendingProjects
        };
    }
}
