namespace vm2.Ulid1;

/// <summary>
/// Provides functionality to generate unique lexicographically sortable identifiers (ULIDs).
/// </summary>
/// <remarks>
/// This factory ensures that ULIDs generated within the same millisecond are unique by incrementing the random portion of the ULID.<br/>
/// In the extremely unlikely event that where incrementing the random portion overflows, an <see cref="OverflowException"/> is <br/>
/// thrown. The generated ULIDs are compliant with the ULID specification and are suitable for use in distributed systems where <br/>
/// uniqueness and ordering are required.<br/>
/// <b>Hint:</b> you may have more than one factory in your program representing separate sequences of ULID-s. E.g. a factory<br/>
/// per DB table.
/// </remarks>
public class Ulid1Factory
{
    long _lastTimestamp;
    byte[] _lastUlid = new byte[UlidBytesLength];
    Lock _lock = new();

    /// <summary>
    /// Generates a new Universally Unique Lexicographically Sortable Identifier (ULID).
    /// </summary>
    /// <remarks>
    /// This method creates a ULID based on the current UTC timestamp and a random component. If called multiple times within
    /// the same millisecond, the random component is incremented to ensure uniqueness. The method is thread-safe.
    /// </remarks>
    /// <returns>A new <see cref="VmUlid"/> instance representing the generated ULID.</returns>
    /// <exception cref="OverflowException">
    /// Thrown if the random component overflows while incrementing within the same millisecond. This is an extremely rare scenario.
    /// </exception>
    public VmUlid NewUlid()
    {
        var ulidSpan = _lastUlid.AsSpan();
        var unixDt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        lock (_lock)
        {
            if (unixDt == _lastTimestamp)
            {
                // increment the random part with carry over
                var random = ulidSpan[RandomBegin..RandomEnd];
                var i = random.Length-1;
                for (; unchecked(++random[i]) == 0 && i >= 0; i--)
                    ;

                if (i < 0)
                    // extremely unlikely
                    throw new OverflowException("Overflow while incrementing the ULID random part.");
            }
            else
            {
                // accept the new time stamp
                var ts = _lastTimestamp = unixDt;

                if (!BitConverter.IsLittleEndian)
                    ts <<= 2*8; // 0x0000010203040506 << 16 == 0x0102030405060000

                BitConverter.TryWriteBytes(ulidSpan, _lastTimestamp);

                if (BitConverter.IsLittleEndian)
                    // 0x0605040302010000.Reverse(0..6) => 0x0102030405060000
                    ulidSpan[TimestampBegin..TimestampEnd].Reverse();

                // fill the random bytes part from crypto-strong RNG, overwriting the last 2 bytes of the 8-bit modified timestamp:
                // 0x01020304050600000000000000000000 => 0x010203040506rrrrrrrrrrrrrrrrrrrr
                RandomNumberGenerator.Fill(ulidSpan[RandomBegin..RandomEnd]);
            }
        }

        // create a new ULID from the bytes
        return new VmUlid(ulidSpan);
    }
}
