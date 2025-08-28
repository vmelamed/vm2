namespace vm2.Repository.EntityFramework.Models;

/// <summary>
/// This record is used only as an implementation of a convention stating that all entities, that are not marked explicitly with
/// <see cref="IAggregate{TRoot}"/>, belong to an aggregate with root <see cref="Unknown"/>.
/// </summary>
public record Unknown : IAggregate<Unknown>
{
}
