namespace vm2.RegexLib;

internal static class Common
{
    internal const RegexOptions Options = RegexOptions.Singleline |
                                          RegexOptions.IgnorePatternWhitespace |
                                          RegexOptions.CultureInvariant;

    internal const RegexOptions OptionsI = RegexOptions.Singleline |
                                           RegexOptions.IgnorePatternWhitespace |
                                           RegexOptions.CultureInvariant |
                                           RegexOptions.IgnoreCase;

    internal const RegexOptions MultilineOptions = RegexOptions.Multiline |
                                                   RegexOptions.IgnorePatternWhitespace |
                                                   RegexOptions.CultureInvariant;

    internal const RegexOptions MultilineOptionsI = RegexOptions.Multiline |
                                                    RegexOptions.IgnorePatternWhitespace |
                                                    RegexOptions.IgnoreCase |
                                                    RegexOptions.CultureInvariant;

}
