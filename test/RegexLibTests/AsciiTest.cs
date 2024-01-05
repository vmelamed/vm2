using System.Runtime.CompilerServices;

namespace vm2.tests.RegexLibTests;

public class AsciiTest : RegexTests
{
    public AsciiTest(ITestOutputHelper output) : base(output)
    {
    }

    public static TheoryData<string, bool, string> LowAlphaRexData => new TheoryData<string, bool, string>
    {
        { TestLine(), false, ""  },
        { TestLine(), true , "a" },
        { TestLine(), true , "z" },
        { TestLine(), false, "1" },
        { TestLine(), false, "A" },
        { TestLine(), false, "!" },
        { TestLine(), false, "ж" },
        { TestLine(), false, " " },
    };

    [Theory]
    [MemberData(nameof(LowAlphaRexData))]
    public void TestLowAlphaRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.LowAlphaRex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> HighAlphaRexData => new TheoryData<string, bool, string>
    {
        { TestLine(), false, ""  },
        { TestLine(), true , "A" },
        { TestLine(), true , "Z" },
        { TestLine(), false, "1" },
        { TestLine(), false, "a" },
        { TestLine(), false, "!" },
        { TestLine(), false, "Ж" },
        { TestLine(), false, " " },
    };

    [Theory]
    [MemberData(nameof(HighAlphaRexData))]
    public void TestHighAlphaRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.HighAlphaRex, TestLine, shouldBe, input);
    
    public static TheoryData<string, bool, string> AlphaRexData => new TheoryData<string, bool, string>
    {
        { TestLine(), false, ""  },
        { TestLine(), true, "A"  },
        { TestLine(), true, "Z"  },
        { TestLine(), true, "a"  },
        { TestLine(), true, "z"  },
        { TestLine(), false, "1" },
        { TestLine(), false, "!" },
        { TestLine(), false, "Ж" },
        { TestLine(), false, "ж" },
        { TestLine(), false, " " },
    };

    [Theory]
    [MemberData(nameof(AlphaRexData))]
    public void TestAlphaRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.AlphaRex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> Base64CharRexData => new TheoryData<string, bool, string>
    {
        { TestLine(), false, ""   },
        { TestLine(), true , "A"  },
        { TestLine(), true , "Z"  },
        { TestLine(), true , "a"  },
        { TestLine(), true , "z"  },
        { TestLine(), true , "0"  },
        { TestLine(), true , "9"  },
        { TestLine(), true , "/"  },
        { TestLine(), true , "+"  },
        { TestLine(), false, "!"  },
        { TestLine(), false, "Ж"  },
        { TestLine(), false, "ж"  },
        { TestLine(), false, " "  },
        { TestLine(), false, "\r" },
        { TestLine(), false, "\n" },
    };

    [Theory]
    [MemberData(nameof(Base64CharRexData))]
    public void TestBase64CharRex(string TestLine, bool shouldBe, string input) 
        => base.RegexStringTest(Ascii.Base64CharRex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> Base64Data => new TheoryData<string, bool, string>
    {
        { TestLine(), true,  "" },
        { TestLine(), true,  "TWFueSBoYW5kcyBtYWtlIGxpZ2h0IHdvcmsu" },
        { TestLine(), false, "!" },
        { TestLine(), true,  """
                                IA==
                            
                                """ },
        { TestLine(), false, """
                                I!A==
  
                                """ },
        { TestLine(), false, """
                                !IA==
                   
                                """ },
        { TestLine(), false, """
                                !IA==!
                                
                                """ },
        { TestLine(), true,  """
                                IAmg455a
                                12tGb7/+
                                
                                """ },
        { TestLine(), false, """
                                I!Amg455a
                                12tGb7/+
                                
                                """ },
        { TestLine(), false, """
                                !IAmg455a
                                12tGb7/+
                                
                                """ },
        { TestLine(), false, """
                                IAmg455a!
                                12tGb7/+
                                
                                """ },
        { TestLine(), true,  """
                                IAmg455a
                                12tGb7/+
                                IA==
                                
                                """ },
        { TestLine(), false, """
                                IAmg455a
                                12tGb7/+
                                !IA==
                                
                                """ },
        { TestLine(), false, """
                                IAmg455a
                                1!2tGb7/+
                                IA==
                                
                                """ },
        { TestLine(), false, """
                                IAmg455a
                                12tGb7/+!
                                IA==
                                
                                """ },
        { TestLine(), false, """
                                IAmg455a
                                12tGb7/+
                                !IA==
                                
                                """ },
        { TestLine(), false, """
                                IAmg455a
                                12tGb7/+
                                IA==!
                                
                                """ },
        { TestLine(), false, """
                                IAmg455a
                                12tGb7/+
                                IA=!=
                                
                                """ },
        { TestLine(), false, """
                                IAmg455a
                                12tGb7/+
                                I!A==
                                
                                """ },
    };

    [Theory]
    [MemberData(nameof(Base64Data))]
    public void TestBase64(string TestLine, bool shouldBe, string input) 
        => base.RegexTest(Ascii.Base64, TestLine, shouldBe, input);
}