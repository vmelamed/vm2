namespace vm2.Repository.Domain;

public class Person : IFindable<Person>, IAuditable, IValidatable
{
    public const int MaxNameLength = 100;

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public uint Id { get; private set; } = 0;

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
    public HashSet<string> Roles { get; private set; } = [];

    /// <summary>
    /// Gets or sets the collection of genres that the person is known to have worked in, e.g. jazz, classical, etc.
    /// </summary>
    public HashSet<string> Genres { get; private set; } = [];

    /// <summary>
    /// Gets or sets the collection of instrument codes that the person is known to have played, e.g. "tp" for trumpet, "g" for guitar, etc.
    /// </summary>
    public HashSet<string> InstrumentCodes { get; private set; } = [];

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

    /// <summary>
    /// Initializes a new instance of the <see cref="Person"/> class with specified details.
    /// </summary>
    /// <param name="id">The unique identifier for the person.</param>
    /// <param name="name">The name of the person. Cannot be null or empty.</param>
    /// <param name="birthYear">The birth year of the person, or <see langword="null"/> if unknown.</param>
    /// <param name="deathYear">
    /// The death year of the person, or <see langword="null"/> if the person is still alive or the year is unknown.
    /// </param>
    /// <param name="roles">
    /// A collection of roles associated with the person. If <see langword="null"/>, an empty collection is used.
    /// </param>
    /// <param name="genres">
    /// A collection of genres associated with the person. If <see langword="null"/>, an empty collection is used.
    /// </param>
    /// <param name="instrumentCodes">
    /// A collection of instruments the person plays. If <see langword="null"/>, an empty collection is used.
    /// </param>
    /// <param name="createdAt">
    /// The date and time when the person record was created. Defaults to <see cref="DateTimeOffset.MinValue"/> if not specified.
    /// </param>
    /// <param name="createdBy">The identifier of the actor who created the person record.</param>
    /// <param name="updatedAt">The date and time when the person record was last updated.</param>
    /// <param name="updatedBy">The identifier of the user who last updated the person record.</param>
    public Person(
        uint id,
        string name,
        int? birthYear,
        int? deathYear,
        IEnumerable<string>? roles,
        IEnumerable<string>? genres,
        IEnumerable<string>? instrumentCodes = null,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "")
    {
        Id              = id;
        Name            = name;
        BirthYear       = birthYear;
        DeathYear       = deathYear;
        Genres          = genres?.Select(g => g.Trim().ToLower()).ToHashSet() ?? [];
        Roles           = roles?.Select(r => r.Trim().ToLower()).ToHashSet() ?? [];
        InstrumentCodes = instrumentCodes?.Select(i => i.Trim().ToLower()).ToHashSet() ?? [];
        CreatedAt       = createdAt;
        CreatedBy       = createdBy;
        UpdatedAt       = updatedAt;
        UpdatedBy       = updatedBy;

        new PersonInvariantValidator()
                .ValidateAndThrow(this);
    }

    #region IFindable<Person>
    /// <inheritdoc />
    public static Expression<Func<Person, object?>> KeyExpression => p => new { p.Id };

    /// <inheritdoc />
    public ValueTask ValidateFindable(object? _, CancellationToken __)
    {
        new PersonFindableValidator().ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }
    #endregion

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Person"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Person}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Person}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <c><![CDATA[var person = await _repository.Find(Person.ById(42), ct);]]></c>
    /// </summary>
    /// <param name="id">The unique identifier for the person.</param>
    public static IFindable ById(int Id) => new Findable(Id);

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new PersonValidator(context as IRepository).ValidateAndThrowAsync(this, cancellationToken);
    #endregion

    /// <summary>
    /// Adds a role to the current person and returns the updated person.
    /// </summary>
    /// <param name="role">The role to add to the person. Cannot be null or empty.</param>
    /// <returns>The current person with the new role added.</returns>
    public Person AddRole(string role)
    {
        Roles.Add(role);
        return this;
    }

    /// <summary>
    /// Removes the specified role from the person's list of roles.
    /// </summary>
    /// <param name="role">The role to be removed. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the role removed.</returns>
    public Person RemoveRole(string role)
    {
        Roles.Remove(role);
        return this;
    }

    /// <summary>
    /// Adds a musical instrument to the person's collection of instrument codes.
    /// </summary>
    /// <param name="instrumentCode">The code representing the musical instrument to add. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the updated instrument collection.</returns>
    public Person AddInstrument(string instrumentCode)
    {
        InstrumentCodes.Add(instrumentCode);
        return this;
    }

    /// <summary>
    /// Removes the specified instrument code from the collection of instrument codes.
    /// </summary>
    /// <param name="instrumentCode">The code of the instrument to remove. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the specified instrument code removed.</returns>
    public Person RemoveInstrument(string instrumentCode)
    {
        InstrumentCodes.Remove(instrumentCode);
        return this;
    }

    /// <summary>
    /// Adds a specified genre to the list of genres associated with the person.
    /// </summary>
    /// <param name="genre">The genre to add. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the updated list of genres.</returns>
    public Person AddGenre(string genre)
    {
        Genres.Add(genre);
        return this;
    }

    /// <summary>
    /// Removes the specified genre from the list of genres associated with the person.
    /// </summary>
    /// <param name="genre">The genre to be removed. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the specified genre removed.</returns>
    public Person RemoveGenre(string genre)
    {
        Genres.Remove(genre);
        return this;
    }
}