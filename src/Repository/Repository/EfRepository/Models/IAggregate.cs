namespace vm2.Repository.EfRepository.Models;

/// <summary>
/// In the context of Domain-Driven Design (DDD) this marker interface indicates that a class participates in the aggregate with
/// a root <typeparamref name="TRoot"/>. All classes that are part of the aggregate should implement this interface to define
/// the relationship with the aggregate root and maintain the integrity and consistency of the aggregate as a whole.
/// </summary>
/// <typeparam name="TRoot">
/// The type of the aggregate root.
/// </typeparam>
public interface IAggregate<TRoot> where TRoot : class, IAggregate<TRoot>
{
}
