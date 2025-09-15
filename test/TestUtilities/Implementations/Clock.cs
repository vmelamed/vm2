namespace vm2.TestUtilities.Implementations;

public class Clock : IClock
{
    public DateTime Now => TestClock.Now();
}
