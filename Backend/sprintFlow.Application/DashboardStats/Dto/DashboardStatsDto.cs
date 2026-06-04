namespace sprintFlow.Application.DashboardStats.Dto;

public class DashboardStatsDto
{
    public int Users { get; set; }
    public int Admins { get; set; }
    public int Leaders { get; set; }
    public int Employees { get; set; }
    public int Projects { get; set; }
    public int PendingProjects { get; set; }
    public int DoneProjects { get; set; }
    public string EmployeeOfMonthLabel { get; set; } = string.Empty;
    public string EmployeeAnalyticsMonthLabel { get; set; } = string.Empty;
    public int AssignedTasksThisMonth { get; set; }
    public int CompletedTasksThisMonth { get; set; }
    public int EarlyTasksThisMonth { get; set; }
    public int OnTimeTasksThisMonth { get; set; }
    public int LateTasksThisMonth { get; set; }
    public EmployeePerformanceDto? EmployeeOfMonth { get; set; }
    public List<EmployeePerformanceDto> TopEmployees { get; set; } = [];
}

public class EmployeePerformanceDto
{
    public string EmployeeId { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeEmail { get; set; } = string.Empty;
    public int AssignedTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int EarlySubmissions { get; set; }
    public int OnTimeSubmissions { get; set; }
    public int LateSubmissions { get; set; }
    public double CompletionRate { get; set; }
    public double OnTimeRate { get; set; }
    public int Score { get; set; }
}
