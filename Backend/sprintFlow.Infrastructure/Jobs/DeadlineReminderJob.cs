using Microsoft.Extensions.Logging;
using Quartz;
using sprintFlow.Application.Common.Interfaces;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Infrastructure.Jobs;

public class DeadlineReminderJob(ILogger<DeadlineReminderJob> logger,INotificationService notificationService,ITaskRepository taskRepository,IUnitOfWork unitOfWork): IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("DeadlineReminderJob executed at {Time}",
            DateTime.UtcNow);

        var tasks = await taskRepository.GetTasksDueTomorrowAsync();

        foreach (var task in tasks)
        {
            if (string.IsNullOrWhiteSpace(task.EmployeeId))
                continue;

            if (!Guid.TryParse(task.EmployeeId, out var userId))
                continue;

            var notification = await notificationService.CreateAsync(
                userId,
                $"Task '{task.Title}' is due tomorrow.");

            await unitOfWork.SaveChangesAsync();

            await notificationService.PublishAsync(notification);
        }
    }
}