﻿namespace vm2.RegexLib;

/// <summary>
/// Class Files. Contains regular expressions that match strings representing Windows drives, directory names, file 
/// names, and file paths.
/// </summary>
public static class WindowsFileNames
{
    // See https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file.

    // characters that separate drive letter, path segments, and file names.
    const string pathSeparatorChars  = @"\\/";

    // a character that separates drive letter, path segments, and file names.
    const string pathSeparator       = $@"[{pathSeparatorChars}]";

    // Characters that can not be in a file/directory name
    const string notFilenameChars    = $@"{pathSeparatorChars}\x00-\x1F:*?""<>|";
    const string notFilenameLastChar = $@"{notFilenameChars}\.{Ascii.Space}";
    const string anyFilename         = $@"[^{notFilenameChars}]{{0,259}} [^{notFilenameLastChar}]";

    // Special file names (devices)
    const string deviceNames    = $@"(?i: CON | PRN | AUX | NUL | COM\d? | LPT\d?) (?:\.{anyFilename})?";   // CON.txt is equivalent to CON

    /// <summary>
    /// Matches file or directory name on a disk (excludes the device names).
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string DiskFilenameRex = $"(?: {anyFilename} (?<!{deviceNames}) )"; // any filename but not the special filenames or special name with suffix

    /// <summary>
    /// Matches a string representing file or directory name.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string DiskFilenameRegex = $"^{DiskFilenameRex}$";

    const RegexOptions regexOptions = RegexOptions.Compiled|
                                      RegexOptions.CultureInvariant|
                                      RegexOptions.IgnorePatternWhitespace|
                                      RegexOptions.Singleline;

    static readonly Lazy<Regex> filenameRegex = new(() => new(DiskFilenameRegex, regexOptions));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string representing file or directory name.
    /// </summary>
    public static Regex DiskFilename => filenameRegex.Value;

    /// <summary>
    /// The the name of a matching group representing the drive letter in a path name.
    /// </summary>
    public const string G_DRIVE = "drive";

    /// <summary>
    /// The the name of a matching group representing the path path name.
    /// </summary>
    public const string G_PATH = "path";

    /// <summary>
    /// The the name of a matching group representing the file name.
    /// </summary>
    public const string G_FILE = "file";

    const string driveRex = $"(?: (?<{G_DRIVE}>{Ascii.AlphaRex}) : )";

    const string pathSegmentRex = $@"(?: \. | \.\. | {DiskFilenameRex} )";

    const string pathRootless = $"(?: {pathSegmentRex} (?:{pathSeparator} {pathSegmentRex})* )";

    const string pathRex = $"(?: (?: (?<{G_PATH}> {pathSeparator}? {pathRootless} ) {pathSeparator} ) | (?<{G_PATH}> {pathSeparator}? ) )";

    const string pathFilenameRex = $"(?<{G_FILE}> {DiskFilenameRex} )";

    /// <summary>
    /// Matches a disk file pathname.
    /// Named groups: <see cref="G_DRIVE"/>, <see cref="G_PATH"/>, <see cref="G_FILE"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string PathnameRex = $@"(?: (?: {driveRex}? {pathRex}? {pathFilenameRex} ) (?<!.{{261,}}) )";

    /// <summary>
    /// Matches a string that represents a disk file pathname.
    /// Named groups: <see cref="G_DRIVE"/>, <see cref="G_PATH"/>, <see cref="G_FILE"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string PathnameRegex = $"^{PathnameRex}$";

    static readonly Lazy<Regex> pathname = new(() => new Regex(PathnameRegex, regexOptions));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a file path.
    /// </summary>
    public static Regex Pathname = pathname.Value;

    // TODO: \\?\... \\.\... UNC
}
