namespace vm2.UlidType.Rng;

/// <summary>
/// Provides cryptographically secure random number generation functionality.
/// </summary>
/// <remarks>This class implements the <see cref="IRandom"/> interface and is designed to generate random data
/// suitable for cryptographic operations. It ensures that the generated random numbers are of high quality and
/// unpredictable.</remarks>
public class CryptographicRng : IRandom
{
    /// <summary>
    /// Fills the specified span with random bytes.
    /// </summary>
    /// <remarks>This method populates the entire span with random data. The caller is responsible for
    /// ensuring that the span has sufficient capacity to hold the data.</remarks>
    /// <param name="bytes">The span to fill with random byte values. The span must not be empty.</param>
    /// <exception cref="NotImplementedException">This method is not implemented.</exception>
    public void Fill(Span<byte> bytes)
        => RandomNumberGenerator.Fill(bytes);
}
