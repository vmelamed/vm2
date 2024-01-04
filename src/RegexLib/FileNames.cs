using static System.Net.WebRequestMethods;

namespace vm2.RegexLib;

/// <summary>
/// Class Files. Contains regular expressions that match strings representing file names, directory names and file paths.
/// </summary>
public static class FileNames
{
    #region Windows file system
    // See https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file.

    // characters that separate drive letter, path segments, and file names.
    const string winPathSeparatorChars  = @"\\/";

    // a character that separates drive letter, path segments, and file names.
    const string winPathSeparatorRex    = $@"[{winPathSeparatorChars}]";

    // Characters that can not be in a file/directory name
    const string notWinFilenameChars    = $@"{winPathSeparatorChars}\x00-\x1F:*?""<>|";
    const string notWinFilenameLastChar = $@"{notWinFilenameChars}\.{Ascii.Space}";
    const string anyWinFilename         = $@"[^{notWinFilenameChars}]{{0,259}} [^{notWinFilenameLastChar}]";

    // Special Windows file names (devices)1
    const string winDeviceNames    = $@"(?i: CON | PRN | AUX | NUL | COM\d? | LPT\d?) (?:\.{anyWinFilename})?";   // CON.txt is equivalent to CON

    /// <summary>
    /// Matches Windows file or directory name on a disk (excludes the device names).
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string WindowsDiskFilenameRex = $"(?: {anyWinFilename} (?<!{winDeviceNames}) )"; // any win filename but not the special filenames

    /// <summary>
    /// Matches a string representing Windows file or directory name.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string WindowsDiskFilenameRegex = $"^{WindowsDiskFilenameRex}$";

    static readonly Lazy<Regex> windowsFilenameRegex = new(() => new(WindowsDiskFilenameRegex, RegexOptions.Compiled|
                                                                                               RegexOptions.CultureInvariant|
                                                                                               RegexOptions.IgnorePatternWhitespace|
                                                                                               RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string representing Windows file or directory name.
    /// </summary>
    public static Regex WindowsDiskFilename => windowsFilenameRegex.Value;

    const string winPathSegmentRex = $@"(?: \. | \.\. | {WindowsDiskFilenameRex} )" ;

    /// <summary>
    /// The the name of a matching group representing the drive letter in a Windows path name.
    /// </summary>
    public const string G_DRIVE = "drive";

    const string winDriveRex = $"(?: (?<{G_DRIVE}>[A-Za-z]) : )";

    const string winPathRootless = $"(?: {winPathSegmentRex} (?:{winPathSeparatorRex} {winPathSegmentRex})* )";

    /// <summary>
    /// The the name of a matching group representing the path in a Windows or Linux path name.
    /// </summary>
    public const string G_PATH = "path";

    const string winPathRex = $"(?<{G_PATH}> {winPathSeparatorRex}? {winPathRootless}?)";

    /// <summary>
    /// The the name of a matching group representing the file name
    /// </summary>
    public const string G_FILE = "filename";

    const string winPathFilenameRex = $"(?<{G_FILE}>{WindowsDiskFilenameRex})" ;

    /// <summary>
    /// Matches a Windows disk file pathname.
    /// Named groups: <see cref="G_DRIVE"/>, <see cref="G_PATH"/>, <see cref="G_FILE"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string WindowsPathnameRex = $@"(?: {winDriveRex}? (?: {winPathRex} {winPathSeparatorRex} )? {winPathFilenameRex}(?<!.{{261,}}))";

    /// <summary>
    /// Matches a string that represents a windows disk file pathname.
    /// Named groups: <see cref="G_DRIVE"/>, <see cref="G_PATH"/>, <see cref="G_FILE"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string WindowsPathnameRegex = $"^{WindowsPathnameRex}$";

    static readonly Lazy<Regex> windowsPathname = new(() => new Regex(WindowsPathnameRegex, RegexOptions.Compiled|
                                                                                            RegexOptions.CultureInvariant|
                                                                                            RegexOptions.IgnorePatternWhitespace|
                                                                                            RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a windows file path.
    /// </summary>
    public static Regex WindowsPathname = windowsPathname.Value;

    // TODO: \\?\... \\.\... UNC
    #endregion

    #region Linux file system
    // See https://www.cyberciti.biz/faq/linuxunix-rules-for-naming-file-and-directory-names/

    const string notLinuxFilenameChars    = $@"\x00/";

    const string linuxFilenameChar = $"[^{notLinuxFilenameChars}]";

    const string linuxPathSeparatorRex = "/" ;

    /// <summary>
    /// Matches Linux file or directory name.
    /// </summary>
    public const string LinuxFilenameRex = $"{linuxFilenameChar}{{1,255}}";

    const string linuxPathSegmentRex = $@"(?: \. | \.\. | {LinuxFilenameRex} )" ;


    const string linuxPathRootless = $"(?: {linuxPathSegmentRex} (?:{linuxPathSeparatorRex} {linuxPathSegmentRex})* )";

    const string linuxPathRex = $"(?<{G_PATH}> {linuxPathSeparatorRex}? {linuxPathRootless}?)";

    const string linuxPathFilenameRex = $"(?<{G_FILE}>{LinuxFilenameRex})" ;

    /// <summary>
    /// Matches a Windows disk file pathname.
    /// Named groups: <see cref="G_DRIVE"/>, <see cref="G_PATH"/>, <see cref="G_FILE"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string LinuxPathnameRex = $@"(?: (?: {linuxPathRex} {linuxPathSeparatorRex} )? {linuxPathFilenameRex} )";
    #endregion
}
