namespace vm2.Ulid1.Tests;

public class Ulid1Tests
{
    [Fact]
    public void NewUlid_Roundtrip_ToByteArray_ToGuid_ToBase64_ToString()
    {
        var factory = new VmUlidFactory();
        var ulid = factory.NewUlid();

        var bytes = ulid.ToByteArray();
        bytes.Should().HaveCount(UlidBytesLength);

        ulid.ToGuid().ToByteArray().Should().Equal(bytes);
        Convert.FromBase64String(ulid.ToBase64()).Should().Equal(bytes);

        var s = ulid.ToString();
        s.Should().NotBeNullOrWhiteSpace();
        s.Length.Should().Be(UlidStringLength);
    }

    [Fact]
    public void TryWrite_WithSmallDestination_ReturnsFalse_And_WithCorrectSize_WritesBytes()
    {
        var ulid = new VmUlidFactory().NewUlid();

        var small = new byte[UlidBytesLength - 1];
        ulid.TryWrite(small.AsSpan()).Should().BeFalse();

        var dest = new byte[UlidBytesLength];
        ulid.TryWrite(dest.AsSpan()).Should().BeTrue();
        dest.Should().Equal(ulid.ToByteArray());
    }

    [Fact]
    public void TryWriteStringify_SpanSizeBehavior_And_Matches_ToString()
    {
        var ulid = new VmUlidFactory().NewUlid();

        var tooSmall = new char[UlidStringLength - 1];
        ulid.TryWriteStringify(tooSmall.AsSpan()).Should().BeFalse();

        var buffer = new char[UlidStringLength];
        ulid.TryWriteStringify(buffer).Should().BeTrue();

        var written = new string(buffer);
        written.Should().Be(ulid.ToString());
    }

    [Fact]
    public void Parse_And_TryParse_Roundtrip_And_CaseInsensitive()
    {
        var ulid = new VmUlidFactory().NewUlid();
        var str = ulid.ToString();

        var parsed = VmUlid.Parse(str);
        parsed.Should().Be(ulid);

        VmUlid.TryParse(str, out var result).Should().BeTrue();
        result.Should().Be(ulid);

        // case-insensitive parsing
        VmUlid.TryParse(str.ToLowerInvariant(), out var lower).Should().BeTrue();
        lower.Should().Be(ulid);
    }

    [Fact]
    public void Parse_Invalid_Throws_TryParse_ReturnsFalse()
    {
        var invalid = new string('!', UlidStringLength);

        Action act = () => VmUlid.Parse(invalid);
        act.Should().Throw<ArgumentException>();

        VmUlid.TryParse(invalid, out var r).Should().BeFalse();
    }

    [Fact]
    public void TryParse_Null_Throws_ArgumentNullException()
    {
        Action act = () => VmUlid.TryParse(null!, out _);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Timestamp_And_Random_Are_Extractable_And_Within_Reasonable_Range()
    {
        var factory = new VmUlidFactory();
        var ulid = factory.NewUlid();

        var now = DateTimeOffset.UtcNow;

        var ts = ulid.Timestamp();
        ts.Should().BeOnOrBefore(now);
        ts.Should().BeOnOrAfter(now.AddSeconds(-1));

        var bytes = ulid.ToByteArray();
        var random = new byte[RandomLength];
        ulid.Random(random.AsSpan());
        bytes.Skip(RandomBegin).Take(RandomLength).Should().Equal(random);
    }

    [Fact]
    public void NewUlid_Generates_Unique_And_Monotonic_Ulids_When_Incrementing_Within_Same_Millisecond()
    {
        var factory = new VmUlidFactory();

        // Force the factory into the "same millisecond" increment path by setting private state:
        var lastTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Prepare bytes similar to what the factory would write for the timestamp portion
        var lastUlidBytes = new byte[UlidBytesLength];
        BitConverter.TryWriteBytes(lastUlidBytes.AsSpan(), lastTimestamp);
        if (BitConverter.IsLittleEndian)
            lastUlidBytes.AsSpan(TimestampBegin, TimestampLength).Reverse();

        // Inject _lastTimestamp and _lastUlid via reflection
        var type = typeof(VmUlidFactory);
        var tsField = type.GetField("_lastTimestamp", BindingFlags.Instance | BindingFlags.NonPublic);
        var ulidField = type.GetField("_lastUlid", BindingFlags.Instance | BindingFlags.NonPublic);

        tsField.Should().NotBeNull();
        ulidField.Should().NotBeNull();

        tsField!.SetValue(factory, lastTimestamp);
        ulidField!.SetValue(factory, lastUlidBytes);

        const int count = 10;
        var ulids = Enumerable.Range(0, count).Select(_ => factory.NewUlid()).ToList();

        // All generated ULIDs must be unique
        ulids.Should().OnlyHaveUniqueItems();

        // And they should be strictly increasing (monotonic)
        for (var i = 1; i < ulids.Count; i++)
        {
            ulids[i].CompareTo(ulids[i - 1]).Should().BeGreaterThan(0);
        }
    }

    [Fact]
    public void Equals_CompareTo_And_Operators_Behave_As_Expected()
    {
        var factory = new VmUlidFactory();
        var a = factory.NewUlid();

        var b = factory.NewUlid();

        a.Should().NotBe(b);
        (a == b).Should().BeFalse();
        (a != b).Should().BeTrue();

        (a < b).Should().BeTrue();
        (a <= b).Should().BeTrue();
        (b > a).Should().BeTrue();
        (b >= a).Should().BeTrue();

        // equality via same string
        var a2 = VmUlid.Parse(a.ToString());
        a2.Equals((object)a).Should().BeTrue();
        a2.CompareTo(a).Should().Be(0);
        (a2 == a).Should().BeTrue();
        (a2 <= a).Should().BeTrue();
        (a2 >= a).Should().BeTrue();
    }
}