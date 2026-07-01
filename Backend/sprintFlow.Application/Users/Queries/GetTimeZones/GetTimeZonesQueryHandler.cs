using MediatR;
using sprintFlow.Application.Common;
using sprintFlow.Application.Users.Dto;

namespace sprintFlow.Application.Users.Queries.GetTimeZones;

public class GetTimeZonesQueryHandler
    : IRequestHandler<GetTimeZonesQuery, Result<List<TimeZoneDto>>>
{
    public Task<Result<List<TimeZoneDto>>> Handle(
        GetTimeZonesQuery request,
        CancellationToken cancellationToken)
    {
        var timeZones = TimeZoneInfo
            .GetSystemTimeZones()
            .Select(tz => new TimeZoneDto
            {
                Id = tz.Id,
                DisplayName = tz.DisplayName
            })
            .OrderBy(t => t.DisplayName)
            .ToList();

        return Task.FromResult(
            Result<List<TimeZoneDto>>.Success(
                timeZones,
                "Time zones retrieved successfully"));
    }
}