using AutoMapper;
using MediatR;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler(IProjectRepository projectRepository , IMapper mapper) : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetByIdAsync(request.Id);
        var projectDto = mapper.Map<ProjectDto>(project);
        return projectDto;
    }
}
