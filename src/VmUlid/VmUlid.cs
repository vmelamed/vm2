namespace vm2.VmUlid;

/// <summary>
/// Represents a Universally Unique Lexicographically Sortable Identifier (ULID).
/// </summary>
/// <remarks>
/// A ULID is a 128-bit identifier that combines a timestamp with a random component, ensuring both uniqueness and lexicographical<br/>
/// lexicographical order. This struct provides methods for creating, parsing, and manipulating ULIDs, as well as converting them<br/>
/// to other formats such as strings or GUIDs. ULIDs are commonly used in distributed systems where unique, sortable identifiers<br/>
/// are required.
/// </remarks>
public readonly struct VmUlid : IEquatable<VmUlid>, IComparable<VmUlid>, IParsable<VmUlid>
{
    readonly byte[] _ulidBytes;

    /// <summary>
    /// Initializes a new instance of the <see cref="VmUlid"/> struct using the specified byte ulidBytesSpan. Used only by the <see cref="VmUlidFactory"/>.
    /// </summary>
    /// <remarks>
    /// This constructor creates a ULID from the provided byte ulidBytesSpan. The caller must ensure that the ulidBytesSpan contains a valid ULID
    /// representation.<br/>
    /// The data is copied into an internal buffer, so changes to the original ulidBytesSpan after construction do not affect the ULID instance.
    /// </remarks>
    /// <param name="bytes">
    /// A read-only ulidBytesSpan of bytes representing the ULID. The ulidBytesSpan must be exactly  <see cref="UlidBytesLength"/> bytes long.
    /// </param>
    internal VmUlid(in ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length != UlidBytesLength)
            throw new ArgumentException($"The input ulidBytesSpan of bytes must be exactly {UlidBytesLength} bytes long.", nameof(bytes));

        _ulidBytes = new byte[UlidBytesLength];
        bytes.CopyTo(_ulidBytes);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VmUlid"/> struct using the specified string representation.
    /// </summary>
    /// <param name="source">The string representation of the ULID to parse. Must be a valid ULID string.</param>
    public VmUlid(string source)
        => _ulidBytes = Parse(source)._ulidBytes;

    /// <summary>
    /// Gets the bytes of the current ULID instance into a new byte array.
    /// </summary>
    /// <remarks>
    /// The returned byte array is a copy of the internal representation, ensuring that modifications to the array do not affect<br/>
    /// the original ULID instance.
    /// </remarks>
    /// <returns>A new byte array containing the 16 bytes that represent the ULID.</returns>
    public readonly byte[] ToByteArray() => (byte[])_ulidBytes.Clone();

    /// <summary>
    /// Gets the bytes of the current ULID instance into a new byte array.
    /// </summary>
    /// <remarks>
    /// The returned byte array is a copy of the internal representation, ensuring that modifications to the array do not affect<br/>
    /// the original ULID instance.
    /// </remarks>
    /// <returns>A new byte array containing the 16 bytes that represent the ULID.</returns>
    public readonly ReadOnlySpan<byte> ToByteSpan() => _ulidBytes.AsSpan();

    /// <summary>
    /// Converts the current ULID value to its equivalent Base64 string representation.
    /// </summary>
    /// <remarks>
    /// The Base64 string is generated from the underlying byte array of the ULID. This representation is suitable for compact<br/>
    /// storage or transmission of the ULID value.
    /// </remarks>
    /// <returns>A Base64-encoded string that represents the ULID value.</returns>
    public readonly string ToBase64() => Convert.ToBase64String(_ulidBytes.AsSpan());

    /// <summary>
    /// Converts the current ULID instance to its equivalent Base32 (the default) string representation.
    /// </summary>
    /// <remarks>
    /// The string representation follows the standard ULID format, which is a 26-character case-sensitive alphanumeric string.<br/>
    /// This method is optimized for performance and avoids unnecessary allocations.
    /// </remarks>
    /// <returns>A 26-character string that represents the current ULID instance.</returns>
    public override string ToString()
    {
        Span<char> span = stackalloc char[UlidStringLength];

        var r = TryWriteStringify(span);

        Debug.Assert(r is true);
        return new string(span);
    }

    /// <summary>
    /// Attempts to write the string representation of the ULID to the specified character ulidBytesSpan using the Crockford Base32 encoding.
    /// </summary>
    /// <remarks>
    /// The method encodes the ULID as a 26-character string using the Crockford Base32 alphabetSpan. The caller must ensure that the<br/>
    /// <paramref name="destination"/> ulidBytesSpan has sufficient capacity  to hold the resulting string. If the ulidBytesSpan is smaller than<br/>
    /// <see cref="UlidStringLength"/>, the method returns <see langword="false"/>  and does not modify the ulidBytesSpan.
    /// </remarks>
    /// <param name="destination">
    /// The ulidBytesSpan of characters where the ULID string representation will be written. The ulidBytesSpan must have a length of<br/>
    /// <see cref="UlidStringLength"/> or more.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the ULID string representation was successfully written to the  <paramref name="destination"/> ulidBytesSpan; otherwise,<br/>
    /// <see langword="false"/> if the ulidBytesSpan is too small.
    /// </returns>
    public readonly bool TryWriteStringify(Span<char> destination)
    {
        if (destination.Length < UlidStringLength)
            return false;

        var ulidAsNumber = ReadUInt128BigEndian(_ulidBytes.AsSpan());

        for (var i = 0; i < UlidStringLength; i++)
        {
            // get the least significant 5 bits from and convert it to character
            destination[UlidStringLength-i-1] = CrockfordDigits[(byte)ulidAsNumber & UlidCharMask];
            ulidAsNumber >>>= UlidCharShift;
        }

        Debug.Assert(ulidAsNumber == 0);
        return true;
    }

    /// <summary>
    /// Attempts to write the ULID bytes to the specified destination buffer.
    /// </summary>
    /// <remarks>
    /// This method does not throw an exception if the destination buffer is too small. Instead, it returns <see langword="false"/>
    /// to indicate failure.
    /// </remarks>
    /// <param name="destination">
    /// The buffer to which the ULID bytes will be written. Must have a length of at least <see cref="UlidBytesLength"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the ULID bytes were successfully written to the destination buffer; otherwise, <see langword="false"/>.
    /// </returns>
    public readonly bool TryWrite(Span<byte> destination)
    {
        if (destination.Length < UlidBytesLength)
            return false;

        _ulidBytes.AsSpan().CopyTo(destination);
        return true;
    }

    /// <summary>
    /// Extracts and converts the ULID's timestamp component into a <see cref="DateTimeOffset"/> representation.
    /// </summary>
    /// <remarks>
    /// The returned <see cref="DateTimeOffset"/> represents the timestamp encoded in the ULID, which is based on the ulidAsNumber of <br/>
    /// milliseconds since the Unix epoch (January 1, 1970, 00:00:00 UTC).
    /// </remarks>
    /// <returns>
    /// A <see cref="DateTimeOffset"/> representing the timestamp component of the ULID.
    /// </returns>
    public readonly DateTimeOffset Timestamp()
    {
        Span<byte> tsBytes = stackalloc byte[sizeof(long)];

        _ulidBytes[TimestampBegin..TimestampEnd].CopyTo(tsBytes[2..8]);
        if (BitConverter.IsLittleEndian)
            tsBytes.Reverse();

        return DateTimeOffset.FromUnixTimeMilliseconds(BitConverter.ToInt64(tsBytes));
    }

    /// <summary>
    /// Extracts the bytes of the random component from the current ULID instance.
    /// </summary>
    /// <remarks>
    /// The returned byte array represents the random portion of the ULID, which is independent of the timestamp component.
    /// </remarks>
    /// <returns>
    /// A byte array containing the random component of the ULID.
    /// </returns>
    public readonly void Random(in Span<byte> bytes)
    {
        if (bytes.Length < RandomLength)
            throw new ArgumentException($"The parameter must be {RandomLength} long.", nameof(bytes));

        _ulidBytes.AsSpan(RandomBegin, RandomLength).CopyTo(bytes);
    }

    /// <summary>
    /// Extracts the bytes of the random component from the current ULID instance.
    /// </summary>
    /// <remarks>
    /// The returned byte array represents the random portion of the ULID, which is independent of the timestamp component.
    /// </remarks>
    /// <returns>
    /// A byte array containing the random component of the ULID.
    /// </returns>
    public readonly byte[] Random()
    {
        Span<byte> bytes = stackalloc byte[RandomLength];

        Random(bytes);
        return bytes.ToArray();
    }

    /// <summary>
    /// Parses the specified string representation of a ULID and returns the corresponding <see cref="VmUlid"/> instance.
    /// </summary>
    /// <param name="s">The string representation of the ULID to parse.</param>
    /// <param name="formatProvider">An optional format provider, which is ignored in this implementation.</param>
    /// <returns>The <see cref="VmUlid"/> instance that corresponds to the parsed string.</returns>
    /// <exception cref="ArgumentException">Thrown if the input string <paramref name="s"/> cannot be parsed as a valid ULID.</exception>
    public static VmUlid Parse(string s, IFormatProvider? formatProvider = null)
        => TryParse(s, formatProvider, out var u) ? u! : throw new ArgumentException("Unable to parse the input", nameof(s));

    /// <summary>
    /// Attempts to parse the specified string representation of a ULID (Universally Unique Lexicographically Sortable Identifier)<br/>
    /// and returns a value indicating whether the operation succeeded.
    /// </summary>
    /// <remarks>
    /// The method validates the input string against the ULID format and attempts to parse it into a <see cref="VmUlid"/> instance.<br/>
    /// If the input string does not conform to the ULID format, the method returns <see langword="false"/> and the <paramref name="result"/><br/>
    /// parameter is set to <see langword="null"/>.
    /// </remarks>
    /// <param name="source">
    /// The string representation of the ULID to parse. The string must conform to the ULID format.
    /// </param>
    /// <param name="provider">
    /// Ignored in this implementation.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the parsed <see cref="VmUlid"/> value if the parsing succeeded; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the string was successfully parsed into a valid <see cref="VmUlid"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryParse(
        [NotNullWhen(true)] string? source,
        IFormatProvider? provider,
        out VmUlid result)
    {
        result = new VmUlid();

        if (string.IsNullOrWhiteSpace(source) || source.Length < UlidStringLength)
            return false;

        Span<char> sourceSpan = stackalloc char[source.Length];

        source.CopyTo(sourceSpan);

        // parse the string into a UInt128 value first
        UInt128 ulidAsNumber = 0;
        var alphabetSpan = CrockfordDigits.AsSpan();

        for (var i = 0; i < UlidStringLength; i++)
        {
            if (i > 0)
                ulidAsNumber <<= 5;
            ulidAsNumber |= (UInt128)alphabetSpan.BinarySearch(char.ToUpper(sourceSpan[i]));
        }

        // get the bytes of the UInt128 value
        var ulidSpan = BitConverter.GetBytes(ulidAsNumber).AsSpan();

        // make sure they are big-endian
        if (BitConverter.IsLittleEndian)
            ulidSpan.Reverse();

        // this is our ULID
        result = new VmUlid(ulidSpan);
        return true;
    }

    /// <summary>
    /// Attempts to parse the specified string representation of a ULID (Universally Unique Lexicographically Sortable Identifier).
    /// </summary>
    /// <remarks>
    /// This method does not throw an exception if the parsing fails. Instead, it returns <see langword="false"/> and sets <paramref name="result"/><br/>
    /// to <see langword="null"/>.
    /// </remarks>
    /// <param name="source">
    /// The string to parse as a ULID. This value can be <see langword="null"/>.
    /// </param>
    /// <param name="result">
    /// When this method returns, contains the parsed <see cref="VmUlid"/> if the parsing succeeded; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the string was successfully parsed as a ULID; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryParse(
        [NotNullWhen(true)] string? source,
        out VmUlid result)
        => TryParse(source, null, out result);

    /// <summary>
    /// Determines whether the current instance is equal to the specified <see cref="VmUlid"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="VmUlid"/> instance to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true"/> if the current instance is equal to the specified <see cref="VmUlid"/> instance; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(VmUlid other) => _ulidBytes.AsSpan().SequenceCompareTo(other._ulidBytes.AsSpan()) == 0;

    /// <summary>
    /// Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance. Can be <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the specified object is a <see cref="VmUlid"/> and is equal to the current instance; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is VmUlid u && Equals(u);

    /// <summary>
    /// Returns the hash code for the current instance.
    /// </summary>
    /// <remarks>The hash code is computed based on the underlying byte array representing the ULID.</remarks>
    /// <returns>A 32-bit signed integer that serves as the hash code for the current instance.</returns>
    public override int GetHashCode() => _ulidBytes.GetHashCode();

    /// <summary>
    /// Compares the current instance with another <see cref="VmUlid"/> object and returns an integer that indicates their
    /// relative order.
    /// </summary>
    /// <remarks>The comparison is performed based on the byte sequence of the underlying ULID
    /// values.</remarks>
    /// <param name="other">The <see cref="VmUlid"/> instance to compare to the current instance.</param>
    /// <returns>
    /// A signed integer that indicates the relative order of the objects being compared: <list type="bullet">
    /// <item><description>Less than zero if the current instance precedes <paramref name="other"/> in the sort
    /// order.</description></item> <item><description>Zero if the current instance occurs in the same position as
    /// <paramref name="other"/> in the sort order.</description></item> <item><description>Greater than zero if the
    /// current instance follows <paramref name="other"/> in the sort order.</description></item> </list>
    /// </returns>
    public int CompareTo(VmUlid other) => _ulidBytes.AsSpan().SequenceCompareTo(other._ulidBytes.AsSpan());

    /// <summary>
    /// Determines whether two <see cref="VmUlid"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="VmUlid"/> to compare.</param>
    /// <param name="right">The second <see cref="VmUlid"/> to compare.</param>
    /// <returns><see langword="true"/> if the two <see cref="VmUlid"/> instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(VmUlid left, VmUlid right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="VmUlid"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="VmUlid"/> instance to compare.</param>
    /// <param name="right">The second <see cref="VmUlid"/> instance to compare.</param>
    /// <returns><see langword="true"/> if the two <see cref="VmUlid"/> instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(VmUlid left, VmUlid right) => !(left==right);

    /// <summary>
    /// Determines whether the specified <see cref="VmUlid"/> instance is less than another <see cref="VmUlid"/> instance.
    /// </summary>
    /// <param name="left">The first <see cref="VmUlid"/> instance to compare.</param>
    /// <param name="right">The second <see cref="VmUlid"/> instance to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(VmUlid left, VmUlid right) => left.CompareTo(right)<0;

    /// <summary>
    /// Determines whether one <see cref="VmUlid"/> instance is less than or equal to another.
    /// </summary>
    /// <param name="left">The first <see cref="VmUlid"/> instance to compare.</param>
    /// <param name="right">The second <see cref="VmUlid"/> instance to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the value of <paramref name="right"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <=(VmUlid left, VmUlid right) => left.CompareTo(right)<=0;

    /// <summary>
    /// Determines whether one <see cref="VmUlid"/> instance is greater than another.
    /// </summary>
    /// <param name="left">The first <see cref="VmUlid"/> instance to compare.</param>
    /// <param name="right">The second <see cref="VmUlid"/> instance to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(VmUlid left, VmUlid right) => left.CompareTo(right)>0;

    /// <summary>
    /// Determines whether the first <see cref="VmUlid"/> instance is greater than or equal to the second <see
    /// cref="VmUlid"/> instance.
    /// </summary>
    /// <param name="left">The first <see cref="VmUlid"/> instance to compare.</param>
    /// <param name="right">The second <see cref="VmUlid"/> instance to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(VmUlid left, VmUlid right) => left.CompareTo(right)>=0;
}
