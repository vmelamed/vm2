namespace vm2.RegexLibTests;

public class AsciiTests(ITestOutputHelper output) : RegexTests(output)
{
    public static TheoryData<string, bool, string> LowAlphaRexData => new() {
        { TestFileLine(), false, ""  },
        { TestFileLine(), true , "a" },
        { TestFileLine(), true , "z" },
        { TestFileLine(), false, "1" },
        { TestFileLine(), false, "A" },
        { TestFileLine(), false, "!" },
        { TestFileLine(), false, "ж" },
        { TestFileLine(), false, " " },
    };

    [Theory]
    [MemberData(nameof(LowAlphaRexData))]
    public void TestLowAlphaRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.LowAlphaRex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> HighAlphaRexData => new() {
        { TestFileLine(), false, ""  },
        { TestFileLine(), true , "A" },
        { TestFileLine(), true , "Z" },
        { TestFileLine(), false, "1" },
        { TestFileLine(), false, "a" },
        { TestFileLine(), false, "!" },
        { TestFileLine(), false, "Ж" },
        { TestFileLine(), false, " " },
    };

    [Theory]
    [MemberData(nameof(HighAlphaRexData))]
    public void TestHighAlphaRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.HighAlphaRex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> AlphaRexData => new() {
        { TestFileLine(), false, ""  },
        { TestFileLine(), true, "A"  },
        { TestFileLine(), true, "Z"  },
        { TestFileLine(), true, "a"  },
        { TestFileLine(), true, "z"  },
        { TestFileLine(), false, "1" },
        { TestFileLine(), false, "!" },
        { TestFileLine(), false, "Ж" },
        { TestFileLine(), false, "ж" },
        { TestFileLine(), false, " " },
    };

    [Theory]
    [MemberData(nameof(AlphaRexData))]
    public void TestAlphaRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.AlphaRex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> Base64CharRexData => new() {
        { TestFileLine(), false, ""   },
        { TestFileLine(), true , "A"  },
        { TestFileLine(), true , "Z"  },
        { TestFileLine(), true , "a"  },
        { TestFileLine(), true , "z"  },
        { TestFileLine(), true , "0"  },
        { TestFileLine(), true , "9"  },
        { TestFileLine(), true , "/"  },
        { TestFileLine(), true , "+"  },
        { TestFileLine(), false, "!"  },
        { TestFileLine(), false, "Ж"  },
        { TestFileLine(), false, "ж"  },
        { TestFileLine(), false, " "  },
        { TestFileLine(), false, "\r" },
        { TestFileLine(), false, "\n" },
    };

    [Theory]
    [MemberData(nameof(Base64CharRexData))]
    public void TestBase64CharRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.Base64CharRex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> Base64Data => new() {
        { TestFileLine(), true,  "" },
        { TestFileLine(), true,  "TWFueSBoYW5kcyBtYWtlIGxpZ2h0IHdvcmsu" },
        { TestFileLine(), false, "!" },
        { TestFileLine(), true,  """
                                IA==

                                """ },
        { TestFileLine(), false, """
                                I!A==

                                """ },
        { TestFileLine(), false, """
                                !IA==

                                """ },
        { TestFileLine(), false, """
                                !IA==!

                                """ },
        { TestFileLine(), true,  """
                                IAmg455a
                                12tGb7/+

                                """ },
        { TestFileLine(), false, """
                                I!Amg455a
                                12tGb7/+

                                """ },
        { TestFileLine(), false, """
                                !IAmg455a
                                12tGb7/+

                                """ },
        { TestFileLine(), false, """
                                IAmg455a!
                                12tGb7/+

                                """ },
        { TestFileLine(), true,  """
                                IAmg455a
                                12tGb7/+
                                IA==

                                """ },
        { TestFileLine(), false, """
                                IAmg455a
                                12tGb7/+
                                !IA==

                                """ },
        { TestFileLine(), false, """
                                IAmg455a
                                1!2tGb7/+
                                IA==

                                """ },
        { TestFileLine(), false, """
                                IAmg455a
                                12tGb7/+!
                                IA==

                                """ },
        { TestFileLine(), false, """
                                IAmg455a
                                12tGb7/+
                                !IA==

                                """ },
        { TestFileLine(), false, """
                                IAmg455a
                                12tGb7/+
                                IA==!

                                """ },
        { TestFileLine(), false, """
                                IAmg455a
                                12tGb7/+
                                IA=!=

                                """ },
        { TestFileLine(), false, """
                                IAmg455a
                                12tGb7/+
                                I!A==

                                """ },
    };

    [Theory]
    [MemberData(nameof(Base64Data))]
    public void TestBase64(string TestLine, bool shouldBe, string input)
        => base.RegexTest(Ascii.Base64(), TestLine, shouldBe, input);
}