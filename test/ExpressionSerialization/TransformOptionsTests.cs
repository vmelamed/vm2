namespace ExpressionSerializationTests;

using TestUtilities;

using vm2.ExpressionSerialization.Exceptions;
using vm2.ExpressionSerialization.Xml;

using Xunit.Abstractions;

public class TransformOptionsTests
{
    public ITestOutputHelper Out { get; }

    public TransformOptionsTests(ITestOutputHelper output)
    {
        Out = output;
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
    }

    public static TheoryData<string, string, string, IdentifierTransformConvention, bool> TransformOptionsData => new() {
        { TestLine(), "",                         "",                 IdentifierTransformConvention.Camel, true  },
        { TestLine(), " ",                        "",                 IdentifierTransformConvention.Camel, true  },
        { TestLine(), " Abc",                     "",                 IdentifierTransformConvention.Camel, true  },
        { TestLine(), "1BC",                      "",                 IdentifierTransformConvention.Camel, true  },
        { TestLine(), "@__camel__Case__ ",        "",                 IdentifierTransformConvention.Camel, true  },
        { TestLine(), "a",                        "a",                IdentifierTransformConvention.Camel, false },
        { TestLine(), "A",                        "a",                IdentifierTransformConvention.Camel, false },
        { TestLine(), "ABC",                      "aBC",              IdentifierTransformConvention.Camel, false },
        { TestLine(), "CamelCase",                "camelCase",        IdentifierTransformConvention.Camel, false },
        { TestLine(), "camelCase",                "camelCase",        IdentifierTransformConvention.Camel, false },
        { TestLine(), "@enum",                    "enum",             IdentifierTransformConvention.Camel, false },
        { TestLine(), "_CamelCase",               "camelCase",        IdentifierTransformConvention.Camel, false },
        { TestLine(), "__Camel_Case",             "camelCase",        IdentifierTransformConvention.Camel, false },
        { TestLine(), "__camel_case",             "camelCase",        IdentifierTransformConvention.Camel, false },
        { TestLine(), "@__camel_case",            "camelCase",        IdentifierTransformConvention.Camel, false },
        { TestLine(), "@__camel_case__",          "camelCase",        IdentifierTransformConvention.Camel, false },
        { TestLine(), "@__8camel_case__",         "8camelCase",       IdentifierTransformConvention.Camel, false },
        { TestLine(), "@__camel_7case__",         "camel7case",       IdentifierTransformConvention.Camel, false },
        { TestLine(), "@__camel_a_case__",        "camelACase",       IdentifierTransformConvention.Camel, false },
        { TestLine(), "@__camel_with_5_cases__",  "camelWith5Cases",  IdentifierTransformConvention.Camel, false },

        { TestLine(), "",                         "",                 IdentifierTransformConvention.Pascal, true  },
        { TestLine(), " ",                        "",                 IdentifierTransformConvention.Pascal, true  },
        { TestLine(), " Abc",                     "",                 IdentifierTransformConvention.Pascal, true  },
        { TestLine(), "1BC",                      "",                 IdentifierTransformConvention.Pascal, true  },
        { TestLine(), "@__camel__Case__ ",        "",                 IdentifierTransformConvention.Pascal, true  },
        { TestLine(), "a",                        "A",                IdentifierTransformConvention.Pascal, false },
        { TestLine(), "A",                        "A",                IdentifierTransformConvention.Pascal, false },
        { TestLine(), "ABC",                      "ABC",              IdentifierTransformConvention.Pascal, false },
        { TestLine(), "CamelCase",                "CamelCase",        IdentifierTransformConvention.Pascal, false },
        { TestLine(), "camelCase",                "CamelCase",        IdentifierTransformConvention.Pascal, false },
        { TestLine(), "@enum",                    "Enum",             IdentifierTransformConvention.Pascal, false },
        { TestLine(), "_CamelCase",               "CamelCase",        IdentifierTransformConvention.Pascal, false },
        { TestLine(), "__Camel_Case",             "CamelCase",        IdentifierTransformConvention.Pascal, false },
        { TestLine(), "__camel_case",             "CamelCase",        IdentifierTransformConvention.Pascal, false },
        { TestLine(), "@__camel_case",            "CamelCase",        IdentifierTransformConvention.Pascal, false },
        { TestLine(), "@__camel_case__",          "CamelCase",        IdentifierTransformConvention.Pascal, false },
        { TestLine(), "@__8camel_case__",         "8camelCase",       IdentifierTransformConvention.Pascal, false },
        { TestLine(), "@__camel_7case__",         "Camel7case",       IdentifierTransformConvention.Pascal, false },
        { TestLine(), "@__camel_a_case__",        "CamelACase",       IdentifierTransformConvention.Pascal, false },
        { TestLine(), "@__camel_with_5_cases__",  "CamelWith5Cases",  IdentifierTransformConvention.Pascal, false },

        { TestLine(), "",                         "",                    IdentifierTransformConvention.SnakeLower, true  },
        { TestLine(), " ",                        "",                    IdentifierTransformConvention.SnakeLower, true  },
        { TestLine(), " Abc",                     "",                    IdentifierTransformConvention.SnakeLower, true  },
        { TestLine(), "1BC",                      "",                    IdentifierTransformConvention.SnakeLower, true  },
        { TestLine(), "@__camel__Case__ ",        "",                    IdentifierTransformConvention.SnakeLower, true  },
        { TestLine(), "a",                        "a",                   IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "A",                        "a",                   IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "ABC",                      "a_b_c",               IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "CamelCase",                "camel_case",          IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "camelCase",                "camel_case",          IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "@enum",                    "enum",                IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "_CamelCase",               "camel_case",          IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "__Camel_Case",             "camel_case",          IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "__camel_case",             "camel_case",          IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "@__camel_case",            "camel_case",          IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "@__camel_case__",          "camel_case",          IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "@__8camel_case__",         "8camel_case",         IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "@__camel_7case__",         "camel_7case",         IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "@__camel_a_case__",        "camel_a_case",        IdentifierTransformConvention.SnakeLower, false },
        { TestLine(), "@__camel_with_5_cases__",  "camel_with_5_cases",  IdentifierTransformConvention.SnakeLower, false },

        { TestLine(), "",                         "",                    IdentifierTransformConvention.SnakeUpper, true  },
        { TestLine(), " ",                        "",                    IdentifierTransformConvention.SnakeUpper, true  },
        { TestLine(), " Abc",                     "",                    IdentifierTransformConvention.SnakeUpper, true  },
        { TestLine(), "1BC",                      "",                    IdentifierTransformConvention.SnakeUpper, true  },
        { TestLine(), "@__camel__Case__ ",        "",                    IdentifierTransformConvention.SnakeUpper, true  },
        { TestLine(), "a",                        "A",                   IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "A",                        "A",                   IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "ABC",                      "A_B_C",               IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "CamelCase",                "CAMEL_CASE",          IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "camelCase",                "CAMEL_CASE",          IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "@enum",                    "ENUM",                IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "_CamelCase",               "CAMEL_CASE",          IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "__Camel_Case",             "CAMEL_CASE",          IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "__camel_case",             "CAMEL_CASE",          IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "@__camel_case",            "CAMEL_CASE",          IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "@__camel_case__",          "CAMEL_CASE",          IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "@__8camel_case__",         "8CAMEL_CASE",         IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "@__camel_7case__",         "CAMEL_7CASE",         IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "@__camel_a_case__",        "CAMEL_A_CASE",        IdentifierTransformConvention.SnakeUpper, false },
        { TestLine(), "@__camel_with_5_cases__",  "CAMEL_WITH_5_CASES",  IdentifierTransformConvention.SnakeUpper, false },

        { TestLine(), "",                         "",                    IdentifierTransformConvention.KebabLower, true  },
        { TestLine(), " ",                        "",                    IdentifierTransformConvention.KebabLower, true  },
        { TestLine(), " Abc",                     "",                    IdentifierTransformConvention.KebabLower, true  },
        { TestLine(), "1BC",                      "",                    IdentifierTransformConvention.KebabLower, true  },
        { TestLine(), "@__camel__Case__ ",        "",                    IdentifierTransformConvention.KebabLower, true  },
        { TestLine(), "a",                        "a",                   IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "A",                        "a",                   IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "ABC",                      "a-b-c",               IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "CamelCase",                "camel-case",          IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "camelCase",                "camel-case",          IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "@enum",                    "enum",                IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "_CamelCase",               "camel-case",          IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "__Camel_Case",             "camel-case",          IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "__camel_case",             "camel-case",          IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "@__camel_case",            "camel-case",          IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "@__camel_case__",          "camel-case",          IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "@__8camel_case__",         "8camel-case",         IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "@__camel_7case__",         "camel-7case",         IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "@__camel_a_case__",        "camel-a-case",        IdentifierTransformConvention.KebabLower, false },
        { TestLine(), "@__camel_with_5_cases__",  "camel-with-5-cases",  IdentifierTransformConvention.KebabLower, false },

        { TestLine(), "",                         "",                    IdentifierTransformConvention.KebabUpper, true  },
        { TestLine(), " ",                        "",                    IdentifierTransformConvention.KebabUpper, true  },
        { TestLine(), " Abc",                     "",                    IdentifierTransformConvention.KebabUpper, true  },
        { TestLine(), "1BC",                      "",                    IdentifierTransformConvention.KebabUpper, true  },
        { TestLine(), "@__camel__Case__ ",        "",                    IdentifierTransformConvention.KebabUpper, true  },
        { TestLine(), "a",                        "A",                   IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "A",                        "A",                   IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "ABC",                      "A-B-C",               IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "CamelCase",                "CAMEL-CASE",          IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "camelCase",                "CAMEL-CASE",          IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "@enum",                    "ENUM",                IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "_CamelCase",               "CAMEL-CASE",          IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "__Camel_Case",             "CAMEL-CASE",          IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "__camel_case",             "CAMEL-CASE",          IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "@__camel_case",            "CAMEL-CASE",          IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "@__camel_case__",          "CAMEL-CASE",          IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "@__8camel_case__",         "8CAMEL-CASE",         IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "@__camel_7case__",         "CAMEL-7CASE",         IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "@__camel_a_case__",        "CAMEL-A-CASE",        IdentifierTransformConvention.KebabUpper, false },
        { TestLine(), "@__camel_with_5_cases__",  "CAMEL-WITH-5-CASES",  IdentifierTransformConvention.KebabUpper, false },
    };

    [Theory]
    [MemberData(nameof(TransformOptionsData))]
    public void TransformOptionsTest(string _, string input, string expected, IdentifierTransformConvention convention, bool throws)
    {
        var call = () => TransformOptions.DoTransformIdentifier(input, convention);
        if (throws)
        {
            call.Should().Throw<InternalTransformErrorException>();
            return;
        }

        call().Should().Be(expected);
    }
}
