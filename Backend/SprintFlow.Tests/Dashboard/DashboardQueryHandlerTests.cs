using FluentAssertions;
using Moq;
using sprintFlow.Application.DashboardStats.Query;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Repositories;

namespace SprintFlow.Tests.Dashboard;

public class DashboardQueryHandlerTests
{
    private readonly Guid _adminId = Guid.NewGuid();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IProjectRepository> _projectRepository = new();
    private readonly Mock<ITaskRepository> _taskRepository = new();
    private readonly Mock<IUserContext> _userContext = new();

    public DashboardQueryHandlerTests()
    {
        _userContext
            .Setup(x => x.GetCurrentUser())
            .Returns(new CurrentUser(_adminId.ToString(), "admin@sprintflow.test", [nameof(UserRole.Admin)]));

        _userRepository
            .Setup(x => x.IsUserInRoleAsync(_adminId, UserRole.Leader))
            .ReturnsAsync(false);

        _userRepository
            .Setup(x => x.CountUsersByRoleAsync())
            .ReturnsAsync(new Dictionary<string, int>
            {
                ["Admin"] = 1,
                ["Leader"] = 1,
                ["Employee"] = 2
            });

        _projectRepository
            .Setup(x => x.GetAllWithTasksAsync())
            .ReturnsAsync([]);
    }

    [Fact]
    public async Task Handle_BeforeLastDayOfMonth_UsesPreviousMonthForEmployeeOfMonth()
    {
        var mayWinner = Employee("May Winner");
        var juneWinner = Employee("June Winner");

        _taskRepository
            .Setup(x => x.GetAssignedTasksWithEmployeesAsync())
            .ReturnsAsync([
                CompletedTask(mayWinner, new DateOnly(2026, 5, 4), new DateTime(2026, 5, 10, 10, 0, 0), new DateOnly(2026, 5, 10)),
                CompletedTask(juneWinner, new DateOnly(2026, 6, 1), new DateTime(2026, 6, 2, 10, 0, 0), new DateOnly(2026, 6, 2)),
                CompletedTask(juneWinner, new DateOnly(2026, 6, 2), new DateTime(2026, 6, 3, 10, 0, 0), new DateOnly(2026, 6, 3))
            ]);

        var result = await CreateHandler(new DateTimeOffset(2026, 6, 15, 12, 0, 0, TimeSpan.Zero))
            .Handle(new GetDashboardStatsQuery(), CancellationToken.None);

        result.EmployeeOfMonthLabel.Should().Be("Employee of May");
        result.EmployeeOfMonth!.EmployeeName.Should().Be("May Winner");
        result.TopEmployees.First().EmployeeName.Should().Be("June Winner");
    }

    [Fact]
    public async Task Handle_OnLastDayOfMonth_UsesCurrentMonthForEmployeeOfMonth()
    {
        var mayWinner = Employee("May Winner");
        var juneWinner = Employee("June Winner");

        _taskRepository
            .Setup(x => x.GetAssignedTasksWithEmployeesAsync())
            .ReturnsAsync([
                CompletedTask(mayWinner, new DateOnly(2026, 5, 4), new DateTime(2026, 5, 10, 10, 0, 0), new DateOnly(2026, 5, 10)),
                CompletedTask(juneWinner, new DateOnly(2026, 6, 1), new DateTime(2026, 6, 2, 10, 0, 0), new DateOnly(2026, 6, 2)),
                CompletedTask(juneWinner, new DateOnly(2026, 6, 2), new DateTime(2026, 6, 3, 10, 0, 0), new DateOnly(2026, 6, 3))
            ]);

        var result = await CreateHandler(new DateTimeOffset(2026, 6, 30, 12, 0, 0, TimeSpan.Zero))
            .Handle(new GetDashboardStatsQuery(), CancellationToken.None);

        result.EmployeeOfMonthLabel.Should().Be("Employee of June");
        result.EmployeeOfMonth!.EmployeeName.Should().Be("June Winner");
    }

    [Fact]
    public async Task Handle_UsesCompletionStatusForMonthlyTimingCounts()
    {
        var employee = Employee("Timing Tester");

        _taskRepository
            .Setup(x => x.GetAssignedTasksWithEmployeesAsync())
            .ReturnsAsync([
                CompletedTask(employee, new DateOnly(2026, 6, 1), new DateTime(2026, 6, 3, 10, 0, 0), new DateOnly(2026, 6, 4)),
                CompletedTask(employee, new DateOnly(2026, 6, 1), new DateTime(2026, 6, 4, 10, 0, 0), new DateOnly(2026, 6, 4)),
                CompletedTask(employee, new DateOnly(2026, 6, 1), new DateTime(2026, 6, 5, 10, 0, 0), new DateOnly(2026, 6, 4))
            ]);

        var result = await CreateHandler(new DateTimeOffset(2026, 6, 30, 12, 0, 0, TimeSpan.Zero))
            .Handle(new GetDashboardStatsQuery(), CancellationToken.None);

        result.EarlyTasksThisMonth.Should().Be(1);
        result.OnTimeTasksThisMonth.Should().Be(1);
        result.LateTasksThisMonth.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WhenNoCompletedTasks_ReturnsEmptyEmployeeAnalytics()
    {
        var employee = Employee("No Completions");

        _taskRepository
            .Setup(x => x.GetAssignedTasksWithEmployeesAsync())
            .ReturnsAsync([
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Todo",
                    EmployeeId = employee.Id,
                    Employee = employee,
                    AssignedDate = new DateOnly(2026, 6, 1),
                    Deadline = new DateOnly(2026, 6, 5),
                    Status = TaskItemStatus.ToDo
                }
            ]);

        var result = await CreateHandler(new DateTimeOffset(2026, 6, 30, 12, 0, 0, TimeSpan.Zero))
            .Handle(new GetDashboardStatsQuery(), CancellationToken.None);

        result.EmployeeOfMonth.Should().BeNull();
        result.TopEmployees.Should().BeEmpty();
        result.AssignedTasksThisMonth.Should().Be(1);
    }

    [Fact]
    public async Task Handle_PreservesExistingProjectAndUserCounts()
    {
        _projectRepository
            .Setup(x => x.GetAllWithTasksAsync())
            .ReturnsAsync([
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Done project",
                    Tasks =
                    [
                        new TaskItem { Title = "Done", Status = TaskItemStatus.Done }
                    ]
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Pending project",
                    Tasks =
                    [
                        new TaskItem { Title = "Todo", Status = TaskItemStatus.ToDo }
                    ]
                }
            ]);

        _taskRepository
            .Setup(x => x.GetAssignedTasksWithEmployeesAsync())
            .ReturnsAsync([]);

        var result = await CreateHandler(new DateTimeOffset(2026, 6, 30, 12, 0, 0, TimeSpan.Zero))
            .Handle(new GetDashboardStatsQuery(), CancellationToken.None);

        result.Users.Should().Be(4);
        result.Projects.Should().Be(2);
        result.DoneProjects.Should().Be(1);
        result.PendingProjects.Should().Be(1);
    }

    private DashboardQueryHandler CreateHandler(DateTimeOffset utcNow)
    {
        return new DashboardQueryHandler(
            _userRepository.Object,
            _projectRepository.Object,
            _taskRepository.Object,
            _userContext.Object,
            new FixedTimeProvider(utcNow));
    }

    private static User Employee(string name)
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = name,
            Email = $"{name.Replace(" ", ".").ToLowerInvariant()}@sprintflow.test"
        };
    }

    private static TaskItem CompletedTask(User employee, DateOnly assignedDate, DateTime completedAt, DateOnly deadline)
    {
        return new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Completed task",
            EmployeeId = employee.Id,
            Employee = employee,
            AssignedDate = assignedDate,
            Deadline = deadline,
            CompletedAt = completedAt,
            Status = TaskItemStatus.Done
        };
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }
}
