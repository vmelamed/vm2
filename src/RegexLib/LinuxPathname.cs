namespace vm2.RegexLib;

/// <summary>
/// Class Files. Contains regular expressions that match strings representing Linux devices, directory names, file
/// names, and file paths.
/// </summary>
public static partial class LinuxPathname
{
    // See https://www.cyberciti.biz/faq/linuxunix-rules-for-naming-file-and-directory-names/

    const string notFilenameChars   = $@"\x00/";

    const string filenameChar       = $"[^{notFilenameChars}]";

    const string pathSeparator      = "/" ;

    const string maxPathLength      = "4096";
    const string maxFilenameLength  = "255";

    const string checkLength        = $@"(?=^.{{1,{maxPathLength}}}$)"; // length between 1 and 4096 chars

    /// <summary>
    /// Matches file or directory name.
    /// </summary>
    public const string FilenameRex = $"{filenameChar}{{1,{maxFilenameLength}}}";

    const string pathSegment        = FilenameRex;

    const string pathRootless       = $"(?: {pathSegment} (?: {pathSeparator} {pathSegment} )* )";

    /// <summary>
    /// The the name of a matching group representing the path in a path name.
    /// </summary>
    public const string PathGr      = "path";

    /// <summary>
    /// The the name of a matching group representing the file name
    /// </summary>
    public const string FileGr      = "file";

    /// <summary>
    /// Matches a path.
    /// <para>
    /// <para>Named groups: <see cref="PathGr"/>.</para>
    /// </para>
    /// </summary>
    /// <remarks>
    /// </remarks>
    public const string PathRex     = $"(?: (?: (?<{PathGr}> {pathSeparator}? {pathRootless} ) {pathSeparator} ) | (?<{PathGr}> {pathSeparator}? ) )";

    const string pathFilenameRex    = $"(?<{FileGr}> {FilenameRex} )" ;

    #region Pathname
    /// <summary>
    /// Matches a Windows disk file pathname.
    /// <para>Named groups: <see cref="PathGr"/>, <see cref="FileGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PathnameRex = $"""
        {checkLength}
        (?: {PathRex}?
            {pathFilenameRex} )
        """;

    /// <summary>
    /// Regular expression pattern which matches a whole string against $@"(?: (?: {pathRex} {pathSeparatorRex} )? {pathFilenameRex} )".
    /// </summary>
    public const string PathnameRegex = $@"^{PathnameRex}$";

    /// <summary>
    /// Gets a Regex object which matches the entire input string against the pattern &lt;see cref="PathnameRegex" /&gt;
    /// </summary>
    [GeneratedRegex(PathnameRegex, Common.Options)]
    public static partial Regex Pathname();
    #endregion
}
