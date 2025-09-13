namespace vm2.Repository.EntityFramework.CommitInterceptor;

/// <summary>
/// Provides a builder for configuring commit interceptors by specifying a collection of commit actions to be performed.
/// </summary>
/// <remarks>This type is used to construct and manage a collection of <see cref="IPolicyRule"/> instances that
/// define the behavior  to be executed during commit operations. Use this builder to add, modify, or inspect commit
/// actions as part of the  configuration process.</remarks>
public record InterceptorConfigurationBuilder(
    IServiceProvider ServiceProvider,
    IConfiguration Configuration)
{
    /// <summary>
    /// Gets the collection of commit actions to be performed.
    /// </summary>
    List<IPolicyRule> ActionsList { get; init; } = [];

    /// <summary>
    /// Gets the collection of commit actions to be performed.
    /// </summary>
    public IEnumerable<IPolicyRule> Actions => new ReadOnlyCollection<IPolicyRule>(ActionsList);

    /// <summary>
    /// Adds the specified commit action to the configuration builder.
    /// </summary>
    /// <param name="action">The commit action to add. Cannot be <see langword="null"/>.</param>
    /// <returns>The current <see cref="InterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public InterceptorConfigurationBuilder With(IPolicyRule action)
    {
        ActionsList.Add(action);
        return this;
    }

    /// <summary>
    /// Adds a configuration-based action to the builder and returns the updated builder instance.
    /// </summary>
    /// <remarks>The provided <paramref name="configAction"/> is invoked immediately with the current  <see
    /// cref="IServiceProvider"/> and <see cref="IConfiguration"/> to create the action,  which is then added to the
    /// builder's internal action list.</remarks>
    /// <typeparam name="TAction">The type of the action to be added. Must implement <see cref="IPolicyRule"/>.</typeparam>
    /// <param name="configAction">A function that takes an <see cref="IServiceProvider"/> and an <see cref="IConfiguration"/>  and returns an
    /// instance of <typeparamref name="TAction"/>.</param>
    /// <returns>The current <see cref="InterceptorConfigurationBuilder"/> instance with the specified action added.</returns>
    public InterceptorConfigurationBuilder With<TAction>(
        Func<IServiceProvider, IConfiguration, TAction> configAction) where TAction : IPolicyRule
        => With(configAction(ServiceProvider, Configuration));

    /// <summary>
    /// Adds a keyed service of type <see cref="IPolicyRule"/> to the configuration builder.
    /// </summary>
    /// <param name="key">The key used to retrieve the <see cref="IPolicyRule"/> service.</param>
    /// <returns>The current <see cref="InterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public InterceptorConfigurationBuilder WithKeyed(
        object key)
    {
        ActionsList.Add(
            ServiceProvider.GetRequiredKeyedService<IPolicyRule>(key));
        return this;
    }

    /// <summary>
    /// Adds a keyed service of type <see cref="IPolicyRule"/> to the configuration builder.
    /// </summary>
    /// <param name="key">The key used to retrieve the <see cref="IPolicyRule"/> service.</param>
    /// <returns>The current <see cref="InterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public InterceptorConfigurationBuilder WithKeyed<TAction>(
        object key) where TAction : IPolicyRule
    {
        ActionsList.Add(
            ServiceProvider.GetRequiredKeyedService<TAction>(key));
        return this;
    }

    /// <summary>
    /// Adds the actions of the specified commit policy to the configuration builder.
    /// </summary>
    /// <param name="policy">
    /// The commit policy to add. The policy's actions will be included in the configuration.</param>
    /// <returns>The current <see cref="InterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public InterceptorConfigurationBuilder WithPolicy(ICommitPolicy policy)
    {
        ActionsList.AddRange(policy.Actions);
        return this;
    }

    /// <summary>
    /// Adds a commit policy to the configuration using the specified policy factory function.
    /// </summary>
    /// <remarks>The provided <paramref name="configPolicy"/> function is invoked immediately to create the
    /// policy instance,  which is then added to the configuration. This method supports chaining multiple policies in a
    /// fluent manner.</remarks>
    /// <typeparam name="TPolicy">The type of the commit policy to add. Must implement <see cref="ICommitPolicy"/>.</typeparam>
    /// <param name="configPolicy">A function that takes an <see cref="IServiceProvider"/> and an <see cref="IConfiguration"/>  and returns an
    /// instance of <typeparamref name="TPolicy"/>.</param>
    /// <returns>The current <see cref="InterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public InterceptorConfigurationBuilder WithPolicy<TPolicy>(
        Func<IServiceProvider, IConfiguration, TPolicy> configPolicy) where TPolicy : ICommitPolicy
    {
        WithPolicy(
            configPolicy(ServiceProvider, Configuration));
        return this;
    }

    /// <summary>
    /// Builds and returns a new <see cref="Configuration"/> instance based on the current settings.
    /// </summary>
    /// <returns>A <see cref="Configuration"/> object initialized with the current state of the builder.</returns>
    internal InterceptorConfiguration Build()
        => new(this);
}