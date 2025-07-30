namespace vm2.Repository.Abstractions.Model;

/// <summary>
/// Extends <see cref="IFindable"/> with a new <c>static abstract</c> property <see cref="KeyExpression"/> - a lambda expression
/// that extracts the primary key(s) from the entity <typeparamref name="TEntity"/>.<br/>
/// Leverages default interface implementation to provide implementation of the <see cref="IFindable.KeyValues"/> property that
/// is used by the repository's method <br/><see cref="IRepository.Find{T}(IEnumerable{object}, CancellationToken)"/> to find
/// the entity by its primary key(s) in the change tracker or in the physical store.
/// </summary>
/// <example><![CDATA[
/// class MyEntity : IFindable<MyEntity>
/// {
///     public long Id { get; set; }
///     public int Index { get; set }
///
///     // the one and only definition of the key:
///     public static readonly Expression<Func<MyEntity, object?>> KeyExpression = e => new { e.Id, e.Index };
/// }
///
/// ...
/// // In the Entity Framework Core mapping configuration of the entity, you can reuse the KeyExpression to configure the primary key:
/// class MyEntityConfiguration : IEntityTypeConfiguration<MyEntity>
/// {
///     ...
///     // reuse the KeyExpression to configure the primary key of the entity
///     builder.HasKey(MyEntity.KeyExpression);
///     ...
/// }
///
/// ...
/// IRepository _repository;
///
/// var entity = new MyEntity { Id = 42L, Index = 7 };  // initialize *only* the primary key properties
/// var findableEntity = (IFindable)entity;             // explicitly cast to IFindable to use KeyValues in the Find method
/// ...
/// MyEntity found = await _repository.Find<MyEntity>(findableEntity.KeyValues, ct);
/// ]]></example>
public interface IFindable<TEntity> : IFindable where TEntity : class, IFindable<TEntity>
{
    /// <summary>
    /// A lambda expression that gets the primary key(s) of the entity <typeparamref name="TEntity"/> in the underlying data
    /// store, e.g. `<c>e =&gt; e.Id</c>`.<br/>
    /// If the entity has a composite primary key, the lambda should return anonymous object with properties the components -<br/>
    /// of the composite key in the proper order: `<c>e =&gt; new { e.Id, e.Index }</c>`.
    /// </summary>
    /// <remarks>
    /// <b>Hint</b>: Reuse this property when configuring the mapping of the entity to a DB table to specify the primary key's
    /// properties.
    /// </remarks>
    static abstract Expression<Func<TEntity, object?>> KeyExpression { get; }

    /// <summary>
    /// Caches the compiled lambda-function <see cref="KeyExpression"/> that extracts the keys from an entity.
    /// </summary>
    private static Func<TEntity, IEnumerable<object?>>? _getKeys;

    /// <summary>
    /// Overrides <see cref="IFindable.KeyValues"/> by extracting the keys with the help of the <see cref="KeyExpression"/>
    /// property. Returns an ordered set of physical store key values that can be used to find the entity fast and cheap in the
    /// change tracker or in the physical store of the instance. Note that usually these represent a database specific, physical
    /// identity of the entity (e.g. "primary key", "primary, composite key", "id, partition key", etc.)
    /// </summary>
    IEnumerable<object?> IFindable.KeyValues   // (TODO: AOT candidate?)
    {
        get
        {
            if (_getKeys is not null)
                return _getKeys((TEntity)this);

            var entityParam = Expression.Parameter(typeof(TEntity), "x");
            Expression? keyExpressions;

            if (TEntity.KeyExpression.Body is UnaryExpression conversion &&
                conversion.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked &&
                conversion.Operand is MemberExpression memberExpression)
            {
                // if the lambda has the form `x => x.Id`,
                // the key expression must be `x => x.Id`
                keyExpressions = Expression.Convert(
                                    Expression.MakeMemberAccess(
                                                    entityParam,
                                                    memberExpression.Member),
                                    typeof(object));
            }
            else
            if (TEntity.KeyExpression.Body is NewExpression newExpression &&
                newExpression.Members is not null &&
                newExpression.Members.Count > 0)
            {
                // if the lambda has the form `x => new { x.Id, x.SubId }`,
                // the key expression must be `e => new[] { x.Id, x.SubId }`
                keyExpressions = Expression.NewArrayInit(
                                    typeof(object),
                                    newExpression
                                        .Members
                                        .Select<MemberInfo, Expression>(
                                            mi => typeof(TEntity).GetMember(mi.Name)[0] is PropertyInfo pi
                                                            ? Expression.Convert(
                                                                    Expression.MakeMemberAccess(entityParam, pi),
                                                                    typeof(object))
                                                            : typeof(TEntity).GetField(mi.Name) is FieldInfo fi
                                                                    ? Expression.Convert(
                                                                            Expression.MakeMemberAccess(entityParam, fi),
                                                                            typeof(object))
                                                                    : Expression.Constant(null)));
            }
            else
                throw new InvalidOperationException("""
                            The body of the KeyExpression lambda must be a member-access expression - the property of the key, e.g.
                            `public Expression<Func<Entity, object?>> KeyExpression => e => e.Id;`
                            or a new operator that creates anonymous object with the property(s) of the composite key, e.g.
                            `public Expression<Func<Entity, object?>> KeyExpression => e => new { e.Id, e.Index };`
                            """);

            // compile the lambda expression to a function that extracts the keys from an entity
            _getKeys = Expression.Lambda<Func<TEntity, IEnumerable<object?>>>(keyExpressions, entityParam).Compile();
            return _getKeys((TEntity)this);
        }
    }

    /// <inheritdoc />
    async ValueTask IFindable.ValidateFindable(object? context, CancellationToken cancellationToken)
        => await new FindableValidator(TEntity.KeyExpression).ValidateAndThrowAsync(this, cancellationToken);
}
