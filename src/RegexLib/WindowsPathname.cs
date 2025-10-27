namespace vm2.RegexLib;

/// <summary>
/// Class Files. Contains regular expressions that match strings representing Windows drives, directory names, file
/// names, and file paths.
/// </summary>
public static partial class WindowsPathname
{
    // See https://learn.microsoft.com/en-us/windows/win32/fileio/naming-a-file.

    /// <summary>
    /// The the name of a matching group representing the UNC server in a path name.
    /// </summary>
    public const string ServerGr        = "server";

    /// <summary>
    /// The the name of a matching group representing the UNC volume in a path name.
    /// </summary>
    public const string VolumeGr        = "volume";

    /// <summary>
    /// The the name of a matching group representing the volume ID in a DOS device path name.
    /// </summary>
    public const string VolumeIdGr      = "volumeID";

    /// <summary>
    /// The the name of a matching group representing the drive letter in a path name.
    /// </summary>
    public const string DriveGr         = "drive";

    /// <summary>
    /// The the name of a matching group representing the path path name.
    /// </summary>
    public const string PathGr          = "path";

    /// <summary>
    /// The the name of a matching group representing the full file name.
    /// </summary>
    public const string FileGr          = "file";

    /// <summary>
    /// The the name of a matching group representing the file name without the suffix.
    /// </summary>
    public const string NameGr          = "name";

    /// <summary>
    /// The the name of a matching group representing the file name's suffix.
    /// </summary>
    public const string SuffixGr        = "suffix";

    // characters that separate drive letter, path segments, and file names.
    const string pathSeparatorChars     = """/\\""";

    // a character that separates drive letter, path segments, and file names.
    const string pathSeparator          = $"""[{pathSeparatorChars}]""";

    const string maxPathLength          = "260";

    const string checkLength            = $@"(?=^.{{1,{maxPathLength}}}$)"; // length between 1 and 260 chars

    // Characters that can not be in a file/directory name
    const string notNameChars           = """\x00-\x1F"*/:<>?\\|""";
    const string notSuffixChars         = """\x00-\x1F"*./:<>?\\|""";
    const string notLastChars           = """\x00-\x1F "*./:<>?\\|""";

    const string nameChar               = $"""[^{notNameChars}]""";
    const string suffixChar             = $"""[^{notSuffixChars}]""";
    const string lastChar               = $"""[^{notLastChars}]""";

    const string nameNoSuffix           = $"""(?: {nameChar}*{lastChar} )""";
    const string suffixNc               = $"""{suffixChar}*{lastChar}""";
    const string suffix                 = $"""(?<{SuffixGr}> {suffixNc} )""";
    const string nameWithNoSuffix       = $"""(?<{NameGr}> {nameChar}*{lastChar} )""";
    const string nameWithSuffix         = $"""(?: (?<{NameGr}> {nameChar}+ )\.{suffix} )""";

    const string filename               = $"""
        (?: {nameWithSuffix}
                | {nameWithNoSuffix} )
        """;

    // Special file names (devices)
    const string repelDevice            = $"""
        (?! (?:CON|PRN|AUX|NUL|COM\d?|LPT\d?)(?:\.{suffixNc})? )
        """;

    const string diskFilename           = $"""
        (?<{FileGr}>
          {repelDevice}
          {filename} )
        """; // any filename but not the device names

    /// <summary>
    /// Matches file name on a disk (excludes the device names).
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string DiskFilenameRex = $"""{checkLength} {diskFilename}"""; // any filename but not the device names

    const string dirName                = $"""
        (?:
            {repelDevice}
            {nameNoSuffix} )
        """;

    /// <summary>
    /// A regular expression pattern for matching directory names.
    /// names.
    /// </summary>
    public const string DirNameRex      = dirName;

    const string uncPath                = $"""
        (?: {pathSeparator}{pathSeparator}
            (?<{ServerGr}> {dirName} ) {pathSeparator}
            (?<{VolumeGr}> {dirName} ) {dirName} )
        """;

    const string drive                  = $"(?:(?<{DriveGr}>{AlphaChar}):)";

    const string hexDigit              = "[0-9A-Fa-f]";
    const string uuid                   = $"{hexDigit}{{8}}-{hexDigit}{{4}}-{hexDigit}{{4}}-{hexDigit}{{4}}-{hexDigit}{{12}}";

    const string volume                 = $"(?:Volume(?<brace> {{)(?<volumeID> {uuid})(?<close-brace> }}))";

    const string dosDevicePath          = $"""
        (?: {pathSeparator}{pathSeparator}
            [\.\?] {pathSeparator}
            {drive} | {volume} )
        """;

    const string uncDevicePath          = $"""
        (?: {pathSeparator}{pathSeparator}
            [\.\?] {pathSeparator} UNC {pathSeparator}
            (?<{ServerGr}> {dirName} ) {pathSeparator}
            (?<{VolumeGr}> {dirName} ) )
        """;

    const string pathSegment            = $"""
        (?: \. | \.\. |
            {dirName} )
        """;

    const string pathRootless           = $"""
        (?: {pathSegment} (?:{pathSeparator}
            {pathSegment})* )
        """;

    /// <summary>
    /// Matches a path.
    /// <para>Named groups: <see cref="PathGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/> and <see cref="RegexOptions.IgnoreCase"/>.
    /// </remarks>
    public const string PathRex         = $"""
        (?<{PathGr}>
            (?: {pathSeparator}? {pathRootless} {pathSeparator} )
          | (?: {pathSeparator} )
          | (?: \.|\.\. )
        )
        """;

    const string pathFilename           = diskFilename;

    /// <summary>
    /// Matches a disk file pathname.
    /// <para>Named groups: <see cref="DriveGr"/>, <see cref="PathGr"/>, <see cref="FileGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string DosPathnameRex  = $"""
        {checkLength}
        {drive}?
        {PathRex}?
        {pathFilename}?
        """;

    // TODO:
    //  UNC paths:
    //      \\server\share\...
    //      \\server\C$\...
    //  DOS device paths: (Note: \\.\ or \\?\ remove all limitations on path length and allowed characters)
    //      \\.\C:\... or \\?\C:\...
    //      \\.\Volume{...}... or \\?\Volume{...}...
    //      \\.\COM1 or \\?\COM1 etc.
}
