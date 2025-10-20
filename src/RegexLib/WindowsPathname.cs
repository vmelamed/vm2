namespace vm2.RegexLib;

/// <summary>
/// Class Files. Contains regular expressions that match strings representing Windows drives, directory names, file
/// names, and file paths.
/// </summary>
public static partial class WindowsPathname
{
    // See https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file.

    /// <summary>
    /// The the name of a matching group representing the drive letter in a path name.
    /// </summary>
    public const string DriveGr = "drive";

    /// <summary>
    /// The the name of a matching group representing the path path name.
    /// </summary>
    public const string PathGr = "path";

    /// <summary>
    /// The the name of a matching group representing the full file name.
    /// </summary>
    public const string FileGr = "file";

    /// <summary>
    /// The the name of a matching group representing the file name without the suffix.
    /// </summary>
    public const string NameGr = "name";

    /// <summary>
    /// The the name of a matching group representing the file name's suffix.
    /// </summary>
    public const string SuffixGr = "suffix";

    // characters that separate drive letter, path segments, and file names.
    const string pathSeparatorChars  = """/\\""";

    // a character that separates drive letter, path segments, and file names.
    const string pathSeparator       = $"""[{pathSeparatorChars}]""";

    // Characters that can not be in a file/directory name
    const string notNameChars       = """\x00-\x1F"*/:<>?\\|""";
    const string notSuffixChars     = """\x00-\x1F"*./:<>?\\|""";
    const string notLastChars       = """\x00-\x1F "*./:<>?\\|""";

    const string nameChar           = $"""[^{notNameChars}]""";
    const string suffixChar         = $"""[^{notSuffixChars}]""";
    const string lastChar           = $"""[^{notLastChars}]""";

    const string nameNoSuffix       = $"""(?: {nameChar}*{lastChar} )""";
    const string suffix             = $"""(?<{SuffixGr}> {suffixChar}*{lastChar} )""";
    const string suffixNc           = $"""{suffixChar}*{lastChar}""";
    const string nameWithSuffix     = $"""(?: (?<{NameGr}> {nameChar}+ )\.{suffix} )""";

    const string filename           = $"""(?: {nameWithSuffix} | (?<{NameGr}> {nameNoSuffix} ) )""";

    // Special file names (devices)
    const string noDeviceNames      = $"""(?! (?:CON|PRN|AUX|NUL|COM\d?|LPT\d?)(?:\.{suffixNc})? )""";

    const string diskFilename       = $"""(?<{FileGr}> {noDeviceNames} {filename} )"""; // any filename but not the device names

    const string checkLength   = @"(?=^.{1,260}$)"; // length between 1 and 260 chars

    /// <summary>
    /// Matches file name on a disk (excludes the device names).
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string DiskFilenameRex = $"""{checkLength} {diskFilename}"""; // any filename but not the device names

    /// <summary>
    /// Matches a string representing file name.
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/> and <see cref="RegexOptions.IgnoreCase"/>.
    /// </remarks>
    public const string DiskFilenameRegex = $"""^{DiskFilenameRex}$""";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string representing file name (without path!).
    /// </summary>
    [GeneratedRegex(DiskFilenameRegex, Common.OptionsI)]
    public static partial Regex DiskFilename();

    const string dirName = $"""(?: {noDeviceNames} {nameNoSuffix} )""";

    /// <summary>
    /// A regular expression pattern for matching directory names.
    /// names.
    /// </summary>
    public const string DirNameRex = dirName;

    /// <summary>
    /// A regular expression that matches strings representing valid directory names.
    /// </summary>
    public const string DirNameRegex = $"^{DirNameRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches strings representing valid directory names (not path!).
    /// </summary>
    [GeneratedRegex(DirNameRegex, Common.OptionsI)]
    public static partial Regex DirName();

    const string drive = $"(?:(?<{DriveGr}>{AlphaChar}):)";

    const string pathSegment = $@"(?: \.|\.\.|{dirName} )";

    const string pathRootless = $"(?: {pathSegment} (?:{pathSeparator} {pathSegment})* )";

    /// <summary>
    /// Matches a path.
    /// <para>Named groups: <see cref="PathGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/> and <see cref="RegexOptions.IgnoreCase"/>.
    /// </remarks>
    public const string PathRex = $"""
        (?<{PathGr}>
            (?: {pathSeparator}? {pathRootless} {pathSeparator} )
          | (?: {pathSeparator} )
          | (?: \.|\.\. )
        )
        """;

    const string pathFilename = diskFilename;

    /// <summary>
    /// Matches a disk file pathname.
    /// <para>Named groups: <see cref="DriveGr"/>, <see cref="PathGr"/>, <see cref="FileGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PathnameRex = $"""
                                       {checkLength}
                                       {drive}?
                                       {PathRex}?
                                       {pathFilename}?
                                       """;

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
