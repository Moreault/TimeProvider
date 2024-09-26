namespace ToolBX.TimeProvider;

internal readonly record struct DateDifference
{
    public int Year { get; init; }
    public int Month { get; init; }
    public int Day { get; init; }
    public int Hour { get; init; }
    public int Minute { get; init; }
    public int Second { get; init; }
    public int Millisecond { get; init; }
    public int Microsecond { get; init; }
    public TimeSpan Offset { get; init; }

    public TimeSpan NewOffset { get; init; }
    public TimeSpan OldOffset { get; init; }

}