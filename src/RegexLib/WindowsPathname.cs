namespace vm2.RegexLib;

/// <summary>
/// Class Files. Contains regular expressions that match strings representing Windows drives, directory names, file
/// names, and file paths.
/// </summary>
public static partial class WindowsPathname
{
    // See https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file.

    // characters that separate drive letter, path segments, and file names.
    const string pathSeparatorChars  = @"\\/";

    // a character that separates drive letter, path segments, and file names.
    const string pathSeparator       = $@"[{pathSeparatorChars}]";

    // Characters that can not be in a file/directory name
    const string notFilenameChars    = $@"{pathSeparatorChars}\x00-\x1F:*?""<>|";
    const string notFilenameLastChar = $@"{notFilenameChars}\.{Space}";
    const string anyFilename         = $@"[^{notFilenameChars}]{{0,259}} [^{notFilenameLastChar}]";

    // Special file names (devices)
    const string deviceNames    = $@"(?: CON | PRN | AUX | NUL | COM\d? | LPT\d? ) (?:\.{anyFilename})?";   // CON.txt is equivalent to CON

    /// <summary>
    /// Matches file or directory name on a disk (excludes the device names).
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string DiskFilenameRex = $"(?: {anyFilename} (?<!{deviceNames}) )"; // any filename but not the special filenames or special name with suffix

    /// <summary>
    /// Matches a string representing file or directory name.
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/> and <see cref="RegexOptions.IgnoreCase"/>.
    /// </remarks>
    public const string DiskFilenameRegex = $"^{DiskFilenameRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string representing file or directory name.
    /// </summary>
    [GeneratedRegex(DiskFilenameRegex, Common.OptionsI)]
    public static partial Regex DiskFilename();

    /// <summary>
    /// The the name of a matching group representing the drive letter in a path name.
    /// </summary>
    public const string DriveGr = "drive";

    /// <summary>
    /// The the name of a matching group representing the path path name.
    /// </summary>
    public const string PathGr = "path";

    /// <summary>
    /// The the name of a matching group representing the file name.
    /// </summary>
    public const string FileGr = "file";

    const string drive = $"(?: (?<{DriveGr}>{AlphaChar}) : )";

    const string pathSegment = $@"(?: \. | \.\. | {DiskFilenameRex} )";

    const string pathRootless = $"(?: {pathSegment} (?:{pathSeparator} {pathSegment})* )";

    /// <summary>
    /// Matches a path.
    /// <para>Named groups: <see cref="PathGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/> and <see cref="RegexOptions.IgnoreCase"/>.
    /// </remarks>
    public const string PathRex = $"""
        (?:
          (?:
            (?<{PathGr}> {pathSeparator}? {pathRootless} ) {pathSeparator}
          )
          | (?<{PathGr}> {pathSeparator}? )
        )
        """;

    const string pathFilename = $"(?<{FileGr}> {DiskFilenameRex} )";

    /// <summary>
    /// Matches a disk file pathname.
    /// <para>Named groups: <see cref="DriveGr"/>, <see cref="PathGr"/>, <see cref="FileGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PathnameRex = $@"(?: (?: {drive}? {PathRex}? {pathFilename} ) (?<!.{{261,}}) )";

    /// <summary>
    /// Matches a string that represents a disk file pathname.
    /// <para>Named groups: <see cref="DriveGr"/>, <see cref="PathGr"/>, <see cref="FileGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PathnameRegex = $"^{PathnameRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a file path.
    /// </summary>
    [GeneratedRegex(PathnameRegex, Common.OptionsI)]
    public static partial Regex Pathname();

    // TODO: \\?\... \\.\... UNC
}
