﻿namespace vm2.RegexLibTests;

public class SemVerTests : RegexTests
{
    public SemVerTests(ITestOutputHelper output) : base(output) 
    {
    }

    public static TheoryData<string, bool, string> SemVerTestData => new TheoryData<string, bool, string>
    {
        { TestLine(), true,  "0.0.4" },
        { TestLine(), true,  "1.2.3" },
        { TestLine(), true,  "10.20.30" },
        { TestLine(), true,  "1.1.2-prerelease+meta" },
        { TestLine(), true,  "1.1.3-123.abc.0+meta" },
        { TestLine(), true,  "1.1.3-prerelease+123.abc.0" },
        { TestLine(), true,  "1.1.2+meta" },
        { TestLine(), true,  "1.1.2+meta-valid" },
        { TestLine(), true,  "1.0.0-alpha" },
        { TestLine(), true,  "1.0.0-beta" },
        { TestLine(), true,  "1.0.0-alpha.beta" },
        { TestLine(), true,  "1.0.0-alpha.beta.1" },
        { TestLine(), true,  "1.0.0-alpha.1" },
        { TestLine(), true,  "1.0.0-alpha0.valid" },
        { TestLine(), true,  "1.0.0-alpha.0valid" },
        { TestLine(), true,  "1.0.0-alpha-a.b-c-somethinglong+build.1-aef.1-its-okay" },
        { TestLine(), true,  "1.0.0-rc.1+build.1" },
        { TestLine(), true,  "2.0.0-rc.1+build.123" },
        { TestLine(), true,  "1.2.3-beta" },
        { TestLine(), true,  "10.2.3-DEV-SNAPSHOT" },
        { TestLine(), true,  "1.2.3-SNAPSHOT-123" },
        { TestLine(), true,  "1.0.0" },
        { TestLine(), true,  "2.0.0" },
        { TestLine(), true,  "1.1.7" },
        { TestLine(), true,  "2.0.0+build.1848" },
        { TestLine(), true,  "2.0.1-alpha.1227" },
        { TestLine(), true,  "1.0.0-alpha+beta" },
        { TestLine(), true,  "1.2.3----RC-SNAPSHOT.12.9.1--.12+788" },
        { TestLine(), true,  "1.2.3----R-S.12.9.1--.12+meta" },
        { TestLine(), true,  "1.2.3----RC-SNAPSHOT.12.9.1--.12" },
        { TestLine(), true,  "1.0.0+0.build.1-rc.10000aaa-kk-0.1" },
        { TestLine(), true,  "99999999999999999999999.999999999999999999.99999999999999999" },
        { TestLine(), true,  "99999999999999999999999.999999999999999999.99999999999999999----RC-SNAPSHOT.12.0.9.1--------------------------------a.12" },
        { TestLine(), true,  "1.0.0-0A.is.legal" },
        { TestLine(), false, "1" },
        { TestLine(), false, "1.2" },
        { TestLine(), false, "1.2.3-0123" },
        { TestLine(), false, "1.2.3-0123.0123" },
        { TestLine(), false, "1.1.2+.123" },
        { TestLine(), false, "1.1.2+0123" },
        { TestLine(), true,  "1.1.2+0.123" },
        { TestLine(), false, "+invalid" },
        { TestLine(), false, "-invalid" },
        { TestLine(), false, "-invalid+invalid" },
        { TestLine(), false, "-invalid.01" },
        { TestLine(), false, "alpha" },
        { TestLine(), false, "alpha.beta" },
        { TestLine(), false, "alpha.beta.1" },
        { TestLine(), false, "alpha.1" },
        { TestLine(), false, "alpha+beta" },
        { TestLine(), false, "alpha_beta" },
        { TestLine(), false, "alpha." },
        { TestLine(), false, "alpha.." },
        { TestLine(), false, "beta" },
        { TestLine(), false, "1.0.0-alpha_beta" },
        { TestLine(), false, "-alpha." },
        { TestLine(), false, "1.0.0-alpha.." },
        { TestLine(), false, "1.0.0-alpha..1" },
        { TestLine(), false, "1.0.0-alpha...1" },
        { TestLine(), false, "1.0.0-alpha....1" },
        { TestLine(), false, "1.0.0-alpha.....1" },
        { TestLine(), false, "1.0.0-alpha......1" },
        { TestLine(), false, "1.0.0-alpha.......1" },
        { TestLine(), false, "01.1.1" },
        { TestLine(), false, "1.01.1" },
        { TestLine(), false, "1.1.01" },
        { TestLine(), false, "1.2" },
        { TestLine(), false, "1.2.3.DEV" },
        { TestLine(), false, "1.2-SNAPSHOT" },
        { TestLine(), false, "1.2.31.2.3----RC-SNAPSHOT.12.09.1--..12+788" },
        { TestLine(), false, "1.2-RC-SNAPSHOT" },
        { TestLine(), false, "-1.0.3-gamma+b7718" },
        { TestLine(), false, "+justmeta" },
        { TestLine(), false, "9.8.7+meta+meta" },
        { TestLine(), false, "9.8.7-whatever+meta+meta" },
        { TestLine(), false, "99999999999999999999999.999999999999999999.99999999999999999----RC-SNAPSHOT.12.09.1--------------------------------..12" },

    };

    [Theory]
    [MemberData(nameof(SemVerTestData))]
    public void TestWinFilename(string TestLine, bool shouldBe, string fileName)
        => base.RegexTest(SemVer.Regex, TestLine, shouldBe, fileName);
}