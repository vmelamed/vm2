namespace vm2.Repository.EntityFramework.CommitInterceptor;

/// <summary>
/// Provides a builder for configuration of commit interceptors by specifying a collection of commit actions to be performed.
/// </summary>
/// <remarks>
/// This type is used to construct and manage a collection of <see cref="IPolicyRule"/>-s instances or a single
/// <see cref="ICommitPolicy"/> that define the behavior to be executed during commit operations. Use this builder to add,
/// modify, or inspect commit actions as part of the  configuration process.
/// </remarks>
public record CommitInterceptorConfigurationBuilder(
    IServiceProvider ServiceProvider,
    IConfiguration Configuration)
{
    /// <summary>
    /// Gets the collection of commit rules to be performed.
    /// </summary>
    List<IPolicyRule> RulesList { get; init; } = [];

    /// <summary>
    /// Gets the collection of commit rules to be performed.
    /// </summary>
    public IEnumerable<IPolicyRule> Rules => new ReadOnlyCollection<IPolicyRule>(RulesList);

    /// <summary>
    /// Adds the specified commit rule to the configuration builder.
    /// </summary>
    /// <param name="rule">The commit rule to add.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder With(IPolicyRule rule)
    {
        RulesList.Add(rule);
        return this;
    }

    /// <summary>
    /// Adds the specified commit rule to the configuration builder.
    /// </summary>
    /// <param name="rule">The commit rule to add.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder With<TRule>(TRule rule) where TRule : IPolicyRule
    {
        RulesList.Add(rule);
        return this;
    }

    /// <summary>
    /// Adds a keyed rules of type <see cref="IPolicyRule"/> to the configuration builder.
    /// </summary>
    /// <param name="key">The key used to retrieve the <see cref="IPolicyRule"/> service.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder WithKeyed(object key)
    {
        RulesList.Add(
            ServiceProvider.GetRequiredKeyedService<IPolicyRule>(key));
        return this;
    }

    /// <summary>
    /// Adds a keyed rules of type <see cref="IPolicyRule"/> to the configuration builder.
    /// </summary>
    /// <param name="key">The key used to retrieve the <see cref="IPolicyRule"/> service.</param>
    /// <typeparam name="TRule">
    /// The specific type of <see cref="IPolicyRule"/> to retrieve. Note that the C# syntax requires you to specify the type.
    /// </typeparam>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder WithKeyed<TRule>(object key) where TRule : IPolicyRule
    {
        RulesList.Add(
            ServiceProvider.GetRequiredKeyedService<TRule>(key));
        return this;
    }

    /// <summary>
    /// Adds the specified commit rules to the configuration builder.
    /// </summary>
    /// <param name="configureRule">The commit rule configure and add.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder With(Func<IServiceProvider, IConfiguration, IPolicyRule> configureRule)
    {
        RulesList.Add(configureRule(ServiceProvider, Configuration));
        return this;
    }

    /// <summary>
    /// Adds the specified commit rules to the configuration builder.
    /// </summary>
    /// <param name="configureRule">The commit rule configure and add.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder With<TRule>(Func<IServiceProvider, IConfiguration, TRule> configureRule)
        where TRule : IPolicyRule
    {
        RulesList.Add(configureRule(ServiceProvider, Configuration));
        return this;
    }

    /// <summary>
    /// Adds the specified commit rules to the configuration builder.
    /// </summary>
    /// <param name="rules">The commit rules to add. Cannot be <see langword="null"/>.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder With(params IPolicyRule[] rules)
    {
        RulesList.AddRange(rules);
        return this;
    }

    /// <summary>
    /// Adds the specified commit rules to the configuration builder.
    /// </summary>
    /// <param name="rules">The commit rules to add. Cannot be <see langword="null"/>.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder With<TRule>(params TRule[] rules) where TRule : IPolicyRule
    {
        RulesList.AddRange(rules.Cast<IPolicyRule>());
        return this;
    }

    /// <summary>
    /// Adds a configuration-based rules to the builder and returns the updated builder instance.
    /// </summary>
    /// <remarks>
    /// The provided <paramref name="configure"/> is invoked when building the <see cref="CommitInterceptorConfiguration"/> with
    /// the current <see cref="IServiceProvider"/> and <see cref="IConfiguration"/> to create the rules, which is then added to
    /// the builder's internal rules list.
    /// </remarks>
    /// <typeparam name="TRule">The type of the rules to be added. Must implement <see cref="IPolicyRule"/>.</typeparam>
    /// <param name="configure">A function that takes an <see cref="IServiceProvider"/> and an <see cref="IConfiguration"/> and
    /// returns an instance of <typeparamref name="TRule"/>.
    /// </param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance with the specified rules added.</returns>
    public CommitInterceptorConfigurationBuilder With<TRule>(
        params Func<IServiceProvider, IConfiguration, TRule>[] configure) where TRule : IPolicyRule
        => With(
            configure
                .Select(c => c(ServiceProvider, Configuration))
                .ToArray());

    /// <summary>
    /// Adds the actions of the specified commit policy to the configuration builder.
    /// </summary>
    /// <param name="policy">
    /// The commit policy to add. The policy's actions will be included in the configuration.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder WithPolicy(ICommitPolicy policy)
    {
        RulesList.AddRange(
            policy.Actions);
        return this;
    }

    /// <summary>
    /// Adds the actions of the specified commit policy to the configuration builder.
    /// </summary>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder WithPolicy<TPolicy>() where TPolicy : ICommitPolicy
    {
        var policy = ServiceProvider.GetRequiredService<TPolicy>();
        RulesList.AddRange(
            policy.Actions);
        return this;
    }

    /// <summary>
    /// Adds the actions of the specified commit policy to the configuration builder.
    /// </summary>
    /// <param name="key">The key used to retrieve the <see cref="ICommitPolicy"/> service.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder WithKeyedPolicy(object key)
    {
        RulesList.AddRange(
            ServiceProvider.GetRequiredKeyedService<ICommitPolicy>(key).Actions);
        return this;
    }

    /// <summary>
    /// Adds the actions of the specified commit policy to the configuration builder.
    /// </summary>
    /// <param name="key">The key used to retrieve the <see cref="ICommitPolicy"/> service.</param>
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder WithKeyedPolicy<TPolicy>(object key) where TPolicy : ICommitPolicy
    {
        RulesList.AddRange(
            ServiceProvider.GetRequiredKeyedService<TPolicy>(key).Actions);
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
    /// <returns>The current <see cref="CommitInterceptorConfigurationBuilder"/> instance, allowing for method chaining.</returns>
    public CommitInterceptorConfigurationBuilder WithPolicy<TPolicy>(
        Func<IServiceProvider, IConfiguration, TPolicy> configPolicy) where TPolicy : ICommitPolicy
    {
        WithPolicy(configPolicy(ServiceProvider, Configuration));
        return this;
    }

    /// <summary>
    /// Builds and returns a new <see cref="Configuration"/> instance based on the current settings.
    /// </summary>
    /// <returns>A <see cref="Configuration"/> object initialized with the current state of the builder.</returns>
    public CommitInterceptorConfiguration Build() => new(this);
}