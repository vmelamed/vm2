namespace vm2.Ulid1.Rng;

using System.Text;

/// <summary>
/// Represents a random number generator based on the HMAC-based Key Derivation Function (HKDF).
/// </summary>
public class HkdfRng : IRandom
{
    Lock _lock = new();
    byte[] _primaryKey = RandomNumberGenerator.GetBytes(32);
    string _info = DateTimeOffset.UtcNow.ToString("o");

    /// <summary>
    /// Fills the specified span with cryptographically secure random bytes derived using HKDF.
    /// </summary>
    public void Fill(Span<byte> bytes)
    {
        lock (_lock)
            HKDF.Expand(
                HashAlgorithmName.SHA256,
                _primaryKey.AsSpan(),
                bytes,
                Encoding.UTF8.GetBytes(_info + "vm.Ulid1.Rng.HkdfRng").AsSpan());
    }
}