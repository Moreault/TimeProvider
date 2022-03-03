using System;

namespace TimeProvider.Tests;

internal static class DateTimeExtensions
{
    internal static DateTime TrimMilliseconds(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0, date.Kind);
    }

    internal static DateTime TrimMilliseconds(this DateTimeOffset date)
    {
        return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0, DateTimeKind.Utc);
    }

}