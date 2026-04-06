using AutoMapper;
using MediatR;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Constants;
using sprintFlow.Domain.Entities;
using sprintFlow.Domain.Exceptions;
using sprintFlow.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace sprintFlow.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler(IUserContext userContext ,IMapper mapper, IProjectRepository projectRepository, ITaskRepository taskRepository, IUserRepository userRepository) : IRequestHandler<CreateTaskCommand, Guid>
{
    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var ManagerId = await projectRepository.GetProjectManagerIdAsync(request.ProjectId);
        if (currentUser.Id != ManagerId)
            throw new NotAuthorizedException("User", "Create a Task");

        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if (project is null)
            throw new NotFoundException(nameof(TaskItem), request.ProjectId.ToString());
        
        var isEmployee = await userRepository.IsUserInRoleAsync(request.EmployeeId, UserRole.Employee);
        if (!isEmployee)
            throw new ValidationException("Assigned user must have Employee role.");
        var task = mapper.Map<TaskItem>(request);
        return await taskRepository.Create(task);
    }
}
