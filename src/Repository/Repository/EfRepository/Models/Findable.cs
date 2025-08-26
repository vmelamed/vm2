namespace vm2.Repository.EfRepository.Models;

/// <summary>
/// Represents a structure that holds a collection of identifiers, which can be used to locate or identify an entity.
/// </summary>
/// <param name="KeyValues">
/// A sequence of one or more identifying key values comprising the composite key of an object in a DB.
/// </param>
/// <remarks>
/// The <see cref="Findable"/> structure is designed to encapsulate a sequence of one or more identifiers, allowing for flexible<br/>
/// entity identification in a DB. It implements the <see cref="IFindable"/> interface, providing access to the identifiers through<br/>
/// the <see cref="KeyValues"/> property. The structure is particularly useful in scenarios where entities need to be found by<br/>
/// their unique <b>composite</b> identifiers. To ensure that the identifiers are in the order specified in the entity mapping, add<br/>
/// a simple static method to the entity class that returns an instance of this structure and require strongly typed and<br/>
/// ordered composite index components. E.g.:
/// <code><![CDATA[
/// public class MyEntity : IFindable<MyEntity>
/// {
///     ...
///     public static IFindable ByIds(int key, Guid tenant) => new Findable(key, tenant);
/// }
/// ]]></code>
/// Now finding an entity by its composite key can be done using:
/// <code><![CDATA[
/// var found = await _repository.Set<MyEntity>().FindAsync(MyEntity.ByIds(42, tenantId), ct);
/// ]]></code>
/// Reads almost like plain English, making it easy to understand and use in code, doesn't it? But more importantly, <br/>
/// it is much harder to accidentally pass wrong parameters or forget to pass required one(s), as the method enforces <br/>
/// the correct order and type of values.
/// </remarks>
public record struct Findable(IEnumerable<object?>? KeyValues) : IFindable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Findable"/> structure with the specified identifiers.
    /// </summary>
    /// <param name="ids"></param>
    /// <exception cref="ArgumentException"></exception>
    public Findable(params object?[] ids)
        : this(ids.AsEnumerable())
    {
        if (ids is null || ids.Length == 0)
            throw new ArgumentException("At least one identifier value must be provided.", nameof(ids));
    }

    /// <inheritdoc />
    public readonly ValueTask ValidateFindableAsync(
        object? context = null,
        CancellationToken ct = default)
        => ValueTask.CompletedTask;
}
