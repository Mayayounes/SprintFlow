public static class TimeZoneHelper
{
    public static DateTime ToUserTime(DateTime utc, string timeZoneId)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        return TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(utc, DateTimeKind.Utc),
            tz
        );
    }
}