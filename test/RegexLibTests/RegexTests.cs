﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace vm2.RegexLibTests;

public abstract class RegexTests
{
    public ITestOutputHelper Out { get; }

    public RegexTests(ITestOutputHelper output) => Out = output;

    protected static string TestLine([CallerLineNumber] int line = 0) => $"{line:d3}";

    /// <summary>
    /// Data driven (theory) test for the regular expression <see cref="Regex" />
    /// </summary>
    /// <param name="regex">The regex object.</param>
    /// <param name="testId">The test identifier, e.g. "03".</param>
    /// <param name="shouldMatch">The should match.</param>
    /// <param name="input">The input.</param>
    protected virtual MatchCollection RegexTest(
        Regex regex,
        string testId,
        bool shouldMatch,
        string input)
    {
        var matches = regex.Matches(input);
        var isMatch = matches.Count > 0;

        matches.Should().NotBeNull();

        Out.WriteLine($"""
                       Test ID:  {testId}
                         Regex:    →{regex}←
                         Input:    →{input}←
                         Is match: {isMatch}
                         Matches:  {matches.Count}:
                       """);

        isMatch.Should().Be(shouldMatch);
        if (shouldMatch)
            matches.Should().HaveCountGreaterThanOrEqualTo(1);

        foreach (var m in matches)
        {
            m.Should().BeOfType<Match>();
            Out.WriteLine($"→{((Match)m).Value}←");
        }

        return matches;
    }

    /// <summary>
    /// Data driven (theory) test for the regular expression string <see cref="RegexString" />
    /// </summary>
    /// <param name="regex">The regex string.</param>
    /// <param name="testId">The test identifier, e.g. "03".</param>
    /// <param name="shouldMatch">The should match.</param>
    /// <param name="input">The input.</param>
    protected virtual void RegexStringTest(
        string regex,
        string testId,
        bool shouldMatch,
        string input,
        RegexOptions ro = RegexOptions.CultureInvariant |
                          RegexOptions.IgnorePatternWhitespace) => RegexTest(
                                                                        new Regex(regex, ro),
                                                                        testId,
                                                                        shouldMatch,
                                                                        input);
}