namespace vm2.Repository.TestDomain.Validators;

static partial class Regexes
{
    public const RegexOptions Options = RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace;
    public const RegexOptions OptionsI = Options | RegexOptions.IgnoreCase;

    public const string CountryCodeRegex = @"^[A-Z]{2}$";

    [GeneratedRegex(CountryCodeRegex, Options)]
    public static partial Regex CountryCode();

    public const string InstrumentCodeRegex = @"^[a-z]{1,8}$";

    [GeneratedRegex(InstrumentCodeRegex, Options)]
    public static partial Regex InstrumentCode();
}
