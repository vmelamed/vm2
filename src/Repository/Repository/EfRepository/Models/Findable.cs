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
/// the <see cref="IFindable.KeyValues"/> property.<br/>
/// The structure can be particularly useful in scenarios where entities need to be found by their unique composite<br/>
/// identifiers.<br/>
/// <b>Hint:</b> to ensure that the identifiers are in the order specified in the mapping of the entity, add to the entity class<br/>
/// a simple static factory method for <see cref="Findable"/> that requires strongly typed and ordered parameters - the components<br/>
/// of the composite index. E.g.:
/// <code><![CDATA[
/// public class MyEntity : IFindable<MyEntity>
/// {
///     ...
///     // static factory method to create an instance of Findable with the keys in of the proper type and in the right order:
///     public static IFindable ByIds(int key, Guid tenant) => new Findable(key, tenant);
/// }
/// ]]></code>
/// Now finding an entity by its composite key can be done using:
/// <code><![CDATA[
/// var found = await _repository.Set<MyEntity>().FindAsync(MyEntity.ByIds(42, tenantId), ct);
/// ]]></code>
/// Reads almost like plain English, makes it easy to understand and use in code, and it is much harder to accidentally pass <br/>
/// wrong parameters or forget to pass required one(s), as the method enforces the correct order and type of values.<br/>
/// </remarks>
public record struct Findable(IEnumerable<object?>? KeyValues) : IFindable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Findable"/> structure with the specified identifiers.
    /// </summary>
    /// <param name="ids"></param>
    /// <exception cref="ArgumentException"></exception>
    public Findable(params object?[] ids) : this(ids.AsEnumerable())
        => ArgumentNullException.ThrowIfNullOrEmpty(nameof(ids));

    /// <inheritdoc />
    public readonly ValueTask ValidateFindableAsync(object? context = null, CancellationToken ct = default)
        => ValueTask.CompletedTask;
}
