namespace vm2.tests.RegexLibTests;

public class AsciiTest : RegexTests
{
    public AsciiTest(ITestOutputHelper output) : base(output)
    {
    }

    public static TheoryData<string, bool, string> LowAlphaRexData
    {
        get
        {
            var id = 0;
            string testId() => id++.ToString("d2");

            return new TheoryData<string, bool, string>
            {
                { testId(), false, ""  },
                { testId(), true , "a" },
                { testId(), true , "z" },
                { testId(), false, "1" },
                { testId(), false, "A" },
                { testId(), false, "!" },
                { testId(), false, "ж" },
                { testId(), false, " " },
            };
        }
    }

    [Theory]
    [MemberData(nameof(LowAlphaRexData))]
    public void TestLowAlphaRex(string testId, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.LowAlphaRex, testId, shouldBe, input);

    public static TheoryData<string, bool, string> HighAlphaRexData
    {
        get
        {
            var id = 0;
            string testId() => id++.ToString("d2");

            return new TheoryData<string, bool, string>
            {
                { testId(), false, ""  },
                { testId(), true , "A" },
                { testId(), true , "Z" },
                { testId(), false, "1" },
                { testId(), false, "a" },
                { testId(), false, "!" },
                { testId(), false, "Ж" },
                { testId(), false, " " },
            };
        }
    }

    [Theory]
    [MemberData(nameof(HighAlphaRexData))]
    public void TestHighAlphaRex(string testId, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.HighAlphaRex, testId, shouldBe, input);
    
    public static TheoryData<string, bool, string> AlphaRexData
    {
        get
        {
            var id = 0;
            string testId() => id++.ToString("d2");

            return new TheoryData<string, bool, string>
            {
                { testId(), false, ""  },
                { testId(), true, "A"  },
                { testId(), true, "Z"  },
                { testId(), true, "a"  },
                { testId(), true, "z"  },
                { testId(), false, "1" },
                { testId(), false, "!" },
                { testId(), false, "Ж" },
                { testId(), false, "ж" },
                { testId(), false, " " },
            };
        }
    }

    [Theory]
    [MemberData(nameof(AlphaRexData))]
    public void TestAlphaRex(string testId, bool shouldBe, string input)
        => base.RegexStringTest(Ascii.AlphaRex, testId, shouldBe, input);

    public static TheoryData<string, bool, string> Base64CharRexData
    {
        get
        {
            var id = 0;
            string testId() => id++.ToString("d2");

            return new TheoryData<string, bool, string>
            {
                { testId(), false, ""   },
                { testId(), true , "A"  },
                { testId(), true , "Z"  },
                { testId(), true , "a"  },
                { testId(), true , "z"  },
                { testId(), true , "0"  },
                { testId(), true , "9"  },
                { testId(), true , "/"  },
                { testId(), true , "+"  },
                { testId(), false, "!"  },
                { testId(), false, "Ж"  },
                { testId(), false, "ж"  },
                { testId(), false, " "  },
                { testId(), false, "\r" },
                { testId(), false, "\n" },
            };
        }
    }

    [Theory]
    [MemberData(nameof(Base64CharRexData))]
    public void TestBase64CharRex(string testId, bool shouldBe, string input) 
        => base.RegexStringTest(Ascii.Base64CharRex, testId, shouldBe, input);

    public static TheoryData<string, bool, string> Base64Data
    {
        get
        {
            var i = 0;
            string testNo() => i++.ToString("d2");
            
            return new TheoryData<string, bool, string>
            {
                { testNo(), true,  "" },
                { testNo(), true,  "TWFueSBoYW5kcyBtYWtlIGxpZ2h0IHdvcmsu" },
                { testNo(), false, "!" },
                { testNo(), true,  """
                                   IA==

                                   """ },
                { testNo(), false, """
                                   I!A==

                                   """ },
                { testNo(), false, """
                                   !IA==
                 
                                   """ },
                { testNo(), false, """
                                   !IA==!
                 
                                   """ },
                { testNo(), true,  """
                                   IAmg455a
                                   12tGb7/+
                                   
                                   """ },
                { testNo(), false, """
                                   I!Amg455a
                                   12tGb7/+
                 
                                   """ },
                { testNo(), false, """
                                   !IAmg455a
                                   12tGb7/+
                 
                                   """ },
                { testNo(), false, """
                                   IAmg455a!
                                   12tGb7/+
                    
                                   """ },
                { testNo(), true,  """
                                   IAmg455a
                                   12tGb7/+
                                   IA==
                                   
                                   """ },
                { testNo(), false, """
                                   IAmg455a
                                   12tGb7/+
                                   !IA==
                          
                                   """ },
                { testNo(), false, """
                                   IAmg455a
                                   1!2tGb7/+
                                   IA==
                   
                                   """ },
                { testNo(), false, """
                                   IAmg455a
                                   12tGb7/+!
                                   IA==
                    
                                   """ },
                { testNo(), false, """
                                   IAmg455a
                                   12tGb7/+
                                   !IA==
                 
                                   """ },
                { testNo(), false, """
                                   IAmg455a
                                   12tGb7/+
                                   IA==!
                 
                                   """ },
                { testNo(), false, """
                                   IAmg455a
                                   12tGb7/+
                                   IA=!=
                 
                                   """ },
                { testNo(), false, """
                                   IAmg455a
                                   12tGb7/+
                                   I!A==
                    
                                   """ },
            };
        }
    }

    [Theory]
    [MemberData(nameof(Base64Data))]
    public void TestBase64(string testId, bool shouldBe, string input) 
        => base.RegexTest(Ascii.Base64, testId, shouldBe, input);
}