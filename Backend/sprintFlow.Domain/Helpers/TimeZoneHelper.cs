namespace sprintFlow.Domain.Helpers;
public static class TimeZoneHelper
{
    public static DateTime ToUserTime(DateTime utc, string timeZoneId)
    {
        if (utc.Kind != DateTimeKind.Utc)
        {
            utc = DateTime.SpecifyKind(utc, DateTimeKind.Utc);
        }

        try
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            return TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
        }
        catch (TimeZoneNotFoundException)
        {
            return utc;
        }
        catch (InvalidTimeZoneException)
        {
            return utc;
        }
    }
}