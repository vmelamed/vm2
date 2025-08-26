namespace vm2.Repository.FakeDbSet;

#pragma warning disable IDE0079
#pragma warning disable CS0067, IDE1006, EF1001

public partial class FakeDbSet<TEntity>
{
    static readonly StubStateManager DefaultStubStateManager = new();
    static readonly StubEntityType DefaultStubEntityType = new();
    static readonly StubSnapshot DefaultStubSnapshot = new();
    static readonly StubPropertyCounts DefaultStubPropertyCounts = new();
    static readonly StubServiceProvider DefaultStubServiceProvider = new();
    class StubStateManager : IStateManager
    {
        public static IStateManager Instance { get; } = DefaultStubStateManager;

        public StateManagerDependencies Dependencies => throw new NotImplementedException();

        public CascadeTiming DeleteOrphansTiming { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CascadeTiming CascadeDeleteTiming { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool SavingChanges => throw new NotImplementedException();

        public IEnumerable<InternalEntityEntry> Entries => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public int ChangedCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IInternalEntityEntryNotifier InternalEntityEntryNotifier => throw new NotImplementedException();

        public IValueGenerationManager ValueGenerationManager => throw new NotImplementedException();

        public DbContext Context => throw new NotImplementedException();

        public IModel Model => throw new NotImplementedException();

        public IEntityMaterializerSource EntityMaterializerSource => throw new NotImplementedException();

        public bool SensitiveLoggingEnabled => throw new NotImplementedException();

        public IDiagnosticsLogger<DbLoggerCategory.Update> UpdateLogger => throw new NotImplementedException();

        public event EventHandler<EntityTrackingEventArgs>? Tracking;
        public event EventHandler<EntityTrackedEventArgs>? Tracked;
        public event EventHandler<EntityStateChangingEventArgs>? StateChanging;
        public event EventHandler<EntityStateChangedEventArgs>? StateChanged;

        public void AbortAttachGraph() => throw new NotImplementedException();
        public void AcceptAllChanges() => throw new NotImplementedException();
        public void BeginAttachGraph() => throw new NotImplementedException();
        public (EventHandler<EntityTrackingEventArgs>? Tracking, EventHandler<EntityTrackedEventArgs>? Tracked, EventHandler<EntityStateChangingEventArgs>? StateChanging, EventHandler<EntityStateChangedEventArgs>? StateChanged) CaptureEvents() => throw new NotImplementedException();
        public void CascadeChanges(bool force) => throw new NotImplementedException();
        public void CascadeDelete(InternalEntityEntry entry, bool force, IEnumerable<IForeignKey>? foreignKeys = null) => throw new NotImplementedException();
        public void ChangingState(InternalEntityEntry entry, EntityState newState) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();
        public void CompleteAttachGraph() => throw new NotImplementedException();
        public IEntityFinder CreateEntityFinder(IEntityType entityType) => throw new NotImplementedException();
        public InternalEntityEntry CreateEntry(IDictionary<string, object?> values, IEntityType entityType) => throw new NotImplementedException();
        public InternalEntityEntry? FindPrincipal(InternalEntityEntry dependentEntry, IForeignKey foreignKey) => throw new NotImplementedException();
        public InternalEntityEntry? FindPrincipalUsingPreStoreGeneratedValues(InternalEntityEntry dependentEntry, IForeignKey foreignKey) => throw new NotImplementedException();
        public InternalEntityEntry? FindPrincipalUsingRelationshipSnapshot(InternalEntityEntry dependentEntry, IForeignKey foreignKey) => throw new NotImplementedException();
        public int GetCountForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false, bool returnSharedIdentity = false) => throw new NotImplementedException();
        public IEnumerable<IUpdateEntry> GetDependents(IUpdateEntry principalEntry, IForeignKey foreignKey) => throw new NotImplementedException();
        public IEnumerable<IUpdateEntry>? GetDependentsFromNavigation(IUpdateEntry principalEntry, IForeignKey foreignKey) => throw new NotImplementedException();
        public IEnumerable<IUpdateEntry> GetDependentsUsingRelationshipSnapshot(IUpdateEntry principalEntry, IForeignKey foreignKey) => throw new NotImplementedException();
        public IEnumerable<InternalEntityEntry> GetEntriesForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false, bool returnSharedIdentity = false) => throw new NotImplementedException();
        public IList<IUpdateEntry> GetEntriesToSave(bool cascadeChanges) => throw new NotImplementedException();
        public IEnumerable<TEntity1> GetNonDeletedEntities<TEntity1>() where TEntity1 : class => throw new NotImplementedException();
        public InternalEntityEntry GetOrCreateEntry(object entity) => throw new NotImplementedException();
        public InternalEntityEntry GetOrCreateEntry(object entity, IEntityType? entityType) => throw new NotImplementedException();
        public IEnumerable<Tuple<INavigationBase, InternalEntityEntry>> GetRecordedReferrers(object referencedEntity, bool clear) => throw new NotImplementedException();
        public void OnStateChanged(InternalEntityEntry internalEntityEntry, EntityState oldState) => throw new NotImplementedException();
        public void OnStateChanging(InternalEntityEntry internalEntityEntry, EntityState newState) => throw new NotImplementedException();
        public void OnTracked(InternalEntityEntry internalEntityEntry, bool fromQuery) => throw new NotImplementedException();
        public void OnTracking(InternalEntityEntry internalEntityEntry, EntityState state, bool fromQuery) => throw new NotImplementedException();
        public void RecordReferencedUntrackedEntity(object referencedEntity, INavigationBase navigation, InternalEntityEntry referencedFromEntry) => throw new NotImplementedException();
        public void ResetState() => throw new NotImplementedException();
        public Task ResetStateAsync(CancellationToken ct = default) => throw new NotImplementedException();
        public bool ResolveToExistingEntry(InternalEntityEntry newEntry, INavigationBase? navigation, InternalEntityEntry? referencedFromEntry) => throw new NotImplementedException();
        public int SaveChanges(bool acceptAllChangesOnSuccess) => throw new NotImplementedException();
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken ct = default) => throw new NotImplementedException();
        public void SetEvents(EventHandler<EntityTrackingEventArgs>? tracking, EventHandler<EntityTrackedEventArgs>? tracked, EventHandler<EntityStateChangingEventArgs>? stateChanging, EventHandler<EntityStateChangedEventArgs>? stateChanged) => throw new NotImplementedException();
        public InternalEntityEntry StartTracking(InternalEntityEntry entry) => throw new NotImplementedException();
        public InternalEntityEntry StartTrackingFromQuery(IEntityType baseEntityType, object entity, in ValueBuffer valueBuffer) => throw new NotImplementedException();
        public void StopTracking(InternalEntityEntry entry, EntityState oldState) => throw new NotImplementedException();
        public InternalEntityEntry? TryGetEntry(IKey key, object?[] keyValues) => throw new NotImplementedException();
        public InternalEntityEntry? TryGetEntry(IKey key, object?[] keyValues, bool throwOnNullKey, out bool hasNullKey) => throw new NotImplementedException();
        public InternalEntityEntry? TryGetEntry(object entity, bool throwOnNonUniqueness = true) => throw new NotImplementedException();
        public InternalEntityEntry? TryGetEntry(object entity, IEntityType type, bool throwOnTypeMismatch = true) => throw new NotImplementedException();
        public void Unsubscribe() => throw new NotImplementedException();
        public void UpdateDependentMap(InternalEntityEntry entry, IForeignKey foreignKey) => throw new NotImplementedException();
        public void UpdateIdentityMap(InternalEntityEntry entry, IKey principalKey) => throw new NotImplementedException();
        public void UpdateReferencedUntrackedEntity(object referencedEntity, object newReferencedEntity, INavigationBase navigation, InternalEntityEntry referencedFromEntry) => throw new NotImplementedException();
        public InternalEntityEntry? TryGetEntry(IKey key, IReadOnlyList<object?> keyValues) => throw new NotImplementedException();
        public InternalEntityEntry? TryGetEntryTyped<TKey>(IKey key, TKey keyValue) => throw new NotImplementedException();
        public InternalEntityEntry? TryGetExistingEntry(object entity, IKey key) => throw new NotImplementedException();
        public IEnumerable<InternalEntityEntry> GetEntries(IKey key) => throw new NotImplementedException();
        public IEnumerable<IUpdateEntry> GetDependents(IReadOnlyList<object> keyValues, IForeignKey foreignKey) => throw new NotImplementedException();
        public void Unsubscribe(bool resetting) => throw new NotImplementedException();
        public void Clear(bool resetting) => throw new NotImplementedException();
        public InternalEntityEntry StartTrackingFromQuery(IEntityType baseEntityType, object entity, in ISnapshot snapshot) => throw new NotImplementedException();
    }

    class StubEntityType : IEntityType, IRuntimeEntityType
    {
        public static IEntityType Instance { get; } = DefaultStubEntityType;

        public object? this[string name] => throw new NotImplementedException();

        public IEntityType? BaseType => throw new NotImplementedException();

        public InstantiationBinding? ConstructorBinding => throw new NotImplementedException();

        public InstantiationBinding? ServiceOnlyConstructorBinding => throw new NotImplementedException();

        public IModel Model => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public Type ClrType => throw new NotImplementedException();

        public bool HasSharedClrType => throw new NotImplementedException();

        public bool IsPropertyBag => throw new NotImplementedException();

        IReadOnlyEntityType? IReadOnlyEntityType.BaseType => throw new NotImplementedException();

        IReadOnlyModel IReadOnlyTypeBase.Model => throw new NotImplementedException();

        IEntityType? IEntityType.BaseType => throw new NotImplementedException();

        InstantiationBinding? IEntityType.ServiceOnlyConstructorBinding => throw new NotImplementedException();

        IModel ITypeBase.Model => throw new NotImplementedException();

        string IReadOnlyTypeBase.Name => throw new NotImplementedException();

        Type IReadOnlyTypeBase.ClrType => throw new NotImplementedException();

        bool IReadOnlyTypeBase.HasSharedClrType => throw new NotImplementedException();

        bool IReadOnlyTypeBase.IsPropertyBag => throw new NotImplementedException();

        object? IReadOnlyAnnotatable.this[string name] => throw new NotImplementedException();

        public IAnnotation AddRuntimeAnnotation(string name, object? value) => throw new NotImplementedException();
        public IAnnotation? FindAnnotation(string name) => throw new NotImplementedException();
        public IEnumerable<IForeignKey> FindDeclaredForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        public INavigation? FindDeclaredNavigation(string name) => throw new NotImplementedException();
        public IProperty? FindDeclaredProperty(string name) => throw new NotImplementedException();
        public ITrigger? FindDeclaredTrigger(string name) => throw new NotImplementedException();
        public IForeignKey? FindForeignKey(IReadOnlyList<IReadOnlyProperty> properties, IReadOnlyKey principalKey, IReadOnlyEntityType principalEntityType) => throw new NotImplementedException();
        public IEnumerable<IForeignKey> FindForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        public IIndex? FindIndex(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        public IIndex? FindIndex(string name) => throw new NotImplementedException();
        public PropertyInfo? FindIndexerPropertyInfo() => throw new NotImplementedException();
        public IKey? FindKey(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        public IKey? FindPrimaryKey() => throw new NotImplementedException();
        public IReadOnlyList<IReadOnlyProperty>? FindProperties(IReadOnlyList<string> propertyNames) => throw new NotImplementedException();
        public IProperty? FindProperty(string name) => throw new NotImplementedException();
        public IAnnotation? FindRuntimeAnnotation(string name) => throw new NotImplementedException();
        public IServiceProperty? FindServiceProperty(string name) => throw new NotImplementedException();
        public ISkipNavigation? FindSkipNavigation(string name) => throw new NotImplementedException();
        public IEnumerable<IAnnotation> GetAnnotations() => throw new NotImplementedException();
        public ChangeTrackingStrategy GetChangeTrackingStrategy() => throw new NotImplementedException();
        public IEnumerable<IForeignKey> GetDeclaredForeignKeys() => throw new NotImplementedException();
        public IEnumerable<IIndex> GetDeclaredIndexes() => throw new NotImplementedException();
        public IEnumerable<IKey> GetDeclaredKeys() => throw new NotImplementedException();
        public IEnumerable<INavigation> GetDeclaredNavigations() => throw new NotImplementedException();
        public IEnumerable<IProperty> GetDeclaredProperties() => throw new NotImplementedException();
        public IEnumerable<IForeignKey> GetDeclaredReferencingForeignKeys() => throw new NotImplementedException();
        public IEnumerable<IServiceProperty> GetDeclaredServiceProperties() => throw new NotImplementedException();
        public IEnumerable<IReadOnlySkipNavigation> GetDeclaredSkipNavigations() => throw new NotImplementedException();
        public IEnumerable<ITrigger> GetDeclaredTriggers() => throw new NotImplementedException();
        public IEnumerable<IForeignKey> GetDerivedForeignKeys() => throw new NotImplementedException();
        public IEnumerable<IIndex> GetDerivedIndexes() => throw new NotImplementedException();
        public IEnumerable<IReadOnlyNavigation> GetDerivedNavigations() => throw new NotImplementedException();
        public IEnumerable<IReadOnlyProperty> GetDerivedProperties() => throw new NotImplementedException();
        public IEnumerable<IReadOnlyServiceProperty> GetDerivedServiceProperties() => throw new NotImplementedException();
        public IEnumerable<IReadOnlySkipNavigation> GetDerivedSkipNavigations() => throw new NotImplementedException();
        public IEnumerable<IReadOnlyEntityType> GetDerivedTypes() => throw new NotImplementedException();
        public IEnumerable<IEntityType> GetDirectlyDerivedTypes() => throw new NotImplementedException();
        public string? GetDiscriminatorPropertyName() => throw new NotImplementedException();
        public IEnumerable<IProperty> GetForeignKeyProperties() => throw new NotImplementedException();
        public IEnumerable<IForeignKey> GetForeignKeys() => throw new NotImplementedException();
        public IEnumerable<IIndex> GetIndexes() => throw new NotImplementedException();
        public IEnumerable<IKey> GetKeys() => throw new NotImplementedException();
        public PropertyAccessMode GetNavigationAccessMode() => throw new NotImplementedException();
        public IEnumerable<INavigation> GetNavigations() => throw new NotImplementedException();
        public TValue GetOrAddRuntimeAnnotationValue<TValue, TArg>(string name, Func<TArg?, TValue> valueFactory, TArg? factoryArgument) => throw new NotImplementedException();
        public IEnumerable<IProperty> GetProperties() => throw new NotImplementedException();
        public PropertyAccessMode GetPropertyAccessMode() => throw new NotImplementedException();
        public LambdaExpression? GetQueryFilter() => throw new NotImplementedException();
        public IEnumerable<IForeignKey> GetReferencingForeignKeys() => throw new NotImplementedException();
        public IEnumerable<IAnnotation> GetRuntimeAnnotations() => throw new NotImplementedException();
        public IEnumerable<IDictionary<string, object?>> GetSeedData(bool providerValues = false) => throw new NotImplementedException();
        public IEnumerable<IServiceProperty> GetServiceProperties() => throw new NotImplementedException();
        public IEnumerable<ISkipNavigation> GetSkipNavigations() => throw new NotImplementedException();
        public IEnumerable<IProperty> GetValueGeneratingProperties() => throw new NotImplementedException();
        public IAnnotation? RemoveRuntimeAnnotation(string name) => throw new NotImplementedException();
        public IAnnotation SetRuntimeAnnotation(string name, object? value) => throw new NotImplementedException();
        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.FindDeclaredForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        IReadOnlyNavigation? IReadOnlyEntityType.FindDeclaredNavigation(string name) => throw new NotImplementedException();
        IReadOnlyTrigger? IReadOnlyEntityType.FindDeclaredTrigger(string name) => throw new NotImplementedException();
        IReadOnlyForeignKey? IReadOnlyEntityType.FindForeignKey(IReadOnlyList<IReadOnlyProperty> properties, IReadOnlyKey principalKey, IReadOnlyEntityType principalEntityType) => throw new NotImplementedException();
        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.FindForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        IReadOnlyIndex? IReadOnlyEntityType.FindIndex(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        IReadOnlyIndex? IReadOnlyEntityType.FindIndex(string name) => throw new NotImplementedException();
        IReadOnlyKey? IReadOnlyEntityType.FindKey(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        IReadOnlyKey? IReadOnlyEntityType.FindPrimaryKey() => throw new NotImplementedException();
        IReadOnlyServiceProperty? IReadOnlyEntityType.FindServiceProperty(string name) => throw new NotImplementedException();
        IReadOnlySkipNavigation? IReadOnlyEntityType.FindSkipNavigation(string name) => throw new NotImplementedException();
        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDeclaredForeignKeys() => throw new NotImplementedException();
        IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetDeclaredIndexes() => throw new NotImplementedException();
        IEnumerable<IReadOnlyKey> IReadOnlyEntityType.GetDeclaredKeys() => throw new NotImplementedException();
        IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetDeclaredNavigations() => throw new NotImplementedException();
        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDeclaredReferencingForeignKeys() => throw new NotImplementedException();
        IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetDeclaredServiceProperties() => throw new NotImplementedException();
        IEnumerable<IReadOnlyTrigger> IReadOnlyEntityType.GetDeclaredTriggers() => throw new NotImplementedException();
        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDerivedForeignKeys() => throw new NotImplementedException();
        IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetDerivedIndexes() => throw new NotImplementedException();
        IEnumerable<IReadOnlyEntityType> IReadOnlyEntityType.GetDirectlyDerivedTypes() => throw new NotImplementedException();
        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetForeignKeys() => throw new NotImplementedException();
        IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetIndexes() => throw new NotImplementedException();
        IEnumerable<IReadOnlyKey> IReadOnlyEntityType.GetKeys() => throw new NotImplementedException();
        IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetNavigations() => throw new NotImplementedException();
        IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetReferencingForeignKeys() => throw new NotImplementedException();
        IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetServiceProperties() => throw new NotImplementedException();
        IEnumerable<IReadOnlySkipNavigation> IReadOnlyEntityType.GetSkipNavigations() => throw new NotImplementedException();
        IEnumerable<IEntityType> IEntityType.GetDirectlyDerivedTypes() => throw new NotImplementedException();
        IKey? IEntityType.FindPrimaryKey() => throw new NotImplementedException();
        IKey? IEntityType.FindKey(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        IEnumerable<IKey> IEntityType.GetDeclaredKeys() => throw new NotImplementedException();
        IEnumerable<IKey> IEntityType.GetKeys() => throw new NotImplementedException();
        IForeignKey? IEntityType.FindForeignKey(IReadOnlyList<IReadOnlyProperty> properties, IReadOnlyKey principalKey, IReadOnlyEntityType principalEntityType) => throw new NotImplementedException();
        IEnumerable<IForeignKey> IEntityType.FindForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        IEnumerable<IForeignKey> IEntityType.FindDeclaredForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        IEnumerable<IForeignKey> IEntityType.GetDeclaredForeignKeys() => throw new NotImplementedException();
        IEnumerable<IForeignKey> IEntityType.GetDerivedForeignKeys() => throw new NotImplementedException();
        IEnumerable<IForeignKey> IEntityType.GetForeignKeys() => throw new NotImplementedException();
        IEnumerable<IForeignKey> IEntityType.GetReferencingForeignKeys() => throw new NotImplementedException();
        IEnumerable<IForeignKey> IEntityType.GetDeclaredReferencingForeignKeys() => throw new NotImplementedException();
        INavigation? IEntityType.FindDeclaredNavigation(string name) => throw new NotImplementedException();
        IEnumerable<INavigation> IEntityType.GetDeclaredNavigations() => throw new NotImplementedException();
        IEnumerable<INavigation> IEntityType.GetNavigations() => throw new NotImplementedException();
        ISkipNavigation? IEntityType.FindSkipNavigation(string name) => throw new NotImplementedException();
        IEnumerable<ISkipNavigation> IEntityType.GetSkipNavigations() => throw new NotImplementedException();
        IIndex? IEntityType.FindIndex(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();
        IIndex? IEntityType.FindIndex(string name) => throw new NotImplementedException();
        IEnumerable<IIndex> IEntityType.GetDeclaredIndexes() => throw new NotImplementedException();
        IEnumerable<IIndex> IEntityType.GetDerivedIndexes() => throw new NotImplementedException();
        IEnumerable<IIndex> IEntityType.GetIndexes() => throw new NotImplementedException();
        IProperty? IEntityType.FindProperty(string name) => throw new NotImplementedException();
        IProperty? IEntityType.FindDeclaredProperty(string name) => throw new NotImplementedException();
        IEnumerable<IProperty> IEntityType.GetDeclaredProperties() => throw new NotImplementedException();
        IEnumerable<IProperty> IEntityType.GetProperties() => [];
        IEnumerable<IProperty> IEntityType.GetForeignKeyProperties() => throw new NotImplementedException();
        IEnumerable<IProperty> IEntityType.GetValueGeneratingProperties() => throw new NotImplementedException();
        IServiceProperty? IEntityType.FindServiceProperty(string name) => throw new NotImplementedException();
        IEnumerable<IServiceProperty> IEntityType.GetDeclaredServiceProperties() => throw new NotImplementedException();
        IEnumerable<IServiceProperty> IEntityType.GetServiceProperties() => throw new NotImplementedException();
        ITrigger? IEntityType.FindDeclaredTrigger(string name) => throw new NotImplementedException();
        IEnumerable<ITrigger> IEntityType.GetDeclaredTriggers() => throw new NotImplementedException();
        IEnumerable<IDictionary<string, object?>> IReadOnlyEntityType.GetSeedData(bool providerValues) => throw new NotImplementedException();
        LambdaExpression? IReadOnlyEntityType.GetQueryFilter() => throw new NotImplementedException();
        string? IReadOnlyEntityType.GetDiscriminatorPropertyName() => throw new NotImplementedException();
        IEnumerable<IReadOnlyEntityType> IReadOnlyEntityType.GetDerivedTypes() => throw new NotImplementedException();
        IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetDerivedNavigations() => throw new NotImplementedException();
        IEnumerable<IReadOnlySkipNavigation> IReadOnlyEntityType.GetDeclaredSkipNavigations() => throw new NotImplementedException();
        IEnumerable<IReadOnlySkipNavigation> IReadOnlyEntityType.GetDerivedSkipNavigations() => throw new NotImplementedException();
        IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetDerivedServiceProperties() => throw new NotImplementedException();
        PropertyAccessMode IReadOnlyTypeBase.GetPropertyAccessMode() => throw new NotImplementedException();
        PropertyInfo? IReadOnlyTypeBase.FindIndexerPropertyInfo() => throw new NotImplementedException();
        IAnnotation? IAnnotatable.FindRuntimeAnnotation(string name) => throw new NotImplementedException();
        IEnumerable<IAnnotation> IAnnotatable.GetRuntimeAnnotations() => throw new NotImplementedException();
        IAnnotation IAnnotatable.AddRuntimeAnnotation(string name, object? value) => throw new NotImplementedException();
        IAnnotation IAnnotatable.SetRuntimeAnnotation(string name, object? value) => throw new NotImplementedException();
        IAnnotation? IAnnotatable.RemoveRuntimeAnnotation(string name) => throw new NotImplementedException();
        TValue IAnnotatable.GetOrAddRuntimeAnnotationValue<TValue, TArg>(string name, Func<TArg?, TValue> valueFactory, TArg? factoryArgument) where TArg : default => throw new NotImplementedException();
        IAnnotation? IReadOnlyAnnotatable.FindAnnotation(string name) => throw new NotImplementedException();
        IEnumerable<IAnnotation> IReadOnlyAnnotatable.GetAnnotations() => throw new NotImplementedException();

        PropertyCounts IRuntimeEntityType.Counts => DefaultStubPropertyCounts;
        Func<InternalEntityEntry, ISnapshot> IRuntimeEntityType.RelationshipSnapshotFactory => throw new NotImplementedException();
        Func<InternalEntityEntry, ISnapshot> IRuntimeEntityType.OriginalValuesFactory => throw new NotImplementedException();
        Func<ISnapshot> IRuntimeEntityType.StoreGeneratedValuesFactory => throw new NotImplementedException();
        Func<InternalEntityEntry, ISnapshot> IRuntimeEntityType.TemporaryValuesFactory => throw new NotImplementedException();
        Func<ISnapshot> IRuntimeEntityType.EmptyShadowValuesFactory => () => DefaultStubSnapshot;
        public bool HasServiceProperties() => throw new NotImplementedException();
        public Func<MaterializationContext, object> GetOrCreateMaterializer(IEntityMaterializerSource source) => throw new NotImplementedException();
        public Func<MaterializationContext, object> GetOrCreateEmptyMaterializer(IEntityMaterializerSource source) => throw new NotImplementedException();
        public IComplexProperty? FindComplexProperty(string name) => throw new NotImplementedException();
        public IEnumerable<IComplexProperty> GetComplexProperties() => throw new NotImplementedException();
        public IEnumerable<IComplexProperty> GetDeclaredComplexProperties() => throw new NotImplementedException();
        public IEnumerable<IPropertyBase> GetMembers() => throw new NotImplementedException();
        public IEnumerable<IPropertyBase> GetDeclaredMembers() => throw new NotImplementedException();
        public IPropertyBase? FindMember(string name) => throw new NotImplementedException();
        public IEnumerable<IPropertyBase> FindMembersInHierarchy(string name) => throw new NotImplementedException();
        public IEnumerable<IPropertyBase> GetSnapshottableMembers() => throw new NotImplementedException();
        public IEnumerable<IProperty> GetFlattenedProperties() => [];
        public IEnumerable<IComplexProperty> GetFlattenedComplexProperties() => throw new NotImplementedException();
        public IEnumerable<IProperty> GetFlattenedDeclaredProperties() => throw new NotImplementedException();
        IReadOnlyProperty? IReadOnlyTypeBase.FindProperty(string name) => throw new NotImplementedException();
        IReadOnlyProperty? IReadOnlyTypeBase.FindDeclaredProperty(string name) => throw new NotImplementedException();
        IEnumerable<IReadOnlyProperty> IReadOnlyTypeBase.GetDeclaredProperties() => throw new NotImplementedException();
        IEnumerable<IReadOnlyProperty> IReadOnlyTypeBase.GetProperties() => throw new NotImplementedException();
        IReadOnlyComplexProperty? IReadOnlyTypeBase.FindComplexProperty(string name) => throw new NotImplementedException();
        public IReadOnlyComplexProperty? FindDeclaredComplexProperty(string name) => throw new NotImplementedException();
        IEnumerable<IReadOnlyComplexProperty> IReadOnlyTypeBase.GetComplexProperties() => throw new NotImplementedException();
        IEnumerable<IReadOnlyComplexProperty> IReadOnlyTypeBase.GetDeclaredComplexProperties() => throw new NotImplementedException();
        public IEnumerable<IReadOnlyComplexProperty> GetDerivedComplexProperties() => throw new NotImplementedException();
        IEnumerable<IReadOnlyPropertyBase> IReadOnlyTypeBase.GetMembers() => throw new NotImplementedException();
        IEnumerable<IReadOnlyPropertyBase> IReadOnlyTypeBase.GetDeclaredMembers() => throw new NotImplementedException();
        IReadOnlyPropertyBase? IReadOnlyTypeBase.FindMember(string name) => throw new NotImplementedException();
        IEnumerable<IReadOnlyPropertyBase> IReadOnlyTypeBase.FindMembersInHierarchy(string name) => throw new NotImplementedException();
        public ConfigurationSource? GetConstructorBindingConfigurationSource() => throw new NotImplementedException();
        public ConfigurationSource? GetServiceOnlyConstructorBindingConfigurationSource() => throw new NotImplementedException();
        public Func<IDictionary<string, object?>, ISnapshot> ShadowValuesFactory => d => new StubSnapshot();
    }

    class StubSnapshot : ISnapshot
    {
        public object? this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public T GetValue<T>(int index) => throw new NotImplementedException();
    }

    class StubPropertyCounts : PropertyCounts
    {
        public StubPropertyCounts() : base(1, 0, 1, 1, 0, 1, 0)
        {
        }
    }

    class StubServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType) => default;
    }
}
