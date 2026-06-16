//using sprintFlow.Domain.Repositories;
//using sprintFlow.Infrastructure.Services;

//public class TaskDeadlineJob(ITaskRepository taskRepository , NotificationService notificationService)
//{
//    public async Task Execute()
//    {
//        var now = DateTime.UtcNow;

//        var tasks = await taskRepository.GetActiveAssignedTasksAsync();

//        foreach (var task in tasks)
//        {
//            var deadline = task.Deadline.ToDateTime(TimeOnly.MinValue);
//            var remaining = deadline - now;

//            if (remaining <= TimeSpan.Zero)
//            {
//                var userId = Guid.Parse(task.EmployeeId);

//                task.EmployeeId = null;
//                await taskRepository.UpdateAsync(task);

//                await notificationService.SendAsync(
//                    userId,
//                    $"❌ Task '{task.Title}' has expired and you were unassigned."
//                );

//                continue;
//            }

//            if (remaining <= TimeSpan.FromHours(1))
//            {
//                await notificationService.SendAsync(
//                    Guid.Parse(task.EmployeeId),
//                    $"⚠️ Only {remaining.Minutes} minutes left for task '{task.Title}'"
//                );
//            }
//            else if (remaining <= TimeSpan.FromHours(24))
//            {
//                await notificationService.SendAsync(
//                    Guid.Parse(task.EmployeeId),
//                    $"📌 Less than 24 hours left for task '{task.Title}'"
//                );
//            }
//        }
//    }
////}