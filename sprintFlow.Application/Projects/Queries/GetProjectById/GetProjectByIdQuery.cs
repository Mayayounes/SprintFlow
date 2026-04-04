using MediatR;
using sprintFlow.Application.Projects.Dto;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQuery : IRequest<ProjectDto>
{
    public Guid Id { get; }
    public GetProjectByIdQuery(Guid id)
    {
        Id = id;
    }
        
}
