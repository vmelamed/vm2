namespace vm2.RegexLibTests;

public class UrnTheoryData : TheoryData<string, bool, string, Captures?>
{
}

public partial class UrnsTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    /// <summary>
    /// Test data for URN validation based on RFC 8141.
    /// Tests the AssignedName regex: urn:&lt;NID&gt;:&lt;NSS&gt;
    /// </summary>
    public static UrnTheoryData AssignedNameData = new()
    {
        // Empty and whitespace cases
        { TestFileLine("Empty string"), false, "", null },
        { TestFileLine("Just spaces"), false, "   ", null },
        { TestFileLine("Just urn"), false, "urn", null },
        { TestFileLine("urn with one colon"), false, "urn:", null },
        { TestFileLine("urn with two colons but no NID"), false, "urn::", null },
        { TestFileLine("urn with two colons but no NSS"), false, "urn:nid:", null },

        // Case sensitivity tests
        { TestFileLine("Uppercase URN"), true, "URN:example:test", new()
            {
                [Urns.AssignedNameGr] = "URN:example:test",
                [Urns.NidGr] = "example",
                [Urns.NssGr] = "test"
            }
        },
        { TestFileLine("Mixed case URN"), true, "Urn:example:test", new()
            {
                [Urns.AssignedNameGr] = "Urn:example:test",
                [Urns.NidGr] = "example",
                [Urns.NssGr] = "test"
            }
        },

        // Valid minimal URNs
        { TestFileLine("Minimal valid URN"), true, "urn:az:b", new()
            {
                [Urns.AssignedNameGr] = "urn:az:b",
                [Urns.NidGr] = "az",
                [Urns.NssGr] = "b"
            }
        },

        // NID validation tests
        { TestFileLine("NID with numbers"), true, "urn:test123:value", new()
            {
                [Urns.AssignedNameGr] = "urn:test123:value",
                [Urns.NidGr] = "test123",
                [Urns.NssGr] = "value"
            }
        },
        { TestFileLine("NID with hyphens"), true, "urn:test-nid:value", new()
            {
                [Urns.AssignedNameGr] = "urn:test-nid:value",
                [Urns.NidGr] = "test-nid",
                [Urns.NssGr] = "value"
            }
        },
        { TestFileLine("NID starting with number"), true, "urn:1test:value", new()
            {
                [Urns.AssignedNameGr] = "urn:1test:value",
                [Urns.NidGr] = "1test",
                [Urns.NssGr] = "value"
            }
        },
        { TestFileLine("NID all numbers"), true, "urn:123:value", new()
            {
                [Urns.AssignedNameGr] = "urn:123:value",
                [Urns.NidGr] = "123",
                [Urns.NssGr] = "value"
            }
        },
        { TestFileLine("NID with multiple hyphens"), true, "urn:foo-bar-baz:value", new()
            {
                [Urns.AssignedNameGr] = "urn:foo-bar-baz:value",
                [Urns.NidGr] = "foo-bar-baz",
                [Urns.NssGr] = "value"
            }
        },

        // Invalid NID tests
        { TestFileLine("NID with dot"), false, "urn:test.nid:value", null },
        { TestFileLine("NID with underscore"), false, "urn:test_nid:value", null },
        { TestFileLine("NID with space"), false, "urn:test nid:value", null },
        { TestFileLine("NID starting with hyphen"), false, "urn:-test:value", null },
        { TestFileLine("NID ending with hyphen"), false, "urn:test-:value", null },
        { TestFileLine("NID with special chars"), false, "urn:test@nid:value", null },
        { TestFileLine("NID with non-ASCII"), false, "urn:tést:value", null },

        // NID length tests
        { TestFileLine("NID 31 chars"), true, "urn:a123456789012345678901234567890:v", new()
            {
                [Urns.AssignedNameGr] = "urn:a123456789012345678901234567890:v",
                [Urns.NidGr] = "a123456789012345678901234567890",
                [Urns.NssGr] = "v"
            }
        },
        { TestFileLine("NID 32 chars"), true, "urn:a1234567890123456789012345678901:v", new()
            {
                [Urns.AssignedNameGr] = "urn:a1234567890123456789012345678901:v",
                [Urns.NidGr] = "a1234567890123456789012345678901",
                [Urns.NssGr] = "v"
            }
        },

        // NSS validation tests
        { TestFileLine("NSS with alphanumeric"), true, "urn:test:abc123", new()
            {
                [Urns.AssignedNameGr] = "urn:test:abc123",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "abc123"
            }
        },
        { TestFileLine("NSS with special chars"), true, "urn:test:abc-123+def", new()
            {
                [Urns.AssignedNameGr] = "urn:test:abc-123+def",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "abc-123+def"
            }
        },
        { TestFileLine("NSS with colons"), true, "urn:test:ns:path:to:resource", new()
            {
                [Urns.AssignedNameGr] = "urn:test:ns:path:to:resource",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "ns:path:to:resource"
            }
        },
        { TestFileLine("NSS with percent encoding"), true, "urn:test:path%20with%20spaces", new()
            {
                [Urns.AssignedNameGr] = "urn:test:path%20with%20spaces",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "path%20with%20spaces"
            }
        },
        { TestFileLine("NSS with dots"), true, "urn:test:file.ext", new()
            {
                [Urns.AssignedNameGr] = "urn:test:file.ext",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "file.ext"
            }
        },
        { TestFileLine("NSS with slashes"), true, "urn:test:path/to/resource", new()
            {
                [Urns.AssignedNameGr] = "urn:test:path/to/resource",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "path/to/resource"
            }
        },
        { TestFileLine("NSS with query-like chars"), false, "urn:test:resource?param=value", null },
        { TestFileLine("NSS with fragment-like chars"), false, "urn:test:resource#fragment", null },
        { TestFileLine("NSS with empty string"), false, "urn:test:", null },
        { TestFileLine("NSS with only percent encoding"), true, "urn:test:%41%42%43", new()
            {
                [Urns.AssignedNameGr] = "urn:test:%41%42%43",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "%41%42%43"
            }
        },
        { TestFileLine("NSS with reserved URI chars"), false, "urn:test:foo/bar?baz#qux", null },

        // Real-world URN examples
        { TestFileLine("ISBN URN"), true, "urn:isbn:0451450523", new()
            {
                [Urns.AssignedNameGr] = "urn:isbn:0451450523",
                [Urns.NidGr] = "isbn",
                [Urns.NssGr] = "0451450523"
            }
        },
        { TestFileLine("UUID URN"), true, "urn:uuid:6e8bc430-9c3a-11d9-9669-0800200c9a66", new()
            {
                [Urns.AssignedNameGr] = "urn:uuid:6e8bc430-9c3a-11d9-9669-0800200c9a66",
                [Urns.NidGr] = "uuid",
                [Urns.NssGr] = "6e8bc430-9c3a-11d9-9669-0800200c9a66"
            }
        },
        { TestFileLine("OID URN"), true, "urn:oid:1.2.840.113549", new()
            {
                [Urns.AssignedNameGr] = "urn:oid:1.2.840.113549",
                [Urns.NidGr] = "oid",
                [Urns.NssGr] = "1.2.840.113549"
            }
        },
        { TestFileLine("DOI URN"), true, "urn:doi:10.1000/182", new()
            {
                [Urns.AssignedNameGr] = "urn:doi:10.1000/182",
                [Urns.NidGr] = "doi",
                [Urns.NssGr] = "10.1000/182"
            }
        },
        { TestFileLine("IETF RFC URN"), true, "urn:ietf:rfc:2141", new()
            {
                [Urns.AssignedNameGr] = "urn:ietf:rfc:2141",
                [Urns.NidGr] = "ietf",
                [Urns.NssGr] = "rfc:2141"
            }
        },

        // Custom schema URN
        { TestFileLine("Custom schema URN"), true, "urn:schemas-vm-com:Linq-Expressions-Serialization-Json", new()
            {
                [Urns.AssignedNameGr] = "urn:schemas-vm-com:Linq-Expressions-Serialization-Json",
                [Urns.NidGr] = "schemas-vm-com",
                [Urns.NssGr] = "Linq-Expressions-Serialization-Json"
            }
        },

        // Edge cases with excluded characters in NSS
        { TestFileLine("NSS with control chars"), false, "urn:test:abc\x01def", null },
        { TestFileLine("NSS with backslash"), false, "urn:test:path\\to\\resource", null },
        { TestFileLine("NSS with quotes"), false, "urn:test:\"quoted\"", null },
        { TestFileLine("NSS with angle brackets"), false, "urn:test:abc<def>ghi", null },
        { TestFileLine("NSS with square brackets"), false, "urn:test:abc[def]ghi", null },
        { TestFileLine("NSS with curly braces"), false, "urn:test:abc{def}ghi", null },
        { TestFileLine("NSS with pipe"), false, "urn:test:abc|def", null },
        { TestFileLine("NSS with caret"), false, "urn:test:abc^def", null },
        { TestFileLine("NSS with backtick"), false, "urn:test:abc`def", null },
        { TestFileLine("NSS with tilde"), true, "urn:test:abc~def", new() {
                [Urns.AssignedNameGr] = "urn:test:abc~def",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "abc~def",
            }
        },
        { TestFileLine("NSS with space"), false, "urn:test:abc def", null },
        { TestFileLine("NSS with high-bit chars"), false, "urn:test:abcÿdef", null },

        // Whitespace and formatting edge cases
        { TestFileLine("Leading whitespace"), false, " urn:test:value", null },
        { TestFileLine("Trailing whitespace"), false, "urn:test:value ", null },
        { TestFileLine("Internal whitespace in scheme"), false, "ur n:test:value", null },
        { TestFileLine("Internal whitespace in NID"), false, "urn:te st:value", null },
        { TestFileLine("Extra colons"), false, "urn::test:value", null },
        { TestFileLine("Missing scheme"), false, "test:value", null },
        { TestFileLine("Wrong scheme"), false, "uri:test:value", null },

        // Long NSS values
        { TestFileLine("Very long NSS"), true, "urn:test:" + new string('a', 1000), new()
            {
                [Urns.AssignedNameGr] = "urn:test:" + new string('a', 1000),
                [Urns.NidGr] = "test",
                [Urns.NssGr] = new string('a', 1000)
            }
        },

        // Percent encoding edge cases
        { TestFileLine("NSS with uppercase hex"), true, "urn:test:abc%2Fdef", new()
            {
                [Urns.AssignedNameGr] = "urn:test:abc%2Fdef",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "abc%2Fdef"
            }
        },
        { TestFileLine("NSS with lowercase hex"), true, "urn:test:abc%2fdef", new()
            {
                [Urns.AssignedNameGr] = "urn:test:abc%2fdef",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "abc%2fdef"
            }
        },
        { TestFileLine("NSS with invalid percent encoding"), false, "urn:test:abc%2", null },
        { TestFileLine("NSS with invalid percent encoding char"), false, "urn:test:abc%2G", null },

        // RFC 8141 edge cases (should not match AssignedName)
        { TestFileLine("URN with r-component"), false, "urn:example:foo?+bar", null },
        { TestFileLine("URN with q-component"), false, "urn:example:foo?=bar", null },
        { TestFileLine("URN with both r- and q-components"), false, "urn:example:foo?+bar?=baz", null },
        { TestFileLine("URN with fragment"), false, "urn:example:foo#frag", null },
        { TestFileLine("URN with all components"), false, "urn:example:foo?+bar?=baz#frag", null },
        { TestFileLine("URN with empty r-component"), false, "urn:example:foo?+", null },
        { TestFileLine("URN with empty q-component"), false, "urn:example:foo?=", null },
        { TestFileLine("URN with empty fragment"), false, "urn:example:foo#", null },
        { TestFileLine("URN with only fragment"), false, "urn:example:foo#", null },
        { TestFileLine("URN with only r-component"), false, "urn:example:foo?+", null },
        { TestFileLine("URN with only q-component"), false, "urn:example:foo?=", null },
    };

    [Theory]
    [MemberData(nameof(AssignedNameData))]
    public void TestAssignedName(string testLine, bool shouldMatch, string input, Captures? expectedCaptures)
        => base.RegexTest(Urns.AssignedName(), testLine, shouldMatch, input, expectedCaptures);

    // Repeat the same for NameStringData, using TestFileLine() for all entries
    public static UrnTheoryData NameStringData = new()
    {
        { TestFileLine("Simple"), true, "urn:example:foo", new()
            {
                [Urns.UrnGr] = "urn:example:foo",
                [Urns.AssignedNameGr] = "urn:example:foo",
                [Urns.NidGr] = "example",
                [Urns.NssGr] = "foo"
            }
        },
        { TestFileLine("With r-component"), true, "urn:example:foo?+bar", new()
            {
                [Urns.UrnGr] = "urn:example:foo?+bar",
                [Urns.AssignedNameGr] = "urn:example:foo",
                [Urns.NidGr] = "example",
                [Urns.NssGr] = "foo",
                [Urns.RComponentGr] = "bar"
            }
        },
        { TestFileLine("With q-component"), true, "urn:example:foo?=bar", new()
            {
                [Urns.UrnGr] = "urn:example:foo?=bar",
                [Urns.AssignedNameGr] = "urn:example:foo",
                [Urns.NidGr] = "example",
                [Urns.NssGr] = "foo",
                [Urns.QComponentGr] = "bar"
            }
        },
        { TestFileLine("With both r- and q-components"), true, "urn:example:foo?+bar?=baz", new()
            {
                [Urns.UrnGr] = "urn:example:foo?+bar?=baz",
                [Urns.AssignedNameGr] = "urn:example:foo",
                [Urns.NidGr] = "example",
                [Urns.NssGr] = "foo",
                [Urns.RComponentGr] = "bar",
                [Urns.QComponentGr] = "baz"
            }
        },
        { TestFileLine("With fragment"), true, "urn:example:foo#frag", new()
            {
                [Urns.UrnGr] = "urn:example:foo#frag",
                [Urns.AssignedNameGr] = "urn:example:foo",
                [Urns.NidGr] = "example",
                [Urns.NssGr] = "foo",
                [Urns.FComponentGr] = "frag"
            }
        },
        { TestFileLine("With all components"), true, "urn:example:foo?+bar?=baz#frag", new()
            {
                [Urns.UrnGr] = "urn:example:foo?+bar?=baz#frag",
                [Urns.AssignedNameGr] = "urn:example:foo",
                [Urns.NidGr] = "example",
                [Urns.NssGr] = "foo",
                [Urns.RComponentGr] = "bar",
                [Urns.QComponentGr] = "baz",
                [Urns.FComponentGr] = "frag"
            }
        },
        { TestFileLine("With empty r-component"), false, "urn:example:foo?+", null },
        { TestFileLine("With empty q-component"), false, "urn:example:foo?=", null },
        { TestFileLine("With empty fragment"), true, "urn:example:foo#", new()
            {
                [Urns.UrnGr] = "urn:example:foo#",
                [Urns.AssignedNameGr] = "urn:example:foo",
                [Urns.NidGr] = "example",
                [Urns.NssGr] = "foo",
                [Urns.FComponentGr] = ""
            }
        },

        // Invalid URNs
        { TestFileLine("Missing NID"), false, "urn::foo", null },
        { TestFileLine("Missing NSS"), false, "urn:foo:", null },
        { TestFileLine("NID with invalid char"), false, "urn:foo@bar:baz", null },
        { TestFileLine("NID too short"), false, "urn:a:b", new()
            {
                [Urns.UrnGr] = "urn:a:b",
                [Urns.AssignedNameGr] = "urn:a:b",
                [Urns.NidGr] = "a",
                [Urns.NssGr] = "b"
            }
        }, // Accept if regex allows single-char NID
        { TestFileLine("Wrong scheme"), false, "uri:example:foo", null },
        { TestFileLine("Extra colons"), false, "urn::test:value", null },
        { TestFileLine("Whitespace in NID"), false, "urn:te st:value", null },
        { TestFileLine("Whitespace in NSS"), false, "urn:test:abc def", null },

        // Edge cases
        { TestFileLine("Very long NSS"), true, "urn:test:" + new string('a', 1000), new()
            {
                [Urns.UrnGr] = "urn:test:" + new string('a', 1000),
                [Urns.AssignedNameGr] = "urn:test:" + new string('a', 1000),
                [Urns.NidGr] = "test",
                [Urns.NssGr] = new string('a', 1000)
            }
        },
        { TestFileLine("NSS with only percent encoding"), true, "urn:test:%41%42%43", new()
            {
                [Urns.UrnGr] = "urn:test:%41%42%43",
                [Urns.AssignedNameGr] = "urn:test:%41%42%43",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "%41%42%43"
            }
        },
        { TestFileLine("NSS with reserved URI chars"), false, "urn:test:foo/bar?baz#qux", null },
        { TestFileLine("NSS with invalid percent encoding"), false, "urn:test:abc%2", null },
        { TestFileLine("NSS with invalid percent encoding char"), false, "urn:test:abc%2G", null },
    };

    [Theory]
    [MemberData(nameof(AssignedNameData))]
    public void TestAssignedName2(string testLine, bool shouldMatch, string input, Captures? expectedCaptures)
        => base.RegexTest(Urns.AssignedName(), testLine, shouldMatch, input, expectedCaptures);

    [Theory]
    [MemberData(nameof(NameStringData))]
    public void TestNameString(string testLine, bool shouldMatch, string input, Captures? expectedCaptures)
        => base.RegexTest(Urns.NameString(), testLine, shouldMatch, input, expectedCaptures);
}