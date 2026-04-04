using AutoMapper;
using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Application.Users;
using sprintFlow.Domain.Repositories;

namespace sprintFlow.Application.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler(IProjectRepository projectRepository , IMapper mapper) : IRequestHandler<GetAllProjectsQuery, PagedResults<ProjectDto>>
{
    public async Task<PagedResults<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var (projects, totalCount) = await projectRepository.GetAllMatchingAsync(request.SearchPhrase, request.PageNumber, request.PageSize);
        var projectsDto = mapper.Map<IEnumerable<ProjectDto>>(projects);

        var results = new PagedResults<ProjectDto>(projectsDto, totalCount, request.PageNumber, request.PageSize);
        return results;

    }
}
