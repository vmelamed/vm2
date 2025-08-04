namespace vm2.Repository.Tests.Domain;
using System;

[DebuggerDisplay("{Name} ({CountryCode})")]
public class Label : IFindable<Label>, IAuditable, IValidatable, IEquatable<Label>
{
    /// <summary>
    /// Represents the maximum allowed length for Label's name.
    /// </summary>
    /// <remarks>This constant defines the upper limit for the number of characters in a name. It can be used
    /// to validate input or enforce constraints in applications.</remarks>
    public const int MaxNameLength = 100;

    /// <summary>
    /// The collection of albums released by this label.
    /// </summary>
    HashSet<Album> _albums = [];

    /// <summary>
    /// Gets or sets the unique identifier for the recording label entities.
    /// </summary>
    public uint Id { get; private set; }

    /// <summary>
    /// Gets or sets the name of the recording label.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the country code where the recording label company is registered.
    /// </summary>
    public string CountryCode { get; set; } = null!;

    /// <summary>
    /// Gets the collection of albums released by this label.
    /// </summary>
    public ICollection<Album> Albums => _albums;

    #region IAuditable
    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; } = default;

    /// <inheritdoc />
    public string CreatedBy { get; set; } = "";

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; } = default;

    /// <inheritdoc />
    public string UpdatedBy { get; set; } = "";
    #endregion

    #region IFindable<Label>
    /// <inheritdoc />
    public static Expression<Func<Label, object?>> KeyExpression => l => new { l.Id };

    /// <inheritdoc />
    public ValueTask ValidateFindable(object? _, CancellationToken __)
    {
        new LabelFindableValidator()
                .ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }
    #endregion

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Label"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Label}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Label}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <c><![CDATA[var label = await _repository.Find(Label.ById(42), ct);]]></c>
    /// </summary>
    /// <param name="id">The unique identifier for the label.</param>
    public static IFindable ById(int Id) => new Findable(Id);

    /// <summary>
    /// Initializes a new instance of the <see cref="Label"/> class with specified creation and update metadata. For use mostly
    /// by EF.
    /// </summary>
    /// <param name="id">The unique identifier for the label.</param>
    /// <param name="name">The name of the label. Cannot be null or empty.</param>
    /// <param name="countryCode">The country code associated with the label. Must be a valid ISO 3166-1 alpha-2 code.</param>
    /// <param name="createdAt">The date and time when the label was created. Defaults to the current date and time if not specified.</param>
    /// <param name="createdBy">The identifier of the actor who created the label. Defaults to an empty string if not specified.</param>
    /// <param name="updatedAt">The date and time when the label was last updated. Defaults to the current date and time if not specified.</param>
    /// <param name="updatedBy">The identifier of the actor who last updated the label. Defaults to an empty string if not specified.</param>
    public Label(
        uint id,
        string name,
        string countryCode,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "")
    {
        Id          = id;
        Name        = name;
        CountryCode = countryCode;
        CreatedAt   = createdAt;
        CreatedBy   = createdBy;
        UpdatedAt   = updatedAt;
        UpdatedBy   = updatedBy;

        new LabelInvariantValidator()
                .ValidateAndThrow(this);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Label"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="Label"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private Label()
    {
    }

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new LabelValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
    #endregion

    public Label Releases(Album album)
    {
        if (album.Label is null)
            throw new InvalidOperationException("Album is already assigned to a different label.");

        album.Label = this;
        return this;
    }

    #region Identity rules implementation.
    #region IEquatable<Label> Members
    /// <summary>
    /// Indicates whether the current object is equal to a reference to another object of the same type.
    /// </summary>
    /// <param name="other">A reference to another object of type <see cref="Label"/> to compare with the current object.</param>
    /// <returns><list type="number">
    ///     <item><see langword="false"/> if <paramref name="other"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="other"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="other"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="other"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list></returns>
    public virtual bool Equals(Label? other)
        => other is not null
           && (ReferenceEquals(this, other)
               ||  typeof(Label) == other.GetType()
                   && Id         == other.Id);
    #endregion

    /// <summary>
    /// Determines whether this <see cref="Label"/> instance is equal to the specified <see cref="object"/> reference.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> reference to compare with this <see cref="Label"/> object.</param>
    /// <returns><list type="number">
    ///     <item><see langword="false"/> if <paramref name="obj"/> cannot be cast to <see cref="Label"/>, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="obj"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="obj"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list></returns>
    public override bool Equals(object? obj)
        => Equals(obj as Label);

    /// <summary>
    /// Serves as a hash function for the objects of <see cref="Label"/> and its derived types.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Label"/> instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(typeof(Label), Id);

    /// <summary>
    /// Compares two <see cref="Label"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are considered to be equal (<see cref="Equals(Label)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Label left, Label right)
        => left is null
                ? right is null
                : left.Equals(right);

    /// <summary>
    /// Compares two <see cref="Label"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are not considered to be equal (<see cref="Equals(Label)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Label left, Label right)
        => !(left==right);
    #endregion
}
