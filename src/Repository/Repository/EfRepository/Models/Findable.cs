namespace vm2.Repository.EfRepository.Models;
/// <summary>
/// Represents a structure that holds a collection of identifiers, which can be used to locate or identify an entity.
/// </summary>
/// <param name="Ids">The identifier(s)</param>
/// <remarks>
/// The <see cref="Findable"/> structure is designed to encapsulate one or more identifiers, allowing for flexible entity
/// identification. It implements the <see cref="IFindable"/> interface, providing access to the identifiers through the
/// <see cref="KeyValues"/> property.<br/>
/// The structure is particularly useful in scenarios where entities need to be found by their unique <b>composite</b>
/// identifiers. To ensure that the identifiers are in the order specified in the entity mapping, add a simple function to the
/// entity class that returns an instance of this structure and require strongly typed and ordered composite index components.
/// E.g.:
/// <code><![CDATA[
/// public class MyEntity : IFindable<MyEntity>
/// {
///     ...
///     public static IFindable ByIds(int key, Guid tenant) => new Findable(key, tenant);
/// }
/// ]]></code>
/// Now finding an entity by its composite key can be done using:
/// <code><![CDATA[
///     var found = await _repository.Set<MyEntity>().Find(MyEntity.ByIds(42, tenantId), cancellationToken);
/// ]]></code>
/// </remarks>
/// Reads almost like plain English, making it easy to understand and use in code. Also, there is no way to accidentally pass
/// wrong parameters or forget to pass the required ones, as the function enforces the correct order and type of identifiers.
public record struct Findable(params object?[] Ids) : IFindable
{
    /// <inheritdoc />
    public readonly IEnumerable<object?>? KeyValues => Ids;
}
