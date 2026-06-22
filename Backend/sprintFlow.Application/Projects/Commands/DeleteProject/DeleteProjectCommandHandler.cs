using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler(IProjectRepository projectRepository , IUserContext userContext): IRequestHandler<DeleteProjectCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteProjectCommand request, CancellationToken ct)
    {
        var project = await projectRepository.GetByIdAsync(request.ProjectId);

        if (project == null)
            return Result<bool>.Failure(
                new List<string> { "Project not found" },
                "Delete failed",
                false
            );
        var currentUser = userContext.GetCurrentUser();
        var userId = Guid.Parse(currentUser!.Id);
        if (project.ManagerId != userId.ToString())
        {
            return Result<bool>.Failure(
                new List<string> { "Unauthorized" },
                "Only project manager can delete this project",
                false
            );
        }
        await projectRepository.Delete(project);

        await projectRepository.SaveChangesSafe();

        return Result<bool>.Success(true, "Project deleted successfully");
    }
}