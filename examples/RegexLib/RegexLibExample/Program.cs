using System.CommandLine;
using System.Text.RegularExpressions;

using vm2.RegexLib;

var regexes = new Dictionary<string, Func<Regex>>
{
    { "Base64", Ascii.Base64 },
    { "RoutingNumber", Banking.AbaRoutingNumber },
    { "SwiftCode", Banking.SwiftCode },
    { "SwiftCodeI", Banking.SwiftCodeI },
    { "IbanCode", Banking.Iban },
    { "IbanCodeI", Banking.IbanI },
    { "CountryCode", Countries.CountryCode2 },
    { "CountryCodeI", Countries.CountryCode2I },
    { "CountryCode3", Countries.CountryCode3 },
    { "CountryCode3I", Countries.CountryCode3I },
    { "CurrencyCode3",  Countries.CurrencyCode },
    { "CurrencyCode3I", Countries.CurrencyCodeI },
    { "TelCountryCode", Countries.TelephoneCode },
    { "TelephoneNumber", Countries.TelephoneNumber },
    { "TelephoneNumberE164", Countries.TelephoneNumberE164 },
    { "TelephoneNumberExtra", Countries.TelephoneNumberExtra },
    { "USTelephoneNumber", Countries.US.TelephoneNumber },
    { "USTelephoneNumberStrict", Countries.US.TelephoneNumberStrict },
    { "USStateCode", Countries.US.StateCode },
    { "USStateCodeI", Countries.US.StateCodeI },
    { "USZipCode5", Countries.US.ZipCode5 },
    { "USZipCode5-4", Countries.US.ZipCode5x4 },
    { "USZipCode5?4", Countries.US.ZipCode5o4 },
    { "SSN", Countries.US.SocialSecurityNumber },
    { "ITIN", Countries.US.Itin },
    { "DateTime", DateAndTime.DateTime },
    { "DateTimeI", DateAndTime.DateTimeI },
    { "Duration", DateAndTime.DateTime },
    { "DurationI", DateAndTime.DateTimeI },
    { "LinuxPathname", LinuxPathname.Pathname },
    { "WindowsPathname", WindowsPathname.Pathname },
    { "IPv4", Net.Ipv4Address },
    { "IPv6", Net.Ipv6Address },
    { "IPv6NoZone", Net.Ipv6NzAddress },
    { "IPvFuture", Net.IpvFutureAddress },
    { "DnsName", Net.DnsName },
    { "Host", Net.Host },
    { "Port", Net.Port },
    { "Endpoint", Net.Endpoint },
    { "Octal", Numerical.OctalNumber },
    { "Hexadecimal", Numerical.HexadecimalNumber },
    { "Natural", Numerical.NaturalNumber },
    { "Decimal", Numerical.DecimalNumber },
    { "Integer", Numerical.IntegerNumber },
    { "Fractional", Numerical.FractionalNumber },
    { "Scientific", Numerical.ScientificNumber },
    { "UUID", Numerical.Uuid },
    { "SemVer", SemVer.SemanticVersion },
    { "URI", Uris.Uri },
    { "Scheme", Uris.Scheme },
    { "Host", Uris.Host },
    { "Endpoint", Uris.Endpoint },
};

var patternNames = regexes.Keys.ToArray();

var rootCommand = new RootCommand("RegexLib CLI - Match input against a chosen regex pattern from RegexLib");

var patternOption = new Option<string>(
    name: "--pattern",
    description: $"Regex pattern to use. Available: {string.Join(", ", patternNames)}")
{
    IsRequired = true
};

var inputArgument = new Argument<string>(
    name: "input",
    description: "Input string to match against the chosen regex")
{
    Arity = ArgumentArity.ExactlyOne
};

rootCommand.AddOption(patternOption);
rootCommand.AddArgument(inputArgument);

rootCommand.SetHandler((string pattern, string input) =>
{
    if (!regexes.TryGetValue(pattern, out var regex))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Pattern '{pattern}' not found. Available patterns: {string.Join(", ", patternNames)}");
        Console.ResetColor();
        return;
    }

    var match = regex().Match(input);
    if (match.Success)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✔ Match! Pattern '{pattern}' matched input.");
        Console.ResetColor();
        Console.WriteLine("Groups:");
        foreach (var groupName in regex().GetGroupNames())
        {
            Console.WriteLine($"  {groupName}: {match.Groups[groupName].Value}");
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"✘ No match. Pattern '{pattern}' did not match input.");
        Console.ResetColor();
    }
}, patternOption, inputArgument);

return await rootCommand.InvokeAsync(args);
