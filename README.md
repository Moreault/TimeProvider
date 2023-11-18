![TimeProvider](https://github.com/Moreault/TimeProvider/blob/master/timeprovider.png)
# TimeProvider
A static utility class for overriding system time.

:warning: The `TimeProvider` class will be renamed `GlobalTimeProvider` in 3.0.0 to avoid name collisions with .NET 8's new `TimeProvider` class.

## Getting started

In order to use this library to its full potential, all your DateTime.Now calls must be changed to TimeProvider.Now. This will ensure that time overrides are accurate for your entire application.

```c#

//Sets the current date to Christmas 2025
TimeProvider.Override(new DateTime(2025, 12, 25));

//Will return the above overriden date ^ ... plus the amount of time that went by between overriding the date and now
var now = TimeProvider.Now;

//If you want to "freeze" time in place you can use
TimeProvider.Freeze(new DateTime(2021, 10, 31);

//Will return the above frozen date ^ ... which will always be halloween 2021 at exactly midnight no matter how much time goes by
var now = TimeProvider.Now;

//If you just want to test how it'll go tomorrow
TimeProvider.AddDays(1);

//Or how it'll go three years ago
TimeProvider.SubtractYears(3);

```


## Why

In most cases, TimeProvider is used to facilitate unit and integration tests that depend on time. 

Let’s say you were working on a video game, and you want to give the player a “Happy birthday!” message when they play it on their birthday. 

To test it, you could have a way to override the date in your debug menu which would ultimately call TimeProvider.Override(). 

Alternatively, I suppose you could modify your system time manually, but I find it more complicated than it needs to be on some operating systems such as Windows. TimeProvider also provides you with a unified way of overriding dates in case you need to test your application on multiple platforms.

But it’s not all sunshine and rainbows either because nothing prevents you or your colleagues from just using DateTime.Now instead of TimeProvider.Now. There is no perfect solution to the problem- at least none that I have found yet.