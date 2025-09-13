namespace vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

/// <summary>
/// Used to indicate aggregate root types for entities without <see cref="IAggregate{TRoot}"/>.
/// </summary>
public record struct NoRoot { }
