namespace vm2.RegexLib;

/// <summary>
/// Class Files. Contains regular expressions that match strings representing Windows drives, directory names, file
/// names, and file paths.
/// </summary>
public static partial class WindowsPathname
{
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

    /// <summary>
    /// A regular expression that matches strings representing valid directory names.
    /// </summary>
    public const string DirNameRegex = $"^{DirNameRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches strings representing valid directory names (not path!).
    /// </summary>
    [GeneratedRegex(DirNameRegex, Common.OptionsI)]
    public static partial Regex DirName();

    /// <summary>
    /// Matches a string that represents a disk file pathname.
    /// <para>Named groups: <see cref="DriveGr"/>, <see cref="PathGr"/>, <see cref="FileGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string DosPathnameRegex = $"^{DosPathnameRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a file path.
    /// </summary>
    [GeneratedRegex(DosPathnameRegex, Common.OptionsI)]
    public static partial Regex Pathname();
}
