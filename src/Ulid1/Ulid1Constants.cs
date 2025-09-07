namespace vm2.Ulid1;

/// <summary>
/// Provides constants and utility methods related to the Universally Unique Lexicographically Sortable Identifier (ULID) format.
/// </summary>
/// <remarks>
/// This class defines constants for the structure and validation of ULIDs, including offsets, lengths, and the Crockford Base32<br/>
/// alphabet. It also provides a precompiled regular expression for validating ULID strings.
/// </remarks>
public static partial class Ulid1Constants
{
    /// <summary>
    /// Represents the offset of the timestamp bytes in a ULID.
    /// </summary>
    public const int TimestampBegin             = 0;

    /// <summary>
    /// Represents the length of the timestamp bytes in a ULID.
    /// </summary>
    public const int TimestampLength            = 6;

    /// <summary>
    /// Represents the length of the timestamp bytes in a ULID.
    /// </summary>
    public const int TimestampEnd               = TimestampBegin + TimestampLength;

    /// <summary>
    /// Represents the offset of the random bytes in a ULID.
    /// </summary>
    public const int RandomBegin                = TimestampEnd;

    /// <summary>
    /// Represents the length of the random bytes in a ULID.
    /// </summary>
    public const int RandomLength               = 10;

    /// <summary>
    /// Represents the offset of the random bytes in a ULID.
    /// </summary>
    public const int RandomEnd                  = RandomBegin + RandomLength;

    /// <summary>
    /// Represents the total length, in bytes, of a ULID (Universally Unique Lexicographically Sortable Identifier).
    /// </summary>
    public const int UlidBytesLength            = TimestampLength + RandomLength;

    /// <summary>
    /// Represents the total length, in bytes, of a ULID (Universally Unique Lexicographically Sortable Identifier).
    /// </summary>
    public const int UlidBitsLength             = UlidBytesLength * 8;

    /// <summary>
    /// Represents the Crockford Base32 alphabet, a character set used for encoding data (here ULID data) in a case-insensitive manner.
    /// </summary>
    /// <remarks>
    /// The Crockford Base32 alphabet excludes characters that are easily confused, such as 'I', 'L', 'O', and 'U'. This alphabet<br/>
    /// is commonly used in applications such as unique identifier generation and human-readable encoding.
    /// </remarks>
    public const string CrockfordAlphabet       = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

    /// <summary>
    /// Represents the bit shift value used for ULID-related calculations.
    /// </summary>
    /// <remarks>
    /// This constant defines the number of bits to shift in binary to string operations involving ULIDs. It is primarily used <br/>
    /// to adjust or manipulate ULID components during encoding or decoding processes.
    /// </remarks>
    public static readonly int UlidCharShift    = Math.ILogB(CrockfordAlphabet.Length);

    /// <summary>
    /// Represents the bitmask used to extract the least significant character ULID value.
    /// </summary>
    /// <remarks>
    /// This constant is derived from the bit shift value defined by <see cref="UlidCharShift"/> and is used in operations <br/>
    /// involving ULID character manipulation.
    /// </remarks>
    public static readonly int UlidCharMask     = CrockfordAlphabet.Length - 1;

    /// <summary>
    /// Represents the fixed length of a ULID (Universally Unique Lexicographically Sortable Identifier) string.
    /// </summary>
    public static readonly int UlidStringLength = (UlidBitsLength / UlidCharShift) + (UlidBitsLength % UlidCharShift > 0 ? 1 : 0);

    /// <summary>
    /// The regular expression pattern for validating ULID strings.
    /// </summary>
    public const string UlidStringRegex         = $"[{CrockfordAlphabet}]{{26}}";

    /// <summary>
    /// Creates a compiled, case-insensitive regular expression for validating ULID (Universally Unique Lexicographically<br/>
    /// Sortable Identifier) strings.
    /// </summary>
    /// <remarks>
    /// The generated regular expression matches strings that conform to the ULID format, which consists of 26 alphanumeric<br/>
    /// characters from the <see cref="CrockfordAlphabet"/>. This method is optimized for performance by pre-compiling the regular expression.
    /// </remarks>
    /// <returns>A <see cref="Regex"/> instance configured to match valid ULID strings.</returns>
    [GeneratedRegex(UlidStringRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase, 500)]
    public static partial Regex UlidString();
}
