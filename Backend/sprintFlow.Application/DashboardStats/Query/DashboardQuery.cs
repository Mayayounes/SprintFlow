using MediatR;
using sprintFlow.Application.DashboardStats.Dto;

namespace sprintFlow.Application.DashboardStats.Query;

public class GetDashboardStatsQuery : IRequest<DashboardStatsDto>
{
}