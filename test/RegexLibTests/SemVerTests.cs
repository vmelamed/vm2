namespace vm2.RegexLibTests;

public class SemVerTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    public static TheoryData<string, bool, string, Captures?> SemVerTestData => new() {
        { TestFileLine(), true,  "0.0.4", new() {  ["core"] = "0.0.4", ["major"] = "0", ["minor"] = "0", ["patch"] = "4", ["pre"] = "", ["build"] = "" } },
        { TestFileLine(), true,  "1.2.3", new() {  ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "", ["build"] = "" } },
        { TestFileLine(), true,  "10.20.30", new() {  ["core"] = "10.20.30", ["major"] = "10", ["minor"] = "20", ["patch"] = "30", ["pre"] = "", ["build"] = "" } },
        { TestFileLine(), true,  "1.1.2-prerelease+meta", new() {  ["core"] = "1.1.2", ["major"] = "1", ["minor"] = "1", ["patch"] = "2", ["pre"] = "prerelease", ["build"] = "meta" } },
        { TestFileLine(), true,  "1.1.3-123.abc.0+meta", new() {  ["core"] = "1.1.3", ["major"] = "1", ["minor"] = "1", ["patch"] = "3", ["pre"] = "123.abc.0", ["build"] = "meta" } },
        { TestFileLine(), true,  "1.1.3-prerelease+123.abc.0", new() {  ["core"] = "1.1.3", ["major"] = "1", ["minor"] = "1", ["patch"] = "3", ["pre"] = "prerelease", ["build"] = "123.abc.0" } },
        { TestFileLine(), true,  "1.1.2+meta", new() {  ["core"] = "1.1.2", ["major"] = "1", ["minor"] = "1", ["patch"] = "2", ["pre"] = "", ["build"] = "meta" } },
        { TestFileLine(), true,  "1.1.2+meta-valid", new() {  ["core"] = "1.1.2", ["major"] = "1", ["minor"] = "1", ["patch"] = "2", ["pre"] = "", ["build"] = "meta-valid" } },
        { TestFileLine(), true,  "1.0.0-alpha", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "alpha", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0-beta", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "beta", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0-alpha.beta", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "alpha.beta", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0-alpha.beta.1", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "alpha.beta.1", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0-alpha.1", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "alpha.1", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0-alpha0.valid", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "alpha0.valid", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0-alpha.0valid", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "alpha.0valid", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0-alpha-a.b-c-somethinglong+build.1-aef.1-its-okay", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "alpha-a.b-c-somethinglong", ["build"] = "build.1-aef.1-its-okay" } },
        { TestFileLine(), true,  "1.0.0-rc.1+build.1", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "rc.1", ["build"] = "build.1" } },
        { TestFileLine(), true,  "2.0.0-rc.1+build.123", new() {  ["core"] = "2.0.0", ["major"] = "2", ["minor"] = "0", ["patch"] = "0", ["pre"] = "rc.1", ["build"] = "build.123" } },
        { TestFileLine(), true,  "1.2.3-beta", new() {  ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "beta", ["build"] = "" } },
        { TestFileLine(), true,  "10.2.3-DEV-SNAPSHOT", new() {  ["core"] = "10.2.3", ["major"] = "10", ["minor"] = "2", ["patch"] = "3", ["pre"] = "DEV-SNAPSHOT", ["build"] = "" } },
        { TestFileLine(), true,  "1.2.3-SNAPSHOT-123", new() {  ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "SNAPSHOT-123", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "", ["build"] = "" } },
        { TestFileLine(), true,  "2.0.0", new() {  ["core"] = "2.0.0", ["major"] = "2", ["minor"] = "0", ["patch"] = "0", ["pre"] = "", ["build"] = "" } },
        { TestFileLine(), true,  "1.1.7", new() {  ["core"] = "1.1.7", ["major"] = "1", ["minor"] = "1", ["patch"] = "7", ["pre"] = "", ["build"] = "" } },
        { TestFileLine(), true,  "2.0.0+build.1848", new() {  ["core"] = "2.0.0", ["major"] = "2", ["minor"] = "0", ["patch"] = "0", ["pre"] = "", ["build"] = "build.1848" } },
        { TestFileLine(), true,  "2.0.1-alpha.1227", new() {  ["core"] = "2.0.1", ["major"] = "2", ["minor"] = "0", ["patch"] = "1", ["pre"] = "alpha.1227", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0-alpha+beta", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "alpha", ["build"] = "beta" } },
        { TestFileLine(), true,  "1.2.3----RC-SNAPSHOT.12.9.1--.12+788", new() {  ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "---RC-SNAPSHOT.12.9.1--.12", ["build"] = "788" } },
        { TestFileLine(), true,  "1.2.3----R-S.12.9.1--.12+meta",        new() {  ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "---R-S.12.9.1--.12", ["build"] = "meta" } },
        { TestFileLine(), true,  "1.2.3----RC-SNAPSHOT.12.9.1--.12",     new() {  ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "---RC-SNAPSHOT.12.9.1--.12", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0+0.build.1-rc.10000aaa-kk-0.1",   new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "", ["build"] = "0.build.1-rc.10000aaa-kk-0.1" } },
        { TestFileLine(), true,  "99999999999999999999999.999999999999999999.99999999999999999",                                                             new() {  ["core"] = "99999999999999999999999.999999999999999999.99999999999999999", ["major"] = "99999999999999999999999", ["minor"] = "999999999999999999", ["patch"] = "99999999999999999", ["pre"] = "", ["build"] = "" } },
        { TestFileLine(), true,  "99999999999999999999999.999999999999999999.99999999999999999----RC-SNAPSHOT.12.0.9.1--------------------------------a.12", new() {  ["core"] = "99999999999999999999999.999999999999999999.99999999999999999", ["major"] = "99999999999999999999999", ["minor"] = "999999999999999999", ["patch"] = "99999999999999999", ["pre"] = "---RC-SNAPSHOT.12.0.9.1--------------------------------a.12", ["build"] = "" } },
        { TestFileLine(), true,  "1.0.0-0A.is.legal", new() {  ["core"] = "1.0.0", ["major"] = "1", ["minor"] = "0", ["patch"] = "0", ["pre"] = "0A.is.legal", ["build"] = "" } },
        { TestFileLine(), false, "1", null },
        { TestFileLine(), false, "1.2", null },
        { TestFileLine(), true,  "1.2.3-0123", new()
        {
            ["core"] = "1.2.3",
            ["major"] = "1",
            ["minor"] = "2",
            ["patch"] = "3",
            ["pre"] = "0123",
        } },
        { TestFileLine(), true,  "1.2.3-0123.0123", new()
        {
            ["core"] = "1.2.3",
            ["major"] = "1",
            ["minor"] = "2",
            ["patch"] = "3",
            ["pre"] = "0123.0123",
        } },
        { TestFileLine(), false, "1.1.2+.123", null },
        { TestFileLine(), true,  "1.1.2+0123", new()
        {
            ["core"] = "1.1.2",
            ["major"] = "1",
            ["minor"] = "1",
            ["patch"] = "2",
            ["build"] = "0123",
        } },
        { TestFileLine(), true,  "1.1.2+0.123", new() {  ["core"] = "1.1.2", ["major"] = "1", ["minor"] = "1", ["patch"] = "2", ["pre"] = "", ["build"] = "0.123" } },
        { TestFileLine(), false, "+invalid", null },
        { TestFileLine(), false, "-invalid", null },
        { TestFileLine(), false, "-invalid+invalid", null },
        { TestFileLine(), false, "-invalid.01", null },
        { TestFileLine(), false, "alpha", null },
        { TestFileLine(), false, "alpha.beta", null },
        { TestFileLine(), false, "alpha.beta.1", null },
        { TestFileLine(), false, "alpha.1", null },
        { TestFileLine(), false, "alpha+beta", null },
        { TestFileLine(), false, "alpha_beta", null },
        { TestFileLine(), false, "alpha.", null },
        { TestFileLine(), false, "alpha..", null },
        { TestFileLine(), false, "beta", null },
        { TestFileLine(), false, "1.0.0-alpha_beta", null },
        { TestFileLine(), false, "-alpha.", null },
        { TestFileLine(), false, "1.0.0-alpha..", null },
        { TestFileLine(), false, "1.0.0-alpha..1", null },
        { TestFileLine(), false, "1.0.0-alpha...1", null },
        { TestFileLine(), false, "1.0.0-alpha....1", null },
        { TestFileLine(), false, "1.0.0-alpha.....1", null },
        { TestFileLine(), false, "1.0.0-alpha......1", null },
        { TestFileLine(), false, "1.0.0-alpha.......1", null },
        { TestFileLine(), false, "01.1.1", null },
        { TestFileLine(), false, "1.01.1", null },
        { TestFileLine(), false, "1.1.01", null },
        { TestFileLine(), false, "1.2", null },
        { TestFileLine(), false, "1.2.3.DEV", null },
        { TestFileLine(), false, "1.2-SNAPSHOT", null },
        { TestFileLine(), false, "1.2.31.2.3----RC-SNAPSHOT.12.09.1--..12+788", null },
        { TestFileLine(), false, "1.2-RC-SNAPSHOT", null },
        { TestFileLine(), false, "-1.0.3-gamma+b7718", null },
        { TestFileLine(), false, "+justmeta", null },
        { TestFileLine(), false, "9.8.7+meta+meta", null },
        { TestFileLine(), false, "9.8.7-whatever+meta+meta", null },
        { TestFileLine(), false, "99999999999999999999999.999999999999999999.99999999999999999----RC-SNAPSHOT.12.09.1--------------------------------..12", null },

        // Edge: Maximum allowed identifier length (per semver.org, no explicit max, but test a long one)
        { TestFileLine("long pre-release"), true, "1.2.3-" + new string('a', 100) + "+build", new() { ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = new string('a', 100), ["build"] = "build" } },
        { TestFileLine("long build"), true, "1.2.3+build" + new string('b', 100), new() { ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "", ["build"] = "build" + new string('b', 100) } },

        // Edge: Pre-release and build with hyphens and dots
        { TestFileLine("pre-release with hyphens and dots"), true, "1.2.3-alpha-1.2.3", new() { ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "alpha-1.2.3", ["build"] = "" } },
        { TestFileLine("build with hyphens and dots"), true, "1.2.3+build-1.2.3", new() { ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "", ["build"] = "build-1.2.3" } },

        // Edge: Pre-release numeric identifier must not have leading zeroes
        { TestFileLine("pre-release numeric leading zero"), true, "1.2.3-01", new()
        {
            ["core"] = "1.2.3",
            ["major"] = "1",
            ["minor"] = "2",
            ["patch"] = "3",
            ["pre"] = "01",
        } },
        { TestFileLine("pre-release numeric leading zero in dot"), true, "1.2.3-alpha.01", new()
        {
            ["core"] = "1.2.3",
            ["major"] = "1",
            ["minor"] = "2",
            ["patch"] = "3",
            ["pre"] = "alpha.01",
        } },

        // Edge: Build metadata can have leading zeroes
        { TestFileLine("build numeric leading zero"), true, "1.2.3+01", new() { ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "", ["build"] = "01" } },

        // Edge: Pre-release with only digits (no leading zero)
        { TestFileLine("pre-release numeric no leading zero"), true, "1.2.3-1", new() { ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "1", ["build"] = "" } },

        // Edge: Pre-release with empty identifier (invalid)
        { TestFileLine("pre-release empty identifier"), false, "1.2.3-", null },
        { TestFileLine("pre-release dot empty identifier"), false, "1.2.3-.", null },
        { TestFileLine("pre-release double dot"), false, "1.2.3-..", null },

        // Edge: Build with empty identifier (invalid)
        { TestFileLine("build empty identifier"), false, "1.2.3+", null },
        { TestFileLine("build dot empty identifier"), false, "1.2.3+.", null },
        { TestFileLine("build double dot"), false, "1.2.3+..", null },

        // Edge: Unicode in identifiers (should be invalid per semver.org)
        { TestFileLine("unicode in pre-release"), false, "1.2.3-α", null },
        { TestFileLine("unicode in build"), false, "1.2.3+β", null },

        // Edge: Whitespace in identifiers (invalid)
        { TestFileLine("whitespace in pre-release"), false, "1.2.3-alpha beta", null },
        { TestFileLine("whitespace in build"), false, "1.2.3+build meta", null },

        // Edge: Leading/trailing whitespace (invalid)
        { TestFileLine("leading whitespace"), false, " 1.2.3", null },
        { TestFileLine("trailing whitespace"), false, "1.2.3 ", null },

        // Edge: Only major.minor.patch, but with extra dot (invalid)
        { TestFileLine("extra dot after patch"), false, "1.2.3.", null },

        // Edge: Pre-release and build with underscores (invalid)
        { TestFileLine("underscore in pre-release"), false, "1.2.3-alpha_beta", null },
        { TestFileLine("underscore in build"), false, "1.2.3+build_meta", null },

        // Edge: Pre-release and build with uppercase (valid)
        { TestFileLine("uppercase pre-release"), true, "1.2.3-ALPHA", new() { ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "ALPHA", ["build"] = "" } },
        { TestFileLine("uppercase build"), true, "1.2.3+BUILD", new() { ["core"] = "1.2.3", ["major"] = "1", ["minor"] = "2", ["patch"] = "3", ["pre"] = "", ["build"] = "BUILD" } },
    };

    [Theory]
    [MemberData(nameof(SemVerTestData))]
    public void TestSemVer(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(SemVer.SemanticVersion(), TestLine, shouldBe, input, captures);
}
