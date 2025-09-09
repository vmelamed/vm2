namespace vm2.UlidType;

/// <summary>
/// Represents a source of random numbers.
/// </summary>
/// <remarks>This interface defines the contract for generating random numbers. Implementations may provide
/// different algorithms or sources of randomness, such as cryptographic or pseudo-random number generators.</remarks>
public interface IRandom
{
    /// <summary>
    /// Fills the specified span with random bytes.
    /// </summary>
    /// <remarks>This method overwrites the entire span with random values. The caller is responsible for
    /// ensuring that the span is appropriately sized and initialized before calling this method.</remarks>
    /// <param name="bytes">The span to fill with random data. The span must have a non-zero length.</param>
    void Fill(Span<byte> bytes);
}