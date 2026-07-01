using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;

namespace sprintFlow.Application.Users.Queries.GetTimeZones;

public record GetTimeZonesQuery() : IRequest<Result<List<TimeZoneDto>>>;