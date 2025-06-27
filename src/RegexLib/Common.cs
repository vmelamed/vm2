namespace vm2.RegexLib;

internal static class Common
{
    internal const RegexOptions Options = RegexOptions.Singleline |
                                          RegexOptions.IgnorePatternWhitespace;

    internal const RegexOptions OptionsI = RegexOptions.Singleline |
                                           RegexOptions.IgnorePatternWhitespace |
                                           RegexOptions.IgnoreCase;

    internal const RegexOptions MultilineOptions = RegexOptions.Multiline |
                                                   RegexOptions.IgnorePatternWhitespace;

    internal const RegexOptions MultilineOptionsI = RegexOptions.Multiline |
                                                    RegexOptions.IgnorePatternWhitespace |
                                                    RegexOptions.IgnoreCase;

}
