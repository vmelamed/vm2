# RegexLib

A library of regular expressions for .NET, providing a collection of commonly used regex patterns for various purposes, such as
validation, parsing, and matching. For many of the patterns I tried to follow aplicable RFC-s and other standards, so that the
regular expressions can be used in a wide range of applications. Most of the standards describe the syntax of their respective
formats in some form of Bacus-Naur Form (BNF), which I tried to follow when creating the regular expressions. By compositioning
the regular expressions from smaller fragments, as they are defined in the respective defining documents, I tried to make the
regular expressions easier to understand, follow, and debug. Along the way, I tried to stick with the documents' definitions of
specific terms but also adopted some naming conventions to make the identifiers more readable and easier to use.

The regular expressions that I thought would be used the most I provided compioled versions of them, leveraging the latest .NET
performance improvements and the attribute `GeneratedRegexAttribute`. Other regex fragments, for which I do not expect a lot of
demand, I exposed as `public const string`-s. If you need them compiled, you can use either the `Regex.CompileToAssembly` method
or define your own methods with `GeneratedRegexAttribute`.

Because some of the regular expressions are quite complex, I adopted a regex style that uses whitespaces, new lines, etc. Some
of the expressions should be case-insensitive and some even match multiline strings (e.g. Base64). The used regex options are
defined in the static class `Common`.

## Naming Conventions

I tried to follow consistent naming conventions for the identifiers in this library to make it easier to understand, use,
comprehend the complex regular expressions, and last but not least to debug. Here are the main conventions:

- Identifiers with suffix <b>`Chars`</b> are regular expression fragments that are meant to be used as a sequence of characters
  (__character set__ -> chars) in more complex regular expressions. Examples:

  ```csharp
  "A-Z"
  "1-9"
  ":/\?\#\[\]@" // etc.
  ```

  Note that these identifiers can be concatenated to characters or other `*Chars` fragments to form larger character sets, e.g.

  ```csharp
  public const string NzDigitChars = "1-9";
  public const string DigitChars = "0{NzDigitChars}";
  public const string AlphaNumericChars = $"{AlphaChars}{DigitChars}";
  ```

- Identifiers with suffix <b>`Char`</b> are regular expression fragments that represent a single character, e.g.

  ```csharp
  public const string NzDigitChar = $"[{NzDigitChars}]";
  public const string DigitChar = $"[{DigitChars}]";
  public const string AlphaNumericChar = $"{AlphaNumericChars}";

  public const string PctEncoded = $"%{Numerical.HexDigitChar}{Numerical.HexDigitChar}";
  public const string PctEncodedChar = $"(?:{PctEncoded})";
  ```

  These identifiers can be adorned with quantifiers and concatenated with other regular expression fragments to form complex
  regular expressions:

  ```csharp
  public const string NzDigit = $"{NzDigitChar}+";
  public const string Digit = $"{DigitChar}*";
  ```

- Identifiers with suffix <b>`Rex`</b> are regular expression fragments that represent a complete regular expression that can
  match a term in a string. For example:

  ```csharp
  public const string NaturalNumberRex = $"0*{Ascii.NzDigitChar}{Ascii.DigitChar}*";
  ```

  These identifiers can be used as standalone regular expressions or concatenated with other `*Rex` fragments to form more
  complex regular expressions. These can be great for validation, parsing, or matching parameters that are supposed to form the
  bigger concept. For example, the `NaturalNumberRex` can be used to validate a parameter that will be combined with a
  separately provided sign to form a complete integer number, e.g. `"123"` and `"-"`, or `"456"` and `"+"` or `42` and `""`.

- Identifiers with suffix <b>`Regex`</b> are complete regular expressions that matches a term against the whole string. For example:
  ```csharp
  public const string NaturalNumberRegex = $"^{NaturalNumberRex}$";
  ```
  Most of these are used in the `GeneratedRegexAttribute` to create compiled regular expressions that can be used for
  validation, etc.

  It is very often that you can have three regular expressions that are related to each other, e.g. a `*Rex` regular expression
  used inside a `*Regex` regular expression, which is then used in a `GeneratedRegexAttribute`. The `*Rex` can be used to find a
  term in a string; the `*Regex` can be used to validate that a string is a valid term; and the
  `[GeneratedRegexAttribute] Method` can be used directly in code:

   ```csharp
   public const string IntegerNumberRex = $"(?<{IntSignGr}> [+-])? (?<{IntAbsGr}> {Ascii.DigitChar}+)";

   public const string IntegerNumberRegex = $"^{IntegerNumberRex}$";

   [GeneratedRegex(IntegerNumberRegex, Common.Options)]
   public static partial Regex IntegerNumber();
   ```

- Note that most regular exprerssions and fragments in the library contain named groups, which are used to
  identify the parts of the matched string. The names of the groups are usually derived from the term they represent, e.g.
  `IntSignGr` for the sign of an integer number, `IntAbsGr` for the absolute value of an integer number, etc. The names of the
  groups have suffix `Gr`. These groups can be used to extract the parts of the matched string, e.g. the sign and the absolute
  value, or the host address, path, query and fragment of a URI, etc. E.g.
  ```csharp
  var url = "https://www.acme.com/Douglas-Adams/The-Hitchhikers-Guide-To-The-Galaxy?p1=42&p2=dontpanic#so-long-and-thanks-for-all-the-fish";
  var match = UriRegex.Match(url);
  if (match.Success)
  {
      var host = match.Groups[Uri.HostGr].Value; // "www.acme.com"
      var path = match.Groups[Uri.PathGr].Value; // "/Douglas-Adams/The-Hitchhikers-Guide-To-The-Galaxy"
      var query = match.Groups[Uri.QueryGr].Value; // "p1=42&p2=dontpanic"
      var fragment = match.Groups[Uri.FragmentGr].Value; // "so-long-and-thanks-for-all-the-fish"
  }
  ```