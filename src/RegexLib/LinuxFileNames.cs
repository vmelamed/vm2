using System.IO;

namespace vm2.RegexLib;

/// <summary>
/// Class Files. Contains regular expressions that match strings representing Linux devices, directory names, file 
/// names, and file paths.
/// </summary>
public static class LinuxFileNames
{
    // See https://www.cyberciti.biz/faq/linuxunix-rules-for-naming-file-and-directory-names/

    const string notFilenameChars    = $@"\x00/";

    const string filenameChar = $"[^{notFilenameChars}]";

    const string pathSeparatorRex = "/" ;

    /// <summary>
    /// Matches file or directory name.
    /// </summary>
    public const string FilenameRex = $"{filenameChar}{{1,255}}";

    const string pathSegmentRex = FilenameRex;


    const string pathRootless = $"(?: {pathSegmentRex} (?:{pathSeparatorRex} {pathSegmentRex})* )";

    /// <summary>
    /// The the name of a matching group representing the path in a path name.
    /// </summary>
    public const string G_PATH = "path";

    /// <summary>
    /// The the name of a matching group representing the file name
    /// </summary>
    public const string G_FILE = "file";

    const string pathRex = $"(?<{G_PATH}> {pathSeparatorRex}? {pathRootless}?)";

    const string pathFilenameRex = $"(?<{G_FILE}>{FilenameRex})" ;

    /// <summary>
    /// Matches a Windows disk file pathname.
    /// Named groups: <see cref="G_PATH"/>, <see cref="G_FILE"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>
    /// </remarks>
    public const string PathnameRex = $@"(?: (?: {pathRex} {pathSeparatorRex} )? {pathFilenameRex} )";
}
