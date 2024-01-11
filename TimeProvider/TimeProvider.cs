namespace ToolBX.TimeProvider;

public static class TimeProvider
{
    public static DateTimeOffset Now
    {
        get
        {
            if (!_difference.HasValue && !_frozenTime.HasValue) return DateTimeOffset.Now;

            if (_frozenTime.HasValue) return _frozenTime.Value;

            var difference = _difference!.Value;
            var now = DateTimeOffset.Now;

            return now.AddYears(difference.Year)
                .AddMonths(difference.Month)
                .AddDays(difference.Day)
                .AddHours(difference.Hour)
                .AddMinutes(difference.Minute)
                .AddSeconds(difference.Second)
                .AddMilliseconds(difference.Millisecond);
        }
    }

    public static DateTime Today => Now.Date;

    private static DateDifference? _difference;

    private static DateTimeOffset? _frozenTime;

    public static bool IsOverriden => _difference.HasValue || _frozenTime.HasValue;

    public static bool IsFrozen => _frozenTime.HasValue;

    /// <summary>
    /// Overrides current time with the specified value.
    /// </summary>
    public static void Override(DateTime date) => Override((DateTimeOffset)date);

    /// <summary>
    /// Overrides current time with the specified value.
    /// </summary>
    public static void Override(DateTimeOffset date)
    {
        var now = DateTimeOffset.Now;

        _difference = new DateDifference
        {
            Year = date.Year - now.Year,
            Month = date.Month - now.Month,
            Day = date.Day - now.Day,
            Hour = date.Hour - now.Hour,
            Minute = date.Minute - now.Minute,
            Second = date.Second - now.Second,
            Millisecond = date.Millisecond - now.Millisecond
        };
    }

    /// <summary>
    /// Freezes current time in place until <see cref="Reset"/> is used.
    /// </summary>
    public static DateTimeOffset Freeze() => Freeze(Now);

    /// <summary>
    /// Freezes specified time in place until <see cref="Reset"/> is used.
    /// </summary>
    public static DateTimeOffset Freeze(DateTime date) => Freeze((DateTimeOffset)date);

    /// <summary>
    /// Freezes specified time in place until <see cref="Reset"/> is used.
    /// </summary>
    public static DateTimeOffset Freeze(DateTimeOffset date)
    {
        Override(date);
        _frozenTime = date;
        return date;
    }

    /// <summary>
    /// Removes any active time overrides.
    /// </summary>
    public static void Reset()
    {
        _difference = null;
        _frozenTime = null;
    }

    public static void Unfreeze()
    {
        _frozenTime = null;
    }

    public static void AddYears(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddYearsToCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddYears(value));
        else
            Override(Now.AddYears(value));
    }

    public static void SubtractYears(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotSubtractYearsFromCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddYears(-value));
        else
            Override(Now.AddYears(-value));

    }

    public static void AddMonths(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddMonthsToCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddMonths(value));
        else
            Override(Now.AddMonths(value));
    }

    public static void SubtractMonths(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotSubtractMonthsFromCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddMonths(-value));
        else
            Override(Now.AddMonths(-value));
    }

    public static void AddDays(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddDaysToCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddDays(value));
        else
            Override(Now.AddDays(value));
    }

    public static void SubtractDays(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotSubtractDaysFromCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddDays(-value));
        else
            Override(Now.AddDays(-value));
    }

    public static void AddHours(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddHoursToCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddHours(value));
        else
            Override(Now.AddHours(value));
    }

    public static void SubtractHours(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotSubtractHoursFromCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddHours(-value));
        else
            Override(Now.AddHours(-value));
    }

    public static void AddMinutes(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddMinutesToCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddMinutes(value));
        else
            Override(Now.AddMinutes(value));
    }

    public static void SubtractMinutes(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotSubtractMinutesFromCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddMinutes(-value));
        else
            Override(Now.AddMinutes(-value));
    }

    public static void AddSeconds(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddSecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddSeconds(value));
        else
            Override(Now.AddSeconds(value));
    }

    public static void SubtractSeconds(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotSubtractSecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddSeconds(-value));
        else
            Override(Now.AddSeconds(-value));
    }

    public static void AddMilliseconds(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotAddMillisecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddMilliseconds(value));
        else
            Override(Now.AddMilliseconds(value));
    }

    public static void SubtractMilliseconds(int value)
    {
        if (value <= 0) throw new ArgumentException(string.Format(Exceptions.CannotSubtractMillisecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        if (IsFrozen)
            Freeze(Now.AddMilliseconds(-value));
        else
            Override(Now.AddMilliseconds(-value));
    }
}
