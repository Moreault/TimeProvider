namespace ToolBX.TimeProvider.Tests.Customizations;

public sealed class DateTimeCustomization : CustomizationBase<DateTime>
{
    public override IDummyBuilder<DateTime> Build(IDummy dummy)
    {
        return dummy.Build<DateTime>().FromFactory(() =>
        {
            var year = PseudoRandomNumberGenerator.Shared.Generate(1900, 2200);
            var month = PseudoRandomNumberGenerator.Shared.Generate(1, 12);
            var day = PseudoRandomNumberGenerator.Shared.Generate(1, 28);
            var hour = PseudoRandomNumberGenerator.Shared.Generate(0, 23);
            var minute = PseudoRandomNumberGenerator.Shared.Generate(0, 59);
            var second = PseudoRandomNumberGenerator.Shared.Generate(0, 59);
            var millisecond = PseudoRandomNumberGenerator.Shared.Generate(0, 999);

            return new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Unspecified);
        });
    }
}