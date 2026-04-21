using MediatR;
using sprintFlow.Application.Common;

namespace sprintFlow.Application.Roles;

public record GetRolesQuery() : IRequest<Result<List<string>>>;