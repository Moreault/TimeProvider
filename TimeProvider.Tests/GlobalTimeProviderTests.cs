namespace ToolBX.TimeProvider.Tests;

public abstract class TimeProviderTesterBase : Tester
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.Customize(new DateTimeCustomization(), new DateTimeOffsetCustomization());
        GlobalTimeProvider.Reset();
    }
}

[TestClass]
public class GlobalTimeProviderTests
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
            var result = GlobalTimeProvider.Now.TrimMilliseconds();

            //Assert
            result.TrimMilliseconds().Should().Be(now.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsFrozenAndNotOverriden_ReturnFrozenTime()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            //Act
            var result = GlobalTimeProvider.Now;

            //Assert
            result.Should().Be(now);
        }

        [TestMethod]
        public void WhenIsNotFrozenAndOverriden_ReturnTimeDifferenceBetweenNowAndOverride()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Override(now);

            //Act
            var result = GlobalTimeProvider.Now;

            //Assert
            result.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
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
            var result = GlobalTimeProvider.Today;

            //Assert
            result.Should().Be(today);
        }

        [TestMethod]
        public void WhenIsFrozenAndNotOverriden_ReturnFrozenTime()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            //Act
            var result = GlobalTimeProvider.Today;

            //Assert
            result.TrimMilliseconds().Should().Be(now.Date.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozenAndOverriden_ReturnTimeDifferenceBetweenNowAndOverride()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Override(now);

            //Act
            var result = GlobalTimeProvider.Today;

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
            var result = GlobalTimeProvider.IsOverriden;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsOverriden_ReturnTrue()
        {
            //Arrange
            GlobalTimeProvider.Override(Dummy.Create<DateTime>());

            //Act
            var result = GlobalTimeProvider.IsOverriden;

            //Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsFrozen_ReturnTrue()
        {
            //Arrange
            GlobalTimeProvider.Freeze();

            //Act
            var result = GlobalTimeProvider.IsOverriden;

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
            var result = GlobalTimeProvider.IsFrozen;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsOverridenButNotFrozen_ReturnFalse()
        {
            //Arrange
            GlobalTimeProvider.Override(Dummy.Create<DateTime>());

            //Act
            var result = GlobalTimeProvider.IsFrozen;

            //Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsFrozen_ReturnTrue()
        {
            //Arrange
            GlobalTimeProvider.Freeze();

            //Act
            var result = GlobalTimeProvider.IsFrozen;

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
            GlobalTimeProvider.Freeze();

            //Assert
            Thread.Sleep(1000);
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(now.TrimMilliseconds());
        }
    }

    [TestClass]
    public class Freeze_DateTime : TimeProviderTesterBase
    {
        [TestMethod]
        public void Always_FreezeTimeInPlaceAtSpecifiedDate()
        {
            //Arrange
            var date = Dummy.Create<DateTime>();

            //Act
            GlobalTimeProvider.Freeze(date);

            //Assert
            Thread.Sleep(1000);
            GlobalTimeProvider.Now.Should().BeExactly(date);
        }
    }

    [TestClass]
    public class Freeze_DateTimeOffset : TimeProviderTesterBase
    {
        [TestMethod]
        public void Always_FreezeTimeInPlaceAtSpecifiedDate()
        {
            //Arrange
            var date = Dummy.Create<DateTimeOffset>();

            //Act
            GlobalTimeProvider.Freeze(date);

            //Assert
            Thread.Sleep(1000);
            GlobalTimeProvider.Now.Should().BeExactly(date);
        }
    }

    [TestClass]
    public class Reset : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenIsNotOverridenOrFrozen_DoNothing()
        {
            //Arrange
            var now = GlobalTimeProvider.Now;

            //Act
            GlobalTimeProvider.Reset();

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(now.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsFrozen_Unfreeze()
        {
            //Arrange
            GlobalTimeProvider.Freeze(Dummy.Create<DateTimeOffset>());

            //Act
            GlobalTimeProvider.Reset();

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsFrozen_NowIsEqualToCurrentDateAndTime()
        {
            //Arrange
            GlobalTimeProvider.Freeze(Dummy.Create<DateTimeOffset>());

            //Act
            GlobalTimeProvider.Reset();

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsOverridenAndNotFrozen_RemoveOverride()
        {
            //Arrange
            GlobalTimeProvider.Override(Dummy.Create<DateTimeOffset>());

            //Act
            GlobalTimeProvider.Reset();

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeFalse();
        }
    }

    [TestClass]
    public class Unfreeze : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenIsNotOverridenOrFrozen_DoNothing()
        {
            //Arrange
            var now = GlobalTimeProvider.Now;

            //Act
            GlobalTimeProvider.Unfreeze();

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(now.TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsOverridenAndNotFrozen_DoNothing()
        {
            //Arrange
            var overridenDate = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Override(overridenDate);

            //Act
            GlobalTimeProvider.Unfreeze();

            //Assert
            GlobalTimeProvider.Now.Should().BeCloseTo(overridenDate, TimeSpan.FromMilliseconds(1000));
        }

        [TestMethod]
        public void WhenIsFrozen_RemoveFreeze()
        {
            //Arrange
            GlobalTimeProvider.Freeze(Dummy.Create<DateTimeOffset>());

            //Act
            GlobalTimeProvider.Unfreeze();

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsFrozen_NowIsEqualToCurrentDateAndTime()
        {
            //Arrange
            GlobalTimeProvider.Freeze();

            //Act
            GlobalTimeProvider.Unfreeze();

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.TrimMilliseconds());
        }
    }

    [TestClass]
    public class AddYears : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.AddYears(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddYearsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.AddYears(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddYearsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.AddYears(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddYears(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.AddYears(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.AddYears(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddYears(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.AddYears(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.AddYears(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractYears : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.SubtractYears(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractYearsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.SubtractYears(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractYearsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractYears(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddYears(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractYears(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractYears(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddYears(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractYears(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractYears(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddMonths : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.AddMonths(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMonthsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.AddMonths(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMonthsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Create<int>();

            //Act
            GlobalTimeProvider.AddMonths(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddMonths(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Create<int>();

            //Act
            GlobalTimeProvider.AddMonths(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Create<int>();

            //Act
            GlobalTimeProvider.AddMonths(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMonths(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Create<int>();

            //Act
            GlobalTimeProvider.AddMonths(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Create<int>();

            //Act
            GlobalTimeProvider.AddMonths(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractMonths : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.SubtractMonths(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMonthsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.SubtractMonths(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMonthsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractMonths(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddMonths(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractMonths(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractMonths(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMonths(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractMonths(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100).Create();

            //Act
            GlobalTimeProvider.SubtractMonths(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddDays : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.AddDays(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddDaysToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.AddDays(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddDaysToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.AddDays(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddDays(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.AddDays(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.AddDays(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddDays(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.AddDays(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.AddDays(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractDays : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.SubtractDays(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractDaysFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.SubtractDays(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractDaysFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.SubtractDays(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddDays(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.SubtractDays(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.SubtractDays(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddDays(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.SubtractDays(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000).Create();

            //Act
            GlobalTimeProvider.SubtractDays(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddHours : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.AddHours(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddHoursToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.AddHours(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddHoursToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.AddHours(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddHours(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.AddHours(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.AddHours(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddHours(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.AddHours(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.AddHours(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractHours : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.SubtractHours(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractHoursFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.SubtractHours(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractHoursFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.SubtractHours(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddHours(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.SubtractHours(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.SubtractHours(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddHours(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.SubtractHours(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 10000).Create();

            //Act
            GlobalTimeProvider.SubtractHours(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddMinutes : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Number.Between(1, 100000).Create();


            //Act
            var action = () => GlobalTimeProvider.AddMinutes(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMinutesToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.AddMinutes(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMinutesToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.AddMinutes(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddMinutes(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.AddMinutes(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.AddMinutes(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMinutes(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.AddMinutes(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.AddMinutes(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractMinutes : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.SubtractMinutes(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMinutesFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.SubtractMinutes(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMinutesFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.SubtractMinutes(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddMinutes(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.SubtractMinutes(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.SubtractMinutes(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMinutes(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.SubtractMinutes(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 100000).Create();

            //Act
            GlobalTimeProvider.SubtractMinutes(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddSeconds : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.AddSeconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddSecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.AddSeconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddSecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000000).Create();


            //Act
            GlobalTimeProvider.AddSeconds(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddSeconds(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.AddSeconds(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.AddSeconds(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddSeconds(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.AddSeconds(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.AddSeconds(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractSeconds : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.SubtractSeconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractSecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.SubtractSeconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractSecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractSeconds(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddSeconds(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractSeconds(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractSeconds(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddSeconds(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractSeconds(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractSeconds(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class AddMilliseconds : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.AddMilliseconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMillisecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.AddMilliseconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotAddMillisecondsToCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.AddMilliseconds(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddMilliseconds(value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.AddMilliseconds(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.AddMilliseconds(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMilliseconds(value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.AddMilliseconds(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.AddMilliseconds(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }

    [TestClass]
    public class SubtractMilliseconds : TimeProviderTesterBase
    {
        [TestMethod]
        public void WhenValueIsNegative_Throw()
        {
            //Arrange
            var value = -Dummy.Create<int>();

            //Act
            var action = () => GlobalTimeProvider.SubtractMilliseconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMillisecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenValueIsZero_Throw()
        {
            //Arrange
            const int value = 0;

            //Act
            var action = () => GlobalTimeProvider.SubtractMilliseconds(value);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage(string.Format(Exceptions.CannotSubtractMillisecondsFromCurrentDateBecauseValueIsLowerThanZero, value));
        }

        [TestMethod]
        public void WhenIsFrozen_FreezeToNewDate()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractMilliseconds(value);

            //Assert
            GlobalTimeProvider.Now.Should().Be(now.AddMilliseconds(-value));
        }

        [TestMethod]
        public void WhenIsFrozen_RemainFrozen()
        {
            //Arrange
            var now = Dummy.Create<DateTimeOffset>();
            GlobalTimeProvider.Freeze(now);

            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractMilliseconds(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeTrue();
        }

        [TestMethod]
        public void WhenIsNotFrozen_OverrideToNewDate()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractMilliseconds(value);

            //Assert
            GlobalTimeProvider.Now.TrimMilliseconds().Should().Be(DateTimeOffset.Now.AddMilliseconds(-value).TrimMilliseconds());
        }

        [TestMethod]
        public void WhenIsNotFrozen_DoNotFreeze()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractMilliseconds(value);

            //Assert
            GlobalTimeProvider.IsFrozen.Should().BeFalse();
        }

        [TestMethod]
        public void WhenIsNotFrozen_IsOverridenIsTrue()
        {
            //Arrange
            var value = Dummy.Number.Between(1, 1000000).Create();

            //Act
            GlobalTimeProvider.SubtractMilliseconds(value);

            //Assert
            GlobalTimeProvider.IsOverriden.Should().BeTrue();
        }
    }
}