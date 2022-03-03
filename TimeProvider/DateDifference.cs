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
}