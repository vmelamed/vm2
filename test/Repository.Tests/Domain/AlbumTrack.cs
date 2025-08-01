namespace vm2.Repository.Domain;

/// <summary>
/// Represents a track within an album, including its position and metadata.
/// </summary>
/// <remarks>
/// This type is immutable and provides information about a track's association with an album, including its order within the
/// album and whether it was first released on that album.
/// </remarks>
/// <param name="Track"></param>
/// <param name="OrderNumber"></param>
/// <param name="FirstRelease"></param>
public readonly record struct AlbumTrack(Track Track, bool FirstRelease) : IValidatable
{
    public async ValueTask Validate(object? context = null, CancellationToken cancellationToken = default)
        => await new AlbumTrackValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
}
