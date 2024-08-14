﻿namespace vm2.RegexLib;

/// <summary>
/// Class Numerical. Exposes string and <see cref="Regex"/> constants and static properties that match 
/// </summary>
public static class Numerical
{
    #region OctalNumber
    /// <summary>
    /// The octal digit characters.
    /// </summary>
    internal const string OctDigitChars = "0-7";

    /// <summary>
    /// Matches an octal digit.
    /// <para>BNF: <c>octal_digit = 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7</c></para>
    /// </summary>
    public const string OctDigitRex = $"[{OctDigitChars}]";

    /// <summary>
    /// Matches an octal number.
    /// <para>BNF: <c>octal_number = 1*[octal_digit]</c></para>
    /// </summary>
    public const string OctNumberRex = $"{OctDigitRex}+";

    /// <summary>
    /// Matches a string representing an octal number.
    /// <para>BNF: <c>octal_number = 1*[octal_digit]</c></para>
    /// </summary>
    public const string OctalNumberRegex = $"^{OctNumberRex}$";

    static readonly Lazy<Regex> rexOctalNumber = new(() => new(OctalNumberRegex, RegexOptions.Compiled, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string representing an octal number.
    /// <para>BNF: <c>octal_number = 1*[octal_digit]</c></para>
    /// </summary>
    public static Regex OctalNumber => rexOctalNumber.Value;
    #endregion

    #region HexadecimalNumber
    /// <summary>
    /// The hexadecimal digit characters.
    /// </summary>
    internal const string HexDigitChars = $"{Ascii.DigitChars}A-Fa-f";

    /// <summary>
    /// Matches a hexadecimal digit.
    /// <para>BNF: <c>hexadecimal_digit = decimal_digit | A | B | C | D | E | F | a | b | c | d | e | f</c></para>
    /// </summary>
    public const string HexDigitRex = $"[{HexDigitChars}]";

    /// <summary>
    /// Matches a hexadecimal number.
    /// <para>BNF: <c>hexadecimal_number = 1*[hexadecimal_digit]</c></para>
    /// </summary>
    public const string HexNumberRex = $"{HexDigitRex}+";

    /// <summary>
    /// Matches a string representing a hexadecimal number.
    /// <para>BNF: <c>hexadecimal_number = 1*[hexadecimal_digit]</c></para>
    /// </summary>
    public const string HexadecimalNumberRegex = $"^{HexNumberRex}$";

    static readonly Lazy<Regex> rexHexadecimalNumber = new(() => new(HexadecimalNumberRegex, RegexOptions.Compiled, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a hexadecimal number.
    /// </summary>
    public static Regex HexadecimalNumber => rexHexadecimalNumber.Value;
    #endregion

    #region NaturalNumber
    /// <summary>
    /// Matches sign-less, whole, decimal (a.k.a. natural) number.
    /// <para>BNF: <c>natural_number = *[0] [non-zero-digit] *[decimal_digit]</c></para>
    /// </summary>
    public const string NaturalNumberRex = $"0*{Ascii.NonZeroDigitRex}{Ascii.DigitRex}*";

    /// <summary>
    /// Matches a string that represents a sign-less, whole, decimal number, a.k.a. natural number (i.e. 0, 1, ... , 37, etc.)
    /// <para>BNF: <c>natural_number = 1*[decimal_digit]</c></para>
    /// </summary>
    public const string NaturalNumberRegex = $"^{NaturalNumberRex}$";

    static readonly Lazy<Regex> rexNaturalNumber = new(() => new(NaturalNumberRegex, RegexOptions.Compiled, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a sign-less, whole, decimal 
    /// number, a.k.a. natural number, i.e. 0, 1, ... , 37, etc.
    /// <para>BNF: <c>natural_number = 1*[decimal_digit]</c></para>
    /// </summary>
    public static Regex NaturalNumber => rexNaturalNumber.Value;
    #endregion

    #region DecimalNumber
    /// <summary>
    /// Matches sign-less, whole, decimal (a.k.a. natural) number.
    /// <para>BNF: <c>natural_number = *[0] [non-zero-digit] *[decimal_digit]</c></para>
    /// </summary>
    public const string DecimalNumberRex = $"{Ascii.DigitRex}+";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a zero or natural number.
    /// </summary>
    public const string DecimalNumberRegex = $@"^{DecimalNumberRex}$";

    static readonly Lazy<Regex> regexDecimalNumber = new(() => new(DecimalNumberRegex, RegexOptions.Compiled, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// Gets a Regex object which matches a string representing a zero or natural number.
    /// </summary>
    public static Regex DecimalNumber => regexDecimalNumber.Value;
    #endregion

    #region IntegerNumber
    /// <summary>
    /// The name of a matching group representing the sign of an integer number.
    /// </summary>
    public const string IntSignGr = "isign";

    /// <summary>
    /// The name of a matching group representing the decimal number (after the sign).
    /// </summary>
    public const string IntAbsGr = "iabs";

    /// <summary>
    /// Matches a possibly signed, decimal number, a.k.a. integer number (i.e. 42, -23, etc.)
    /// <para>BNF: <c>integer_number = [+|-]natural_number</c></para>
    /// <para>Named groups: <see cref="IntSignGr"/> and <see cref="IntAbsGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string IntegerNumberRex = $"(?<{IntSignGr}> [+-])? (?<{IntAbsGr}> {Ascii.DigitRex}+)";

    /// <summary>
    /// Matches a string representing an integer number.
    /// <para>BNF: <c>integer_number = [+|-]natural_number</c></para>
    /// <para>Named groups: <see cref="IntSignGr"/> and <see cref="IntAbsGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string IntegerNumberRegex = $"^{IntegerNumberRex}$";

    static readonly Lazy<Regex> rexIntegerNumber = new(() => new(IntegerNumberRegex, RegexOptions.Compiled |
                                                                                     RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string representing an integer number.
    /// <para>BNF: <c>integer_number = [+|-]natural_number</c></para>
    /// <para>Named groups: <see cref="FSignGr"/> and <see cref="IntAbsGr"/>.</para>
    /// </summary>
    public static Regex IntegerNumber => rexIntegerNumber.Value;
    #endregion

    /// <summary>
    /// The name of a matching group representing the sign of a fractional number.
    /// </summary>
    public const string FSignGr = "fsign";

    /// <summary>
    /// The name of a matching group representing the whole part of a number.
    /// </summary>
    public const string WholeGr = "whole";

    /// <summary>
    /// The name of a matching group representing the fractional part of a fractional number.
    /// </summary>
    public const string FractionGr = "fraction";

    /// <summary>
    /// Matches a decimal fractional number with optional sign, whole, and/or fractional part, e.g. 12.34 -.45 +67. -123.456 etc. but not -123.
    /// <para>BNF: <c>fractional_point_number = [+|-]([natural_number].natural_number | natural_number.[natural_number] | natural_number.natural_number)</c></para>
    /// <para>Named groups: <see cref="WholeGr"/> and <see cref="FractionGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string FractionalNumberRex = $$"""
        (?<{{FSignGr}}>[+-]?)
        (?: (?:(?<{{WholeGr}}> {{Ascii.DigitRex}}+)      (?:\.(?<{{FractionGr}}> {{Ascii.DigitRex}}*))? )  |
        (?: (?:(?<{{WholeGr}}> {{Ascii.DigitRex}}* ) \.))? (?:(?<{{FractionGr}}> {{Ascii.DigitRex}}+))  )
        """;

    /// <summary>
    /// Matches a decimal fractional number with optional sign, whole, and/or fractional part, e.g. 12.34 -.45 +67. -123.456 etc.
    /// <para>BNF: <c>fractional_point_number = [+|-]([natural_number].natural_number | natural_number.[natural_number] | natural_number.natural_number)</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string FractionalNumberRegex = $"^{FractionalNumberRex}$";

    static readonly Lazy<Regex> rexFractional = new(() => new(FractionalNumberRegex, RegexOptions.Compiled |
                                                                                     RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string representing a decimal fractional number.
    /// </summary>
    public static Regex FractionalNumber => rexFractional.Value;

    /// <summary>
    /// The name of a matching group representing the mantissa (a.k.a. significant) of a number in a scientific notation.
    /// </summary>
    public const string MantissaGr = "mantissa";

    /// <summary>
    /// The name of a matching group representing the exponent of a number in a scientific notation.
    /// </summary>
    public const string ExponentGr = "exponent";

    /// <summary>
    /// Matches a number in a scientific notation with mantissa and an exponent, 
    /// i.e. 12.34e56 -.45e-67 +67.e12 etc. Note that it is not necessarily normalized.
    /// <para>BNF: <c>scientific_number = fractional_point (e | E) integer_number</c></para>
    /// <para>
    /// Named groups: <see cref="MantissaGr"/> (<see cref="FSignGr"/>, <see cref="WholeGr"/> and <see cref="FractionGr"/>) 
    /// and <see cref="ExponentGr"/> (<see cref="IntSignGr"/> and <see cref="IntAbsGr"/>).
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string ScientificNumberRex = $@"(?<{MantissaGr}>{FractionalNumberRex}) [eE] (?<{ExponentGr}>{IntegerNumberRex})?";

    /// <summary>
    /// Matches a string that represents a number in a scientific notation with mantissa and an exponent, 
    /// i.e. 12.34e56 -.45e-67 +67.e12 etc. Note that it is not necessarily normalized.
    /// <para>BNF: <c>scientific_number = fractional_point (e | E) integer_number</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string ScientificNumberRegex = $"^{ScientificNumberRex}$";

    static readonly Lazy<Regex> scientificNumber = new(() => new(ScientificNumberRegex, RegexOptions.Compiled |
                                                                                        RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a number in a scientific notation with 
    /// mantissa and an exponent, i.e. 12.34e56 -.45e-67 +67.e12 etc. Note that it is not necessarily normalized.
    /// <para>BNF: <c>scientific_number = fractional_point (e | E) integer_number</c></para>
    /// </summary>
    public static Regex ScientificNumber => scientificNumber.Value;

    #region Uuid
    const string uuidWithDashesRex = $$"""
         (?<oparenth> \(? )
         (?<obracket> \{? )
         {{HexDigitRex}}{8}-
         {{HexDigitRex}}{4}-
         {{HexDigitRex}}{4}-
         {{HexDigitRex}}{4}-
         {{HexDigitRex}}{12}
         (?<cbracket-obracket> (?<cbracket> \}? ) )
         (?<cparenth-oparenth> (?<cparenth> \)? ) )
         """;

    const string uuidWithNoDashesRex = $@"{HexDigitRex}{{32}}";

    /// <summary>
    /// Regular expression pattern which matches a UUID in a string.
    /// </summary>
    public const string UuidRex = $@"(?: (?:{uuidWithDashesRex}) | (?:{uuidWithNoDashesRex}) )";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a UUID.
    /// </summary>
    public const string UuidRegex = $@"^{UuidRex}$";

    static readonly Lazy<Regex> regexUuid = new(() => new(UuidRegex, RegexOptions.Compiled, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// Gets a Regex object which matches a string representing a UUID.
    /// </summary>
    public static Regex Uuid => regexUuid.Value;
    #endregion
}
