using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Projects.Dto;

namespace sprintFlow.Application.Projects.Queries.GetAllProjects;

public class GetAllProjectsQuery : IRequest<PagedResults<ProjectDto>>
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public Guid ProjectId { get; set; }
}
