namespace vm2.Repository.TestDomain;

/// <summary>
/// Represents a track within an album.
/// </summary>
/// <remarks>
/// This type is immutable and provides information about a track's association with an album, including its order within the<br/>
/// album and whether it was first released on that album.<br/>
/// TODO: replace with struct when available in, <see href="https://github.com/dotnet/efcore/issues/31237">EF 10(+?)</see>.
/// </remarks>
/// <param name="Track"></param>
/// <param name="OrderNumber"></param>
/// <param name="FirstRelease"></param>
public record AlbumTrack(Track Track) : IValidatable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumTrack"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="AlbumTrack"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    public AlbumTrack()
        : this((Track)null!)
    {
    }

    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new AlbumTrackValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken).ConfigureAwait(false);
}
