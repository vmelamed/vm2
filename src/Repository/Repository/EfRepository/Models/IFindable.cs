namespace vm2.Repository.EfRepository.Models;

/// <summary>
/// Domain objects that implement this interface can be found in the repository by their primary key or composite primary keys.
/// </summary>
public interface IFindable
{
    /// <summary>
    /// Returns an ordered set of physical store key values that can be used to easily find the entity in the change
    /// tracker or the physical store of the instance. Note that usually these represent a database specific, physical
    /// identity of the entity, e.g. "primary key", "primary, composite key", "partition key and id", etc. It is
    /// possible that some of these keys are also the "business keys" of the entities which may not be recommended for
    /// relational DB-s.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// class MyEntity : IFindable
    /// {
    ///     public long Id { get; set; }
    ///     public int SubId { get; set }
    ///     ...
    ///     IEnumerable<object?>? KeyValues { get; } => new[] { Id, SubId }
    ///
    ///     public static IFindable ByIds(long key, int subId) => new Findable(key, tenant);
    /// }
    /// ...
    /// MyEntity found = await _repository.Find(new MyEntity { Id = 42, SubId = 23 });
    /// // or alternatively:
    /// MyEntity found = await _repository.Find<MyEntity>(MyEntity.ByIds(42L, 23))
    /// ]]>
    /// </example>
    IEnumerable<object?>? KeyValues { get; }

    /// <summary>
    /// Validates the keys (the property values) of the current instance that are returned by <see cref="KeyValues"/>. The
    /// method can be used to validate the keys before they are used to find the entity in the repository. If the underlying DB
    /// provider already implements such a constraint, this method should be empty.<para/>
    /// </summary>
    /// <param name="context">The context to validate in.</param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <exception cref="TEntity:FluentValidation.ValidationException"/>
    /// <remarks>
    /// Note that the method is asynchronous.
    /// </remarks>
    ValueTask ValidateFindable(object? context = default, CancellationToken cancellationToken = default);
}
