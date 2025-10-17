namespace vm2.Repository.TestData;

public static class Labels
{
    static UlidFactory UlidFactory { get; } = new UlidFactory();

    public static LabelId NextId => new(UlidFactory.NewUlid());

    internal static IEnumerable<Label> NewLabels()
        => LabelsSequence = new[]
        {
            Label1 = new Label(id: NextId, tenantId: TestTenant.Current(), name: "Columbia Records",    countryCode: "US"),
            Label2 = new Label(id: NextId, tenantId: TestTenant.Current(), name: "Blue Note",           countryCode: "US"),
            Label3 = new Label(id: NextId, tenantId: TestTenant.Current(), name: "Verve Records",       countryCode: "US"),
            Label4 = new Label(id: NextId, tenantId: TestTenant.Current(), name: "Deutsche Grammophon", countryCode: "DE"),
            Label5 = new Label(id: NextId, tenantId: TestTenant.Current(), name: "Philips",             countryCode: "NL"),
            Label6 = new Label(id: NextId, tenantId: TestTenant.Current(), name: "Sony Classical",      countryCode: "US"),
        };

    public static IEnumerable<Label> LabelsSequence { get => field.Any() ? field : NewLabels(); private set; } = [];

    public static Label Label1 { get => field ?? (NewLabels(), field!).Item2; private set; }
    public static Label Label2 { get => field ?? (NewLabels(), field!).Item2; private set; }
    public static Label Label3 { get => field ?? (NewLabels(), field!).Item2; private set; }
    public static Label Label4 { get => field ?? (NewLabels(), field!).Item2; private set; }
    public static Label Label5 { get => field ?? (NewLabels(), field!).Item2; private set; }
    public static Label Label6 { get => field ?? (NewLabels(), field!).Item2; private set; }
}
