using MediatR;
using sprintFlow.Application.DashboardStats.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.DashboardStats.Query;

public class DashboardQueryHandler(IUserRepository userRepository,IProjectRepository projectRepository,ITaskRepository taskRepository,IUserContext userContext,TimeProvider timeProvider) : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
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
        var today = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        var currentMonthStart = new DateOnly(today.Year, today.Month, 1);
        var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);
        var awardMonthStart = today == currentMonthEnd
            ? currentMonthStart
            : currentMonthStart.AddMonths(-1);
        var awardMonthEnd = awardMonthStart.AddMonths(1).AddDays(-1);

        var assignedTasks = isLeader
            ? []
            : await taskRepository.GetAssignedTasksWithEmployeesAsync();

        var currentMonthAnalytics = BuildEmployeePerformance(assignedTasks, currentMonthStart, currentMonthEnd);
        var awardMonthAnalytics = BuildEmployeePerformance(assignedTasks, awardMonthStart, awardMonthEnd);
        var completedTasksThisMonth = assignedTasks
            .Where(t => IsCompletedInMonth(t, currentMonthStart, currentMonthEnd))
            .ToList();

        return new DashboardStatsDto
        {
            Users = totalUsers,
            Admins = roleCounts.GetValueOrDefault("Admin", 0),
            Leaders = roleCounts.GetValueOrDefault("Leader", 0),
            Employees = roleCounts.GetValueOrDefault("Employee", 0),

            Projects = projectsCount,
            DoneProjects = doneProjects,
            PendingProjects = pendingProjects,

            EmployeeOfMonthLabel = $"Employee of {awardMonthStart:MMMM}",
            EmployeeAnalyticsMonthLabel = currentMonthStart.ToString("MMMM yyyy"),
            AssignedTasksThisMonth = assignedTasks.Count(t => IsDateInRange(t.AssignedDate, currentMonthStart, currentMonthEnd)),
            CompletedTasksThisMonth = completedTasksThisMonth.Count,
            EarlyTasksThisMonth = completedTasksThisMonth.Count(t => t.CompletionStatus == TaskCompletionStatus.Early),
            OnTimeTasksThisMonth = completedTasksThisMonth.Count(t => t.CompletionStatus == TaskCompletionStatus.OnTime),
            LateTasksThisMonth = completedTasksThisMonth.Count(t => t.CompletionStatus == TaskCompletionStatus.Late),
            EmployeeOfMonth = awardMonthAnalytics.FirstOrDefault(),
            TopEmployees = currentMonthAnalytics.Take(5).ToList()
        };
    }

    private static List<EmployeePerformanceDto> BuildEmployeePerformance(IEnumerable<TaskItem> tasks,DateOnly periodStart,DateOnly periodEnd)
    {
        return tasks
            .Where(t => !string.IsNullOrWhiteSpace(t.EmployeeId))
            .GroupBy(t => new
            {
                EmployeeId = t.EmployeeId!,
                Name = t.Employee?.UserName ?? t.Employee?.Email ?? "Unknown employee",
                Email = t.Employee?.Email ?? string.Empty
            })
            .Select(group =>
            {
                var assignedTasks = group.Count(t => IsDateInRange(t.AssignedDate, periodStart, periodEnd));
                var completedTasks = group.Where(t => IsCompletedInMonth(t, periodStart, periodEnd)).ToList();
                var earlySubmissions = completedTasks.Count(t => t.CompletionStatus == TaskCompletionStatus.Early);
                var onTimeSubmissions = completedTasks.Count(t => t.CompletionStatus == TaskCompletionStatus.OnTime);
                var lateSubmissions = completedTasks.Count(t => t.CompletionStatus == TaskCompletionStatus.Late);

                return new EmployeePerformanceDto
                {
                    EmployeeId = group.Key.EmployeeId,
                    EmployeeName = group.Key.Name,
                    EmployeeEmail = group.Key.Email,
                    AssignedTasks = assignedTasks,
                    CompletedTasks = completedTasks.Count,
                    EarlySubmissions = earlySubmissions,
                    OnTimeSubmissions = onTimeSubmissions,
                    LateSubmissions = lateSubmissions,
                    CompletionRate = assignedTasks == 0 ? 0 : Math.Round(completedTasks.Count * 100d / assignedTasks, 1),
                    OnTimeRate = completedTasks.Count == 0 ? 0 : Math.Round((earlySubmissions + onTimeSubmissions) * 100d / completedTasks.Count, 1),
                    Score = completedTasks.Count * 10 + earlySubmissions * 4 + onTimeSubmissions * 2 - lateSubmissions * 3
                };
            })
            .Where(employee => employee.CompletedTasks > 0)
            .OrderByDescending(employee => employee.Score)
            .ThenByDescending(employee => employee.EarlySubmissions)
            .ThenByDescending(employee => employee.OnTimeRate)
            .ThenBy(employee => employee.EmployeeName)
            .ToList();
    }

    private static bool IsCompletedInMonth(TaskItem task, DateOnly periodStart, DateOnly periodEnd)
    {
        return task.CompletedAt is not null
            && IsDateInRange(DateOnly.FromDateTime(task.CompletedAt.Value), periodStart, periodEnd);
    }

    private static bool IsDateInRange(DateOnly date, DateOnly periodStart, DateOnly periodEnd)
    {
        return date >= periodStart && date <= periodEnd;
    }
}
