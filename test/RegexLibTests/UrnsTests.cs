namespace vm2.RegexLibTests;

public class UrnTheoryData : TheoryData<string, bool, string, Captures?>
{
}

public partial class UrnsTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    /// <summary>
    /// Test data for URN validation based on RFC 2141.
    /// Tests the complete URN regex: urn:&lt;NID&gt;:&lt;NSS&gt;
    /// </summary>
    public static UrnTheoryData UrnData = new() {

        // Empty and whitespace cases
        { TestFileLine("Empty string"), false, "", null },
        { TestFileLine("Just spaces"), false, "   ", null },
        { TestFileLine("Just urn"), false, "urn", null },
        { TestFileLine("urn with one colon"), false, "urn:", null },
        { TestFileLine("urn with two colons but no NID"), false, "urn::", null },
        { TestFileLine("urn with two colons but no NSS"), false, "urn:nid:", null },

        // Case sensitivity tests (URN scheme should be case-insensitive)
        { TestFileLine("Uppercase URN"), true, "URN:example:test", new() {
            ["urn"] = "URN:example:test",
            [Urns.NidGr] = "example",
            [Urns.NssGr] = "test"
        }},
        { TestFileLine("Mixed case URN"), true, "Urn:example:test", new() {
            ["urn"] = "Urn:example:test",
            [Urns.NidGr] = "example",
            [Urns.NssGr] = "test"
        }},

        // Valid minimal URNs
        { TestFileLine("Minimal valid URN"), true, "urn:a:b", new() {
            ["urn"] = "urn:a:b",
            [Urns.NidGr] = "a",
            [Urns.NssGr] = "b"
        }},
        { TestFileLine("Single char NID and NSS"), true, "urn:x:y", new() {
            ["urn"] = "urn:x:y",
            [Urns.NidGr] = "x",
            [Urns.NssGr] = "y"
        }},

        // NID validation tests (RFC 2141: alphanumeric, may contain hyphens, 2-32 chars)
        { TestFileLine("NID with numbers"), true, "urn:test123:value", new() {
            ["urn"] = "urn:test123:value",
            [Urns.NidGr] = "test123",
            [Urns.NssGr] = "value"
        }},
        { TestFileLine("NID with hyphens"), true, "urn:test-nid:value", new() {
            ["urn"] = "urn:test-nid:value",
            [Urns.NidGr] = "test-nid",
            [Urns.NssGr] = "value"
        }},
        { TestFileLine("NID starting with number"), true, "urn:1test:value", new() {
            ["urn"] = "urn:1test:value",
            [Urns.NidGr] = "1test",
            [Urns.NssGr] = "value"
        }},
        { TestFileLine("NID all numbers"), true, "urn:123:value", new() {
            ["urn"] = "urn:123:value",
            [Urns.NidGr] = "123",
            [Urns.NssGr] = "value"
        }},

        // Invalid NID tests
        { TestFileLine("NID with dot"), false, "urn:test.nid:value", null },
        { TestFileLine("NID with underscore"), false, "urn:test_nid:value", null },
        { TestFileLine("NID with space"), false, "urn:test nid:value", null },
        { TestFileLine("NID starting with hyphen"), false, "urn:-test:value", null },
        { TestFileLine("NID ending with hyphen"), false, "urn:test-:value", null },
        { TestFileLine("NID with special chars"), false, "urn:test@nid:value", null },

        // NID length tests (should be 1-31 characters based on regex pattern)
        { TestFileLine("NID 31 chars"), true, "urn:a123456789012345678901234567890:v", new() {
            ["urn"] = "urn:a123456789012345678901234567890:v",
            [Urns.NidGr] = "a123456789012345678901234567890",
            [Urns.NssGr] = "v"
        }},
        { TestFileLine("NID 32 chars"), false, "urn:a1234567890123456789012345678901:v", null },

        // NSS validation tests (more permissive than NID)
        { TestFileLine("NSS with alphanumeric"), true, "urn:test:abc123", new() {
            ["urn"] = "urn:test:abc123",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "abc123"
        }},
        { TestFileLine("NSS with special chars"), true, "urn:test:abc-123+def", new() {
            ["urn"] = "urn:test:abc-123+def",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "abc-123+def"
        }},
        { TestFileLine("NSS with colons"), true, "urn:test:ns:path:to:resource", new() {
            ["urn"] = "urn:test:ns:path:to:resource",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "ns:path:to:resource"
        }},
        { TestFileLine("NSS with percent encoding"), true, "urn:test:path%20with%20spaces", new() {
            ["urn"] = "urn:test:path%20with%20spaces",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "path%20with%20spaces"
        }},
        { TestFileLine("NSS with dots"), true, "urn:test:file.ext", new() {
            ["urn"] = "urn:test:file.ext",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "file.ext"
        }},
        { TestFileLine("NSS with slashes"), true, "urn:test:path/to/resource", new() {
            ["urn"] = "urn:test:path/to/resource",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "path/to/resource"
        }},
        { TestFileLine("NSS with query-like chars"), true, "urn:test:resource?param=value", new() {
            ["urn"] = "urn:test:resource?param=value",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "resource?param=value"
        }},
        { TestFileLine("NSS with fragment-like chars"), true, "urn:test:resource#fragment", new() {
            ["urn"] = "urn:test:resource#fragment",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "resource#fragment"
        }},

        // Real-world URN examples
        { TestFileLine("ISBN URN"), true, "urn:isbn:0451450523", new() {
            ["urn"] = "urn:isbn:0451450523",
            [Urns.NidGr] = "isbn",
            [Urns.NssGr] = "0451450523"
        }},
        { TestFileLine("UUID URN"), true, "urn:uuid:6e8bc430-9c3a-11d9-9669-0800200c9a66", new() {
            ["urn"] = "urn:uuid:6e8bc430-9c3a-11d9-9669-0800200c9a66",
            [Urns.NidGr] = "uuid",
            [Urns.NssGr] = "6e8bc430-9c3a-11d9-9669-0800200c9a66"
        }},
        { TestFileLine("OID URN"), true, "urn:oid:1.2.840.113549", new() {
            ["urn"] = "urn:oid:1.2.840.113549",
            [Urns.NidGr] = "oid",
            [Urns.NssGr] = "1.2.840.113549"
        }},
        { TestFileLine("DOI URN"), true, "urn:doi:10.1000/182", new() {
            ["urn"] = "urn:doi:10.1000/182",
            [Urns.NidGr] = "doi",
            [Urns.NssGr] = "10.1000/182"
        }},
        { TestFileLine("IETF RFC URN"), true, "urn:ietf:rfc:2141", new() {
            ["urn"] = "urn:ietf:rfc:2141",
            [Urns.NidGr] = "ietf",
            [Urns.NssGr] = "rfc:2141"
        }},

        // Custom schema URN (like yours)
        { TestFileLine("Custom schema URN"), true, "urn:schemas-vm-com:Linq-Expressions-Serialization-Json", new() {
            ["urn"] = "urn:schemas-vm-com:Linq-Expressions-Serialization-Json",
            [Urns.NidGr] = "schemas-vm-com",
            [Urns.NssGr] = "Linq-Expressions-Serialization-Json"
        }},

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
        { TestFileLine("NSS with tilde"), false, "urn:test:abc~def", null },
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
        { TestFileLine("Very long NSS"), true, "urn:test:" + new string('a', 1000), new() {
            ["urn"] = "urn:test:" + new string('a', 1000),
            [Urns.NidGr] = "test",
            [Urns.NssGr] = new string('a', 1000)
        }},

        // Percent encoding edge cases
        { TestFileLine("NSS with uppercase hex"), true, "urn:test:abc%2Fdef", new() {
            ["urn"] = "urn:test:abc%2Fdef",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "abc%2Fdef"
        }},
        { TestFileLine("NSS with lowercase hex"), true, "urn:test:abc%2fdef", new() {
            ["urn"] = "urn:test:abc%2fdef",
            [Urns.NidGr] = "test",
            [Urns.NssGr] = "abc%2fdef"
        }},
        { TestFileLine("NSS with invalid percent encoding"), false, "urn:test:abc%2", null },
        { TestFileLine("NSS with invalid percent encoding char"), false, "urn:test:abc%2G", null },
    };

    [Theory]
    [MemberData(nameof(UrnData))]
    public void TestUrn(string testFileLine, bool shouldMatch, string input, Captures? expectedCaptures)
        => base.RegexTest(Urns.Urn(), testFileLine, shouldMatch, input, expectedCaptures);
}
