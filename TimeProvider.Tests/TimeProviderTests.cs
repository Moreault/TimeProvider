namespace ToolBX.TimeProvider.Tests;

public abstract class TimeProviderTesterBase : Tester
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        TimeProvider.Reset();
    }
}

[TestClass]
public class TimeProviderTests
{
    [TestClass]
    public class Now : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenIsNotOverridenOrFrozen_ReturnActualSystemDate()
        {
            //Arrange
            var now = DateTimeOffset.Now.TrimMilliseconds();

            //Act
            var result = TimeProvider.Now.TrimMilliseconds();

            //Assert
            result.TrimMilliseconds().Should().Be(now.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsFrozenAndNotOverriden_ReturnFrozenTime()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            //Act
            var result = TimeProvider.Now;

            //Assert
            result.Should().Be(now);
        }

        [TestMethod]
        public void WhenIsNotFrozenAndOverriden_ReturnTimeDifferenceBetweenNowAndOverride()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Override(now);

            //Act
            var result = TimeProvider.Now;

            //Assert
            result.TrimMilliseconds().Should().Be(now.TrimMilliseconds());
        }
    }

    [TestClass]
    public class Today : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenIsNotOverridenOrFrozen_ReturnActualSystemDate()
        {
            //Arrange
            var today = DateTime.Today;

            //Act
            var result = TimeProvider.Today;

            //Assert
            result.Should().Be(today);
        }

        [TestMethod]
        public void WhenIsFrozenAndNotOverriden_ReturnFrozenTime()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            //Act
            var result = TimeProvider.Today;

            //Assert
            result.TrimMilliseconds().Should().Be(now.Date.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozenAndOverriden_ReturnTimeDifferenceBetweenNowAndOverride()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Override(now);

            //Act
            var result = TimeProvider.Today;

            //Assert
            result.TrimMilliseconds().Should().Be(now.Date.TrimMilliseconds());
        }
    }

    [TestClass]
    public class IsOverriden : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenIsNotOverriden_ReturnFalse()
        {
            //Arrange

            //Act
            var result = TimeProvider.IsOverriden;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsOverriden_ReturnTrue()
        {
            //Arrange
            TimeProvider.Override(Fixture.Create<DateTime>());

            //Act
            var result = TimeProvider.IsOverriden;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsFrozen_ReturnTrue()
        {
            //Arrange
            TimeProvider.Freeze();

            //Act
            var result = TimeProvider.IsOverriden;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class IsFrozen : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenIsNotFrozen_ReturnFalse()
        {
            //Arrange

            //Act
            var result = TimeProvider.IsFrozen;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsOverridenButNotFrozen_ReturnFalse()
        {
            //Arrange
            TimeProvider.Override(Fixture.Create<DateTime>());

            //Act
            var result = TimeProvider.IsFrozen;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsFrozen_ReturnTrue()
        {
            //Arrange
            TimeProvider.Freeze();

            //Act
            var result = TimeProvider.IsFrozen;

            //Assert
            result.Should().BeTrue();
        }
    }

    [TestClass]
    public class Freeze_Parameterless : TimeProviderTesterBase
    {
        [TestMethod]
        public void Always_FreezeTimeInPlaceAtSpecifiedDate()
        {
            //Arrange
            var now = DateTimeOffset.Now;

            //Act
            TimeProvider.Freeze();

            //Assert
            Thread.Sleep(1000);
            TimeProvider.Now.TrimMilliseconds().Should().Be(now.TrimMilliseconds());
        }
    }

    [TestClass]
    public class Freeze_DateTime : TimeProviderTesterBase
    {
        [TestMethod]
        public void Always_FreezeTimeInPlaceAtSpecifiedDate()
        {
            //Arrange
            var date = Fixture.Create<DateTime>();

            //Act
            TimeProvider.Freeze(date);

            //Assert
            Thread.Sleep(1000);
            TimeProvider.Now.Should().BeExactly(date);
        }
    }

    [TestClass]
    public class Freeze_DateTimeOffset : TimeProviderTesterBase
    {
        [TestMethod]
        public void Always_FreezeTimeInPlaceAtSpecifiedDate()
        {
            //Arrange
            var date = Fixture.Create<DateTimeOffset>();

            //Act
            TimeProvider.Freeze(date);

            //Assert
            Thread.Sleep(1000);
            TimeProvider.Now.Should().BeExactly(date);
        }
    }

    [TestClass]
    public class Reset : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenIsNotOverridenOrFrozen_DoNothing()
        {
            //Arrange
            var now = TimeProvider.Now;

            //Act
            TimeProvider.Reset();

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(now.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsFrozen_Unfreeze()
        {
            //Arrange
            TimeProvider.Freeze(Fixture.Create<DateTimeOffset>());

            //Act
            TimeProvider.Reset();

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsFrozen_NowIsEqualToCurrentDateAndTime()
        {
            //Arrange
            TimeProvider.Freeze(Fixture.Create<DateTimeOffset>());

            //Act
            TimeProvider.Reset();

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsOverridenAndNotFrozen_RemoveOverride()
        {
            //Arrange
            TimeProvider.Override(Fixture.Create<DateTimeOffset>());

            //Act
            TimeProvider.Reset();

            //Assert
            TimeProvider.IsOverriden.Should().BeFalse();
        }
    }

    [TestClass]
    public class Unfreeze : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenIsNotOverridenOrFrozen_DoNothing()
        {
            //Arrange
            var now = TimeProvider.Now;

            //Act
            TimeProvider.Unfreeze();

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(now.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsOverridenAndNotFrozen_DoNothing()
        {
            //Arrange
            var overridenDate = Fixture.Create<DateTimeOffset>();
            TimeProvider.Override(overridenDate);

            //Act
            TimeProvider.Unfreeze();

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(overridenDate.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsFrozen_RemoveFreeze()
        {
            //Arrange
            TimeProvider.Freeze(Fixture.Create<DateTimeOffset>());

            //Act
            TimeProvider.Unfreeze();

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsFrozen_NowIsEqualToCurrentDateAndTime()
        {
            //Arrange
            TimeProvider.Freeze();

            //Act
            TimeProvider.Unfreeze();

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.TrimMilliseconds());
        }
    }

    [TestClass]
    public class AddYears : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.AddYears(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddYearsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.AddYears(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddYearsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddYears(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddYears(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddYears(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddYears(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddYears(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddYears(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddYears(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractYears : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.SubtractYears(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractYearsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.SubtractYears(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractYearsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractYears(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddYears(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractYears(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractYears(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddYears(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractYears(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractYears(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddMonths : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.AddMonths(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMonthsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.AddMonths(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMonthsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMonths(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddMonths(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMonths(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMonths(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMonths(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMonths(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMonths(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractMonths : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.SubtractMonths(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMonthsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.SubtractMonths(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMonthsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMonths(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddMonths(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMonths(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMonths(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMonths(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMonths(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMonths(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddDays : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.AddDays(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddDaysToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.AddDays(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddDaysToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddDays(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddDays(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddDays(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddDays(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddDays(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddDays(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddDays(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractDays : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.SubtractDays(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractDaysFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.SubtractDays(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractDaysFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractDays(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddDays(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractDays(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractDays(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddDays(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractDays(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractDays(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddHours : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.AddHours(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddHoursToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.AddHours(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddHoursToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddHours(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddHours(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddHours(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddHours(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddHours(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddHours(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddHours(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractHours : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.SubtractHours(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractHoursFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.SubtractHours(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractHoursFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractHours(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddHours(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractHours(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractHours(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddHours(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractHours(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractHours(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddMinutes : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.AddMinutes(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMinutesToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.AddMinutes(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMinutesToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMinutes(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddMinutes(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMinutes(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMinutes(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMinutes(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMinutes(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMinutes(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractMinutes : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.SubtractMinutes(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMinutesFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.SubtractMinutes(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMinutesFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMinutes(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddMinutes(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMinutes(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMinutes(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMinutes(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMinutes(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMinutes(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddSeconds : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.AddSeconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddSecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.AddSeconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddSecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddSeconds(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddSeconds(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddSeconds(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddSeconds(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddSeconds(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddSeconds(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddSeconds(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractSeconds : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.SubtractSeconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractSecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.SubtractSeconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractSecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractSeconds(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddSeconds(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractSeconds(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractSeconds(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddSeconds(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractSeconds(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractSeconds(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddMilliseconds : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.AddMilliseconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMillisecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.AddMilliseconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMillisecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMilliseconds(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddMilliseconds(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMilliseconds(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMilliseconds(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMilliseconds(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMilliseconds(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.AddMilliseconds(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractMilliseconds : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Fixture.Create<int>();

            //Act
            var action = () => TimeProvider.SubtractMilliseconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMillisecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => TimeProvider.SubtractMilliseconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMillisecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMilliseconds(value);

            //Assert
            TimeProvider.Now.Should().Be(now.AddMilliseconds(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Fixture.Create<DateTimeOffset>();
            TimeProvider.Freeze(now);

            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMilliseconds(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMilliseconds(value);

            //Assert
            TimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMilliseconds(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMilliseconds(value);

            //Assert
            TimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Fixture.Create<int>();

            //Act
            TimeProvider.SubtractMilliseconds(value);

            //Assert
            TimeProvider.IsOverriden.Should().BeTrue();
        }
    }
}