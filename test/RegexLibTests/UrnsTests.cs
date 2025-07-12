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

        // Invalid URNs
        { TestFileLine("Invalid short URN"), false, "urn:az:b", null },
        { TestFileLine("Invalid short URN"), false, "urn:-az:b", null },
        { TestFileLine("Invalid short URN"), false, "urn:az-:b", null },
        { TestFileLine("Invalid short URN"), false, "urn:urn-abc:b", null },
        { TestFileLine("Invalid short URN"), false, "urn:az-x:b", null },
        { TestFileLine("Invalid short URN"), false, "urn:X-abc:b", null },

        // Valid minimal URNs
        { TestFileLine("Minimal valid URN"), true, "urn:a1z:b", new()
            {
                [Urns.AssignedNameGr] = "urn:a1z:b",
                [Urns.NidGr] = "a1z",
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

        // from copilot:
        { TestFileLine("NID max length (32 chars)"), true, "urn:abcdefghijklmnopqrstuvwxyzabcdef:value", new()
            {
                [Urns.AssignedNameGr] = "urn:abcdefghijklmnopqrstuvwxyzabcdef:value",
                [Urns.NidGr] = "abcdefghijklmnopqrstuvwxyzabcdef",
                [Urns.NssGr] = "value"
            }
        },
        { TestFileLine("NID too long (33 chars)"), false, "urn:abcdefghijklmnopqrstuvwxyzabcdefg:value", null },

        { TestFileLine("NID with leading/trailing hyphen"), false, "urn:-abc-:value", null },

        { TestFileLine("NID with only one char"), false, "urn:a:value", null },

        { TestFileLine("NSS with only allowed punctuation"), true, "urn:test:.-_~", new()
            {
                [Urns.AssignedNameGr] = "urn:test:.-_~",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = ".-_~"
            }
        },

        { TestFileLine("NSS with consecutive percent encoding"), true, "urn:test:%41%42%43", new()
            {
                [Urns.AssignedNameGr] = "urn:test:%41%42%43",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "%41%42%43"
            }
        },

        { TestFileLine("NSS with slash at start"), false, "urn:test:/resource", null },

        { TestFileLine("NSS with multiple consecutive slashes"), true, "urn:test:path//to///resource", new()
            {
                [Urns.AssignedNameGr] = "urn:test:path//to///resource",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "path//to///resource"
            }
        },

        { TestFileLine("NSS with trailing slash"), true, "urn:test:resource/", new()
            {
                [Urns.AssignedNameGr] = "urn:test:resource/",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "resource/"
            }
        },

        { TestFileLine("NSS with empty percent encoding"), false, "urn:test:%", null },

        { TestFileLine("NSS with percent encoding and non-hex"), false, "urn:test:%GG", null },

        { TestFileLine("NSS with percent encoding and lowercase hex"), true, "urn:test:%2f", new()
            {
                [Urns.AssignedNameGr] = "urn:test:%2f",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "%2f"
            }
        },

        { TestFileLine("NSS with percent encoding and uppercase hex"), true, "urn:test:%2F", new()
            {
                [Urns.AssignedNameGr] = "urn:test:%2F",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "%2F"
            }
        },

        { TestFileLine("URN with whitespace between colons"), false, "urn:test :value", null },

        { TestFileLine("URN with tab character"), false, "urn:test:\tvalue", null },

        { TestFileLine("URN with newline character"), false, "urn:test:\nvalue", null },

        { TestFileLine("NID exactly 3 chars"), true, "urn:abc:value", new()
        {
            ["a_name"] = "urn:abc:value",
            ["nid"] = "abc",
            ["nss"] = "value",
        } },
        { TestFileLine("NID with hyphen not at start"), false, "urn:ab-cd:value", null },
        // NID with urn- or X- not at the start(valid):
        { TestFileLine("NID with urn- not at start"), true, "urn:abcurn-def:value", new()
        {
            ["a_name"] = "urn:abcurn-def:value",
            ["nid"] = "abcurn-def",
            ["nss"] = "value",
        } },
        // NSS with allowed percent-encoded reserved chars(valid) :
        { TestFileLine("NSS with percent-encoded reserved"), true, "urn:test:%3A%2F", new()
        {
            ["a_name"] = "urn:test:%3A%2F", // →a_name← = →urn:test::/←
            ["nid"] = "test",
            ["nss"] = "%3A%2F", // →nss← = →:/←
        } },
        // NSS with allowed tilde, dot, dash, underscore (valid):

        // NSS with only one character(valid) :
        { TestFileLine("NSS single char"), true, "urn:test:a", new()
        {
            ["a_name"] = "urn:test:a",
            ["nid"] = "test",
            ["nss"] = "a",
        } },
        // NID with mixed case (valid):
        { TestFileLine("NID mixed case"), true, "urn:AbC:value", new()
        {
            ["a_name"] = "urn:AbC:value",
            ["nid"] = "AbC",
            ["nss"] = "value",
        } },
        // NID with digits at start/end(valid) :
        { TestFileLine("NID starts/ends with digit"), true, "urn:1bc:value", new()
        {
            ["a_name"] = "urn:1bc:value",
            ["nid"] = "1bc",
            ["nss"] = "value",
        } },
        // NSS with percent-encoded non-ASCII(valid) :
        { TestFileLine("NSS percent-encoded non-ASCII"), true, "urn:test:%E2%82%AC", new()
        {
            ["a_name"] = "urn:test:%E2%82%AC", // →a_name← = →urn:test:€←
            ["nid"] = "test",
            ["nss"] = "%E2%82%AC", // →nss← = →€←
        } },
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

        // from copilot:

        { TestFileLine("URN with r-component and percent encoding"), true, "urn:test:foo?+bar%20baz", new()
            {
                [Urns.UrnGr] = "urn:test:foo?+bar%20baz",
                [Urns.AssignedNameGr] = "urn:test:foo",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "foo",
                [Urns.RComponentGr] = "bar%20baz"
            }
        },

        { TestFileLine("URN with q-component and percent encoding"), true, "urn:test:foo?=bar%20baz", new()
            {
                [Urns.UrnGr] = "urn:test:foo?=bar%20baz",
                [Urns.AssignedNameGr] = "urn:test:foo",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "foo",
                [Urns.QComponentGr] = "bar%20baz"
            }
        },

        { TestFileLine("URN with fragment and percent encoding"), true, "urn:test:foo#frag%20ment", new()
            {
                [Urns.UrnGr] = "urn:test:foo#frag%20ment",
                [Urns.AssignedNameGr] = "urn:test:foo",
                [Urns.NidGr] = "test",
                [Urns.NssGr] = "foo",
                [Urns.FComponentGr] = "frag%20ment"
            }
        },

        { TestFileLine("URN with empty r-component and valid q-component"), false, "urn:test:foo?+?=bar", null },

        { TestFileLine("URN with valid r-component and empty q-component"), false, "urn:test:foo?+bar?=", null },

        { TestFileLine("URN with only delimiters"), false, "urn:test:foo?+#", null },

        { TestFileLine("URN with multiple fragments"), false, "urn:test:foo#frag1#frag2", null },
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