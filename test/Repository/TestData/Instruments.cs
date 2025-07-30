namespace vm2.Repository.TestData;

public static class Instruments
{
    static Instrument? _instrument01;
    static Instrument? _instrument02;
    static Instrument? _instrument03;
    static Instrument? _instrument04;
    static Instrument? _instrument05;
    static Instrument? _instrument06;
    static Instrument? _instrument07;
    static Instrument? _instrument08;
    static Instrument? _instrument09;
    static Instrument? _instrument10;
    static Instrument? _instrument11;
    static Instrument? _instrument12;
    static Instrument? _instrument13;
    static Instrument? _instrument14;
    static Instrument? _instrument15;
    static Instrument? _instrument16;
    static Instrument? _instrument17;
    static Instrument? _instrument18;
    static Instrument? _instrument19;
    static Instrument? _instrument20;

    static IEnumerable<Instrument> _allInstruments = [];

    internal static IEnumerable<Instrument> NewInstruments()
        => _allInstruments = new[]
        {
            _instrument01 = new Instrument(Code: "ts",         Name: "Tenor Saxophone"),
            _instrument02 = new Instrument(Code: "as",         Name: "Alto Saxophone"),
            _instrument03 = new Instrument(Code: "ss",         Name: "Soprano Saxophone"),
            _instrument04 = new Instrument(Code: "bs",         Name: "Baritone Saxophone"),
            _instrument05 = new Instrument(Code: "tp",         Name: "Trumpet"),
            _instrument06 = new Instrument(Code: "tb",         Name: "Trombone"),
            _instrument07 = new Instrument(Code: "p",          Name: "piano"),
            _instrument08 = new Instrument(Code: "b",          Name: "Double Bass"),
            _instrument09 = new Instrument(Code: "dr",         Name: "Drums"),
            _instrument10 = new Instrument(Code: "v",          Name: "Violin"),
            _instrument11 = new Instrument(Code: "va",         Name: "Viola"),
            _instrument12 = new Instrument(Code: "c",          Name: "cello"),
            _instrument13 = new Instrument(Code: "fl",         Name: "Flute"),
            _instrument14 = new Instrument(Code: "cl",         Name: "Clarinet"),
            _instrument15 = new Instrument(Code: "ob",         Name: "Oboe"),
            _instrument16 = new Instrument(Code: "fg",         Name: "Bassoon"),
            _instrument17 = new Instrument(Code: "hrn",        Name: "French Horn"),
            _instrument18 = new Instrument(Code: "perc",       Name: "Percussion"),
            _instrument19 = new Instrument(Code: "g",          Name: "guitar"),
            _instrument20 = new Instrument(Code: "voc",        Name: "Vocals"),
        };

    public static IEnumerable<Instrument> InstrumentsSequence => _allInstruments.Any() ? _allInstruments : NewInstruments();

    public static Instrument Instrument01 => _instrument01 ?? (NewInstruments(), _instrument01!).Item2;
    public static Instrument Instrument02 => _instrument02 ?? (NewInstruments(), _instrument02!).Item2;
    public static Instrument Instrument03 => _instrument03 ?? (NewInstruments(), _instrument03!).Item2;
    public static Instrument Instrument04 => _instrument04 ?? (NewInstruments(), _instrument04!).Item2;
    public static Instrument Instrument05 => _instrument05 ?? (NewInstruments(), _instrument05!).Item2;
    public static Instrument Instrument06 => _instrument06 ?? (NewInstruments(), _instrument06!).Item2;
    public static Instrument Instrument07 => _instrument07 ?? (NewInstruments(), _instrument07!).Item2;
    public static Instrument Instrument08 => _instrument08 ?? (NewInstruments(), _instrument08!).Item2;
    public static Instrument Instrument09 => _instrument09 ?? (NewInstruments(), _instrument09!).Item2;
    public static Instrument Instrument10 => _instrument10 ?? (NewInstruments(), _instrument10!).Item2;
    public static Instrument Instrument11 => _instrument11 ?? (NewInstruments(), _instrument11!).Item2;
    public static Instrument Instrument12 => _instrument12 ?? (NewInstruments(), _instrument12!).Item2;
    public static Instrument Instrument13 => _instrument13 ?? (NewInstruments(), _instrument13!).Item2;
    public static Instrument Instrument14 => _instrument14 ?? (NewInstruments(), _instrument14!).Item2;
    public static Instrument Instrument15 => _instrument15 ?? (NewInstruments(), _instrument15!).Item2;
    public static Instrument Instrument16 => _instrument16 ?? (NewInstruments(), _instrument16!).Item2;
    public static Instrument Instrument19 => _instrument19 ?? (NewInstruments(), _instrument19!).Item2;
    public static Instrument Instrument20 => _instrument20 ?? (NewInstruments(), _instrument20!).Item2;
}
