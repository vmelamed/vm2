namespace vm2.Repository.TestData;

public static class Labels
{
    static uint id = 0;

    public static uint NextId => Interlocked.Increment(ref id);

    static Label? _label1;
    static Label? _label2;
    static Label? _label3;
    static Label? _label4;
    static Label? _label5;
    static Label? _label6;

    static IEnumerable<Label> _allLabels = [];

    internal static IEnumerable<Label> NewLabels()
        => _allLabels = new[]
        {
            _label1 = new Label(id: NextId, name: "Columbia Records",    countryCode: "US"),
            _label2 = new Label(id: NextId, name: "Blue Note",           countryCode: "US"),
            _label3 = new Label(id: NextId, name: "Verve Records",       countryCode: "US"),
            _label4 = new Label(id: NextId, name: "Deutsche Grammophon", countryCode: "DE"),
            _label5 = new Label(id: NextId, name: "Philips",             countryCode: "NL"),
            _label6 = new Label(id: NextId, name: "Sony Classical",      countryCode: "US"),
        };

    public static IEnumerable<Label> LabelsSequence => _allLabels.Any() ? _allLabels : NewLabels();

    public static Label Label1 => _label1 ?? (NewLabels(), _label1!).Item2;
    public static Label Label2 => _label2 ?? (NewLabels(), _label2!).Item2;
    public static Label Label3 => _label3 ?? (NewLabels(), _label3!).Item2;
    public static Label Label4 => _label4 ?? (NewLabels(), _label4!).Item2;
    public static Label Label5 => _label5 ?? (NewLabels(), _label5!).Item2;
    public static Label Label6 => _label6 ?? (NewLabels(), _label6!).Item2;
}
