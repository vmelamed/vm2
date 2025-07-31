namespace vm2.Repository.Domain;

public class AlbumTrack : IValidatable
{
    /// <summary>
    /// Gets or sets the album that this track is part of.
    /// </summary>
    public Album Album { get; private set; }

    /// <summary>
    /// Gets the track managed by the system.
    /// </summary>
    public Track Track { get; private set; }

    /// <summary>
    /// Gets or sets the track order number within the album.
    /// </summary>
    public uint OrderNumber { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this track is first released on this album.
    /// </summary>
    public bool FirstRelease { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumTrack"/> class, associating an album with a track and its metadata.
    /// </summary>
    /// <param name="album">The album to which the track belongs. Cannot be <see langword="null"/>.</param>
    /// <param name="track">The track being associated with the album. Cannot be <see langword="null"/>.</param>
    /// <param name="orderNumber">The position of the track within the album. Must be greater than 0.</param>
    /// <param name="firstRelease"><see langword="true"/> if the track's first release was on this album; otherwise, <see langword="false"/>.</param>
    public AlbumTrack(
        Album album,
        Track track,
        uint orderNumber,
        bool firstRelease)
    {
        Album         = album;
        Track         = track;
        OrderNumber   = orderNumber;
        FirstRelease  = firstRelease;
    }

    public async ValueTask Validate(object? context = null, CancellationToken cancellationToken = default)
        => await new AlbumTrackValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
}
