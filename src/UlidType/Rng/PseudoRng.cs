namespace vm2.UlidType.Rng;

/// <summary>
/// Provides a pseudo-random number generator that fills a span of bytes with random values.
/// </summary>
public class PseudoRng : IRandom
{
    Random _random = new(unchecked((int)DateTime.UtcNow.Ticks));
    Lock _lock = new();

    /// <summary>
    /// Fills the specified span with random bytes.
    /// </summary>
    public void Fill(Span<byte> bytes)
    {
        lock (_lock)
            _random.NextBytes(bytes);
    }
}
