namespace vm2.Repository.Abstractions.Model;

/// <summary>
/// Provides a contract for objects that can be validated.
/// </summary>
public interface IValidatable
{
    /// <summary>
    /// <para>
    /// Validates that this instance conforms to the validity requirements for this type. If the instance is not valid the
    /// method should throw exception, e.g. <see cref="ArgumentException" />, <see cref="ArgumentNullException" />, or another
    /// application specific validation exception like <c>FluentValidation.ValidationException</c>).
    /// </para>
    /// </summary>
    /// <param name="context">
    /// The context in which to validate the entity in. E.g. the repository. This way the entity can use the context to implement
    /// various validation rules that may require access to the repository or other services, i.e. contexts.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>ValueTask.</returns>
    ValueTask Validate(object? context = default, CancellationToken cancellationToken = default);
}
