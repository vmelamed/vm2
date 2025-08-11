namespace vm2.Repository.FakeDbSet;

/// <summary>
/// Class TestDbSet encapsulates an in-memory sequence of objects to be queried synchronously or asynchronously. It
/// implements the <see cref="DbSet{TEntity}" /> from Entity Framework, so it can be used as a substitute in unit
/// tests. Also implements the <see cref="IQueryable{TEntity}" /> and the <see cref="IListSource" />.
/// </summary>
/// <typeparam name="TEntity">The type of the entities in the sequence.</typeparam>
/// <seealso cref="DbSet{TEntity}" />
/// <seealso cref="IQueryable{TEntity}" />
/// <seealso cref="IListSource" />
/// <example>
/// <![CDATA[
/// IAsyncRepository _repository = Substitute.For<IAsyncRepository>(); // substitute for EF DbContext
/// List<Entity> testEntities = new {...};
/// _repository.Set<Entity>().Returns(new FakeDbSet<Entity>(testEntities));
/// ...
/// var Sut = new MyService(_repository, ...);
///
/// // method with LINQ queries, etc. EF methods wrapped in IAsyncRepository:
/// var result = await Sut.MyMethodWithLinqQueriesAsync(...);
/// ]]>
/// </example>
public partial class FakeDbSet<TEntity> : DbSet<TEntity>,
                                                IQueryable<TEntity>,
                                                IAsyncEnumerable<TEntity>,
                                                IListSource,
                                                IInfrastructure<IServiceProvider> where TEntity : class
{
    // the in memory sequence that can be queried synchronously by using the default IQueryable provider
    readonly IList<TEntity> _source;

    // the in memory sequence that can be queried asynchronously by using the TestAsyncEnumerable IQueryable provider
    readonly AsyncEnumerable<TEntity> _asyncSource;

    // the names of the properties (columns) that comprise the primary id for implementing Find and FindAsync
    readonly List<string> _primaryKeyNames = [];

    // by default the name of primary id is "ID", "Id", "id".
    static readonly string[] _conventionPrimaryKeyNames = ["ID", "Id", "id"];

    // just a "constant" to be used below in the expression generated in Find and FindAsync
    static readonly ParameterExpression _entityParam = Expression.Parameter(typeof(TEntity), "entity");

    bool GetParameterKeysObject(Expression<Func<TEntity, object?>> keyObjExpression)
    {
        if (keyObjExpression?.Body is UnaryExpression unaryExpr
            && unaryExpr.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked
            && unaryExpr.Operand is MemberExpression memberExpr)
            _primaryKeyNames.Add(memberExpr.Member.Name);
        else
        if (keyObjExpression?.Body is NewExpression newExpr &&
            newExpr.Members is not null &&
            newExpr.Members.Count > 0)
            _primaryKeyNames.AddRange(newExpr.Members.Select(mi => mi.Name));

        return _primaryKeyNames.Count != 0;
    }

    bool GetParameterKeyNames(Expression<Func<TEntity, object>>[]? keyExpressions = default)
    {
        if (keyExpressions is null)
            return false;

        static string PropertyName(Expression body)
            => body switch {
                UnaryExpression ue => PropertyName(ue.Operand),
                MemberExpression me => me.Member.Name,
                _ => throw new ArgumentException("Expected property or field selecting expression like `T obj => obj.Property`", nameof(body))
            };

        foreach (var ke in keyExpressions)
            _primaryKeyNames.Add(PropertyName(ke.Body));

        return _primaryKeyNames.Count != 0;
    }

    /// <summary>
    /// Gets the key names from <see cref="PrimaryKeyAttribute"/>.
    /// </summary>
    /// <returns><see langword="true" /> if there are no primary keys, <see langword="false" /> otherwise.</returns>
    bool GetPrimaryKeyNames()
    {
        _primaryKeyNames.AddRange(typeof(TEntity).GetCustomAttribute<PrimaryKeyAttribute>()?.PropertyNames ?? []);

        return _primaryKeyNames.Count != 0;
    }

    /// <summary>
    /// Gets the key names from <see cref="KeyAttribute"/>.
    /// </summary>
    /// <returns>bool.</returns>
    bool GetKeyAttributeKeyNames()
    {
        _primaryKeyNames.AddRange(
            typeof(TEntity)
                .GetMembers()
                .Where(mi => mi is PropertyInfo or FieldInfo &&
                             mi.IsDefined(typeof(KeyAttribute)))
                .OrderBy(mi => mi.GetCustomAttribute<ColumnAttribute>()?.Order ?? int.MaxValue)
                .Select(mi => mi.Name)
            );

        return _primaryKeyNames.Count != 0;
    }

    /// <summary>
    /// Gets the conventional key names.
    /// </summary>
    /// <returns>bool.</returns>
    bool GetConventionsKeyNames()
    {
        _primaryKeyNames.AddRange(
            typeof(TEntity)
                .GetMembers()
                .Where(mi => mi is PropertyInfo or FieldInfo &&
                             _conventionPrimaryKeyNames.Contains(mi.Name))
                .OrderBy(mi => mi.GetCustomAttribute<ColumnAttribute>()?.Order ?? int.MaxValue)
                .Select(mi => mi.Name)
            );

        return _primaryKeyNames.Count != 0;
    }

    /// <summary>
    /// Gets the key names from the usual suspects in precedence order.
    /// </summary>
    /// <param name="primaryKeys">The primary keys expressed as an array of expressions (<c>e => new[] { e => e.Id, e => e.TenantId }</c>).</param>
    /// <returns>bool.</returns>
    bool GetKeyNames(Expression<Func<TEntity, object>>[]? primaryKeys = default)
        => GetParameterKeyNames(primaryKeys)
           || GetPrimaryKeyNames()
           || GetKeyAttributeKeyNames()
           || GetConventionsKeyNames();

    /// <summary>
    /// Gets the key names from the usual suspects in precedence order.
    /// </summary>
    /// <param name="keyObjExpression">The primary keys expressed as a new expression <c>e => { e.Id, e.TenantId }</c>.</param>
    /// <returns>bool.</returns>
    bool GetKeyNames(Expression<Func<TEntity, object?>> keyObjExpression)
        => GetParameterKeysObject(keyObjExpression)
           || GetPrimaryKeyNames()
           || GetKeyAttributeKeyNames()
           || GetConventionsKeyNames();

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeDbSet{TEntity}" /> class with an empty List{TEntity}.
    /// </summary>
    /// <param name="keyExpressions">
    /// LINQ expressions specifying the primary keys. If empty the constructor will try to infer the names via
    /// reflection, looking for properties with with names: ID, Id, id, or if <typeparamref name="TEntity"/> implements
    /// IPointReadable - the pair (id, partitionKey).
    /// </param>
    public FakeDbSet(params Expression<Func<TEntity, object>>[] keyExpressions)
    {
        GetKeyNames(keyExpressions);

        _source = [];
        _asyncSource = new AsyncEnumerable<TEntity>(_source);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeDbSet{TEntity}"/> class.
    /// </summary>
    /// <param name="source">The initial sequence of entities.</param>
    /// <param name="keyExpressions">
    /// The names of the fields comprising the primary id, as a <c>params</c> array of name selecting lambdas:
    /// <code>e => e.PrimaryKeyField0, e => e.PrimaryKeyField1, ...</code>
    /// </param>
    public FakeDbSet(
        IEnumerable<TEntity> source,
        params Expression<Func<TEntity, object>>[] keyExpressions)
    {
        GetKeyNames(keyExpressions);

        // copy and wrap the input sequence into the local sources
        _source = [.. source];
        _asyncSource = new AsyncEnumerable<TEntity>(_source);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeDbSet{TEntity}" /> class with an empty List{TEntity}.
    /// </summary>
    /// <param name="keyObjExpression">The primary keys expressed as a new expression <c>e => { e.Id, e.TenantId }</c>.</param>
    /// <seealso cref="I:IFindable{TEntity}"/>
    public FakeDbSet(Expression<Func<TEntity, object?>> keyObjExpression)
    {
        GetKeyNames(keyObjExpression);

        _source = [];
        _asyncSource = new AsyncEnumerable<TEntity>(_source);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeDbSet{TEntity}"/> class.
    /// </summary>
    /// <param name="source">The initial sequence of entities.</param>
    /// <param name="keyObjExpression">The primary keys expressed as a new expression <c>e => { e.Id, e.TenantId }</c>.</param>
    /// <seealso cref="I:IFindable{TEntity}"/>
    public FakeDbSet(
        IEnumerable<TEntity> source,
        Expression<Func<TEntity, object?>> keyObjExpression)
    {
        GetKeyNames(keyObjExpression);

        // copy and wrap the input sequence into the local sources
        _source = [.. source];
        _asyncSource = new AsyncEnumerable<TEntity>(_source);
    }

    /// <inheritdoc/>
    public override IEntityType EntityType
        => throw new NotSupportedException("TestDbSet is meant to be used as a unit test substitute.");

    /// <inheritdoc/>
    public override IAsyncEnumerable<TEntity> AsAsyncEnumerable()
        => _asyncSource;

    /// <inheritdoc/>
    public override IQueryable<TEntity> AsQueryable()
        => _asyncSource;

    /// <inheritdoc/>
    public override TEntity? Find(params object?[]? keyValues)
    {
        if (keyValues is null || keyValues.Length != _primaryKeyNames.Count)
            throw new ArgumentException($"Invalid number of values were specified for the primary keys: [{string.Join(", ", _primaryKeyNames)}]." +
                                        "Possible fix: use the `TestDbSet` constructor override that lists the primary keyExpr expressions: `TestDbSet(params Expression<Func<TEntity, object>>[]? keyExpressions)`", nameof(keyValues));

        // build the PK predicate lambda to convert Find into SingleOrDefault
        Expression keyExpr(int i)
            => Expression.MakeBinary(
                            ExpressionType.Equal,
                            Expression.MakeMemberAccess(
                                _entityParam,
                                typeof(TEntity).GetProperty(_primaryKeyNames[i]) as MemberInfo
                                    ?? typeof(TEntity).GetField(_primaryKeyNames[i]) as MemberInfo
                                        ?? throw new InvalidOperationException("The indexes must be either properties or fields.")),
                            Expression.Constant(keyValues[i]));

        var body = keyExpr(0);

        for (var i = 1; i < keyValues.Length; i++)
            body = Expression.MakeBinary(
                ExpressionType.AndAlso,
                body,
                keyExpr(i));

        var predicate = Expression.Lambda<Func<TEntity, bool>>(body, _entityParam);

        // pass the predicate to SingleOrDefault for execution
        return _source.AsQueryable().FirstOrDefault(predicate);
    }

    /// <inheritdoc/>
    public override ValueTask<TEntity?> FindAsync(params object?[]? keyValues)
        => ValueTask.FromResult(Find(keyValues));

    /// <inheritdoc/>
    public override ValueTask<TEntity?> FindAsync(
        object?[]? keyValues,
        CancellationToken cancellationToken)
        => ValueTask.FromResult(Find(keyValues));

#pragma warning disable IDE0079
#pragma warning disable EF1001
    static EntityEntry<TEntity> NewEntry(TEntity entity)
        => new(
            new InternalEntityEntry(
                    DefaultStubStateManager,
                    DefaultStubEntityType,
                    entity));
#pragma warning restore EF1001
#pragma warning restore IDE0079

    /// <inheritdoc/>
    public override EntityEntry<TEntity> Add(TEntity entity)
    {
        if (_source.IndexOf(entity) < 0)
            _source.Add(entity);
        return NewEntry(entity);
    }

    /// <inheritdoc/>
    public override ValueTask<EntityEntry<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken _ = default)
    {
        if (_source.IndexOf(entity) < 0)
            _source.Add(entity);
        return ValueTask.FromResult(NewEntry(entity));
    }

    /// <inheritdoc/>
    public override EntityEntry<TEntity> Attach(TEntity entity)
    {
        if (_source.IndexOf(entity) < 0)
            _source.Add(entity);
        return NewEntry(entity);
    }

    /// <inheritdoc/>
    public override EntityEntry<TEntity> Remove(TEntity entity)
    {
        _source.Remove(entity);
        return NewEntry(entity);
    }

    /// <inheritdoc/>
    public override EntityEntry<TEntity> Update(TEntity entity)
    {
        if (_source.IndexOf(entity) < 0)
            _source.Add(entity);
        return NewEntry(entity);
    }

    /// <inheritdoc/>
    public override void AddRange(params TEntity[] entities)
    {
        foreach (var entity in entities)
            Add(entity);
    }

    /// <inheritdoc/>
    public override void AddRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            Add(entity);
    }

    /// <inheritdoc/>
    public override Task AddRangeAsync(params TEntity[] entities)
    {
        foreach (var entity in entities)
            Add(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken _ = default)
    {
        foreach (var entity in entities)
            Add(entity);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override void AttachRange(params TEntity[] entities)
    {
        foreach (var entity in entities)
            Attach(entity);
    }

    /// <inheritdoc/>
    public override void AttachRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            Attach(entity);
    }

    /// <inheritdoc/>
    public override void RemoveRange(params TEntity[] entities)
    {
        foreach (var entity in entities)
            _source.Remove(entity);
    }

    /// <inheritdoc/>
    public override void RemoveRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            _source.Remove(entity);
    }

    /// <inheritdoc/>
    public override void UpdateRange(params TEntity[] entities)
    {
        foreach (var entity in entities)
            Update(entity);
    }

    /// <inheritdoc/>
    public override void UpdateRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            Update(entity);
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        => _source.GetEnumerator();

    /// <inheritdoc/>
    public override IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => _asyncSource.GetAsyncEnumerator(cancellationToken);

    Type IQueryable.ElementType => _asyncSource.AsQueryable().ElementType;

    /// <inheritdoc/>
    Expression IQueryable.Expression => _asyncSource.AsQueryable().Expression;

    /// <inheritdoc/>
    IQueryProvider IQueryable.Provider => ((IQueryable)_asyncSource).Provider;

    /// <inheritdoc/>
    IServiceProvider IInfrastructure<IServiceProvider>.Instance => DefaultStubServiceProvider;

    /// <inheritdoc/>
    IList IListSource.GetList() => (IList)_source;

    /// <inheritdoc/>
    bool IListSource.ContainsListCollection => true;

}
