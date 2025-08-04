namespace vm2.Repository.Tests.Domain;

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
public record AlbumTrack
{
    /// <summary>
    /// Gets the track of the album.
    /// </summary>
    public Track Track { get; init; }

    /// <summary>
    /// Gets the unique identifier for the track.
    /// </summary>
    public uint TrackId { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumTrack"/> class using the specified track.
    /// </summary>
    /// <param name="track">The track associated with this album track.</param>
    public AlbumTrack(Track track)
    {
        Track   = track;
        TrackId = track?.Id ?? 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumTrack"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="AlbumTrack"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private AlbumTrack()
        : this(default(Track)!)
    {
    }

    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new AlbumTrackValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
}
