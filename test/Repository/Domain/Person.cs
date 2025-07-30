namespace vm2.Repository.Domain;

public class Person : IFindable<Person>, IAuditable, ISoftDeletable, IValidatable
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; } = -1;

    /// <summary>
    /// Gets or sets the names of the person.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// The birth year of the person, or <see langword="null"/> if unknown.
    /// </summary>
    public int? BirthYear { get; set; } = null;

    /// <summary>
    /// The death year of the person, or <see langword="null"/> if the person is still alive or the year is unknown.
    /// </summary>
    public int? DeathYear { get; set; } = null;

    /// <summary>
    /// Gets or sets the collection (set) of roles associated with the person, e.g. performer, conductor, etc.
    /// </summary>
    public ICollection<string> Roles { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of genres that the person is known to have worked in, e.g. jazz, classical, etc.
    /// </summary>
    public ICollection<string> Genres { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of instrument codes that the person is known to have played, e.g. "tp" for trumpet, "g" for guitar, etc.
    /// </summary>
    public ICollection<string> InstrumentCodes { get; set; } = [];

    /// <summary>
    /// This constructor is intentionally left empty to allow EF Core to create instances of this class.
    /// It is not meant to be used directly in application code.
    /// </summary>
    private Person()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Person"/> class with specified details.
    /// </summary>
    /// <param name="id">The unique identifier for the person.</param>
    /// <param name="name">The name of the person. Cannot be null or empty.</param>
    /// <param name="birthYear">The birth year of the person, or <see langword="null"/> if unknown.</param>
    /// <param name="deathYear">The death year of the person, or <see langword="null"/> if the person is still alive or the year is unknown.</param>
    /// <param name="roles">A collection of roles associated with the person. If <see langword="null"/>, an empty collection is used.</param>
    /// <param name="genres">A collection of genres associated with the person. If <see langword="null"/>, an empty collection is used.</param>
    /// <param name="instrumentCodes">A collection of instruments the person plays. If <see langword="null"/>, an empty collection is used.</param>
    /// <param name="createdAt">
    /// The date and time when the person record was created. Defaults to <see cref="DateTimeOffset.MinValue"/> if not specified.
    /// </param>
    /// <param name="createdBy">The identifier of the actor who created the person record.</param>
    /// <param name="updatedAt">The date and time when the person record was last updated.</param>
    /// <param name="updatedBy">The identifier of the user who last updated the person record.</param>
    /// <param name="deletedAt">
    /// The date and time when the person record was deleted, or <see langword="null"/> if the record is not deleted.
    /// </param>
    /// <param name="deletedBy">
    /// The identifier of the user who deleted the person record, or empty string if the record is not deleted.
    /// </param>
    public Person(
        int id,
        string name,
        int? birthYear,
        int? deathYear,
        ICollection<string>? roles,
        ICollection<string>? genres,
        ICollection<string>? instrumentCodes = null,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "",
        DateTimeOffset? deletedAt = null,
        string deletedBy = "")
    {
        Id              = id;
        Name            = name;
        BirthYear       = birthYear;
        DeathYear       = deathYear;
        Genres          = genres ?? [];
        Roles           = roles ?? [];
        InstrumentCodes = instrumentCodes ?? [];
        CreatedAt       = createdAt;
        CreatedBy       = createdBy;
        UpdatedAt       = updatedAt;
        UpdatedBy       = updatedBy;
        DeletedAt       = deletedAt;
        DeletedBy       = deletedBy;
    }

    #region IFindable<Person>
    /// <inheritdoc />
    public static Expression<Func<Person, object?>> KeyExpression => p => new { p.Id };
    #endregion

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Person"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Person}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Person}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <code><![CDATA[var person = await _repository.Find(Person.ById(42), ct);]]></code>
    /// </summary>
    /// <param name="id">The unique identifier for the person.</param>
    public static IFindable ById(int Id) => new Findable(Id);

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

    #region ISoftDeletable
    /// <inheritdoc />
    public DateTimeOffset? DeletedAt { get; set; } = default;

    /// <inheritdoc />
    public string DeletedBy { get; set; } = "";
    #endregion

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new PersonValidator(context as IRepository).ValidateAndThrowAsync(this, cancellationToken);
    #endregion

    public void AddRole(Role role) => Roles.Add(role.Name);

    public void AddRole(string role) => Roles.Add(role.ToLower().Trim());

    public void AddInstrumentCode(Instrument instrument) => InstrumentCodes.Add(instrument.Code);

    public void AddInstrumentCode(string instrumentCode) => InstrumentCodes.Add(instrumentCode.ToLower().Trim());

    public void AddGenre(Genre genre) => Genres.Add(genre.Name);

    public void AddGenre(string genre) => Genres.Add(genre.ToLower().Trim());
}