namespace vm2.RegexLib;

/// <summary>
/// Class Numerics. Exposes string and <see cref="Regex"/> constants and static properties that match 
/// </summary>
public static class Numerics
{
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
    public const string OctNumberRegex = $"^{OctNumberRex}$";

    static readonly Lazy<Regex> rexOctalNumber = new(() => new(OctNumberRegex, RegexOptions.Compiled|
                                                                               RegexOptions.CultureInvariant|
                                                                               RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string representing an octal number.
    /// <para>BNF: <c>octal_number = 1*[octal_digit]</c></para>
    /// </summary>
    public static Regex OctalNumber => rexOctalNumber.Value;

    /// <summary>
    /// The decimal digit characters.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    internal const string DecDigitChars = $"{OctDigitChars} 8 9";

    /// <summary>
    /// Matches a decimal digit.
    /// <para>BNF: <c>decimal_digit = octal_digit | 8 | 9</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string DecDigitRex = $"[{DecDigitChars}]";

    /// <summary>
    /// Matches sign-less, whole, decimal (a.k.a. natural) number.
    /// <para>BNF: <c>natural_number = 1*[decimal_digit]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string NaturalNumberRex = $"{DecDigitRex}+";

    /// <summary>
    /// Matches a string that represents a sign-less, whole, decimal number, a.k.a. natural number (i.e. 0, 1, ... , 37, etc.)
    /// <para>BNF: <c>natural_number = 1*[decimal_digit]</c></para>
    /// </summary>
    public const string NaturalNumberRegex = $"^{NaturalNumberRex}$";

    static readonly Lazy<Regex> rexNaturalNumber = new(() => new(NaturalNumberRegex, RegexOptions.Compiled|
                                                                                     RegexOptions.CultureInvariant|
                                                                                     RegexOptions.Singleline|
                                                                                     RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a sign-less, whole, decimal 
    /// number, a.k.a. natural number, i.e. 0, 1, ... , 37, etc.
    /// <para>BNF: <c>natural_number = 1*[decimal_digit]</c></para>
    /// </summary>
    public static Regex NaturalNumber => rexNaturalNumber.Value;

    /// <summary>
    /// The hexadecimal digit characters.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    internal const string HexDigitChars = $"{DecDigitChars} A-F a-f";

    /// <summary>
    /// Matches a hexadecimal digit.
    /// <para>BNF: <c>hexadecimal_digit = decimal_digit | A | B | C | D | E | F | a | b | c | d | e | f</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HexDigitRex = $"[{HexDigitChars}]";

    /// <summary>
    /// Matches a hexadecimal number.
    /// <para>BNF: <c>hexadecimal_number = 1*[hexadecimal_digit]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HexNumberRex = $"{HexDigitRex}+";

    /// <summary>
    /// Matches a string representing a hexadecimal number.
    /// <para>BNF: <c>hexadecimal_number = 1*[hexadecimal_digit]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HexNumberRegex = $"^{HexNumberRex}$";

    static readonly Lazy<Regex> rexHexNumber = new(() => new(HexNumberRegex, RegexOptions.Compiled|
                                                                             RegexOptions.CultureInvariant|
                                                                             RegexOptions.Singleline|
                                                                             RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a hexadecimal number.
    /// </summary>
    public static Regex HexNumber => rexHexNumber.Value;

    /// <summary>
    /// The name of a matching group representing the sign of a decimal number.
    /// </summary>
    public const string SignGr = "sign";

    /// <summary>
    /// The name of a matching group representing the natural number (after the sign).
    /// </summary>
    public const string NaturalGr = "natural";

    /// <summary>
    /// Matches a possibly signed, decimal number, a.k.a. integer number (i.e. 42, -23, etc.)
    /// <para>BNF: <c>integer_number = [+|-]natural_number</c></para>
    /// Named groups: <see cref="SignGr"/> and <see cref="NaturalGr"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string IntegerNumberRex = $"(?<{SignGr}> [+-])? (?<{NaturalGr}> {NaturalNumberRex})";

    /// <summary>
    /// Matches a string representing an integer number.
    /// <para>BNF: <c>integer_number = [+|-]natural_number</c></para>
    /// Named groups: <see cref="SignGr"/> and <see cref="NaturalGr"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string IntegerNumberRegex = $"^{IntegerNumberRex}$";

    static readonly Lazy<Regex> rexIntegerNumber = new(() => new(IntegerNumberRegex, RegexOptions.Compiled|
                                                                                     RegexOptions.CultureInvariant|
                                                                                     RegexOptions.IgnorePatternWhitespace|
                                                                                     RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string representing an integer number.
    /// <para>BNF: <c>integer_number = [+|-]natural_number</c></para>
    /// Named groups: <see cref="SignGr"/> and <see cref="NaturalGr"/>
    /// </summary>
    public static Regex IntegerNumber => rexIntegerNumber.Value;

    /// <summary>
    /// The name of a matching group representing the whole part of a fractional number.
    /// </summary>
    public const string WholeGr = "whole";

    /// <summary>
    /// The name of a matching group representing the fractional part of a fractional number.
    /// </summary>
    public const string FractionGr = "fraction";

    /// <summary>
    /// Matches a decimal fractional number with optional sign, whole, and/or fractional part, e.g. 12.34 -.45 +67. -123.456 etc. but not -123.
    /// <para>BNF: <c>fractional_point_number = [+|-]([natural_number].natural_number | natural_number.[natural_number] | natural_number.natural_number)</c></para>
    /// Named groups: <see cref="WholeGr"/> and <see cref="FractionGr"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string FractionalNumberRex = $@"(?: (?<{SignGr}>[+-])? (?<{WholeGr}>{NaturalNumberRex}*) \. (?<{FractionGr}>{NaturalNumberRex}*) (?<[+-]?\.) )";

    /// <summary>
    /// Matches a decimal fractional number with optional sign, whole, and/or fractional part, e.g. 12.34 -.45 +67. -123.456 etc.
    /// <para>BNF: <c>fractional_point_number = [+|-]([natural_number].natural_number | natural_number.[natural_number] | natural_number.natural_number)</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string FractionalNumberRegex = $"^{FractionalNumberRex}$";

    static readonly Lazy<Regex> rexFractional = new(() => new(FractionalNumberRegex, RegexOptions.Compiled|
                                                                                     RegexOptions.CultureInvariant|
                                                                                     RegexOptions.IgnorePatternWhitespace|
                                                                                     RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string representing a decimal fractional number.
    /// </summary>
    public static Regex Fractional => rexFractional.Value;

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
    /// Named groups: <see cref="MantissaGr"/> (<see cref="SignGr"/>, <see cref="WholeGr"/> and <see cref="FractionGr"/>) and <see cref="ExponentGr"/> (<see cref="SignGr"/> and <see cref="NaturalGr"/>)
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string ScientificNumberRex = $@"(?<{MantissaGr}>{FractionalNumberRex}) [eE] (?<{ExponentGr}>{IntegerNumberRex})";

    /// <summary>
    /// Matches a string that represents a number in a scientific notation with mantissa and an exponent, 
    /// i.e. 12.34e56 -.45e-67 +67.e12 etc. Note that it is not necessarily normalized.
    /// <para>BNF: <c>scientific_number = fractional_point (e | E) integer_number</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string ScientificNumberRegex = $"^{ScientificNumberRex}$";

    static readonly Lazy<Regex> scientificNumber = new(() => new(ScientificNumberRegex, RegexOptions.Compiled|
                                                                                        RegexOptions.CultureInvariant|
                                                                                        RegexOptions.IgnorePatternWhitespace|
                                                                                        RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a number in a scientific notation with 
    /// mantissa and an exponent, i.e. 12.34e56 -.45e-67 +67.e12 etc. Note that it is not necessarily normalized.
    /// <para>BNF: <c>scientific_number = fractional_point (e | E) integer_number</c></para>
    /// </summary>
    public static Regex ScientificNumber => scientificNumber.Value;
}
