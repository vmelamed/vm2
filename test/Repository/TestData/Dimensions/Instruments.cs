namespace vm2.Repository.TestData.Dimensions;

public static class Instruments
{
    internal static IEnumerable<Instrument> NewInstruments()
        => InstrumentsSequence = new[]
        {
            Instrument01 = new Instrument(Code: "ts",         Name: "Tenor Saxophone"),
            Instrument02 = new Instrument(Code: "as",         Name: "Alto Saxophone"),
            Instrument03 = new Instrument(Code: "ss",         Name: "Soprano Saxophone"),
            Instrument04 = new Instrument(Code: "bs",         Name: "Baritone Saxophone"),
            Instrument05 = new Instrument(Code: "tp",         Name: "Trumpet"),
            Instrument06 = new Instrument(Code: "tb",         Name: "Trombone"),
            Instrument07 = new Instrument(Code: "p",          Name: "piano"),
            Instrument08 = new Instrument(Code: "b",          Name: "Double Bass"),
            Instrument09 = new Instrument(Code: "dr",         Name: "Drums"),
            Instrument10 = new Instrument(Code: "v",          Name: "Violin"),
            Instrument11 = new Instrument(Code: "va",         Name: "Viola"),
            Instrument12 = new Instrument(Code: "c",          Name: "cello"),
            Instrument13 = new Instrument(Code: "fl",         Name: "Flute"),
            Instrument14 = new Instrument(Code: "cl",         Name: "Clarinet"),
            Instrument15 = new Instrument(Code: "ob",         Name: "Oboe"),
            Instrument16 = new Instrument(Code: "fg",         Name: "Bassoon"),
            Instrument17 = new Instrument(Code: "hrn",        Name: "French Horn"),
            Instrument18 = new Instrument(Code: "perc",       Name: "Percussion"),
            Instrument19 = new Instrument(Code: "g",          Name: "guitar"),
            Instrument20 = new Instrument(Code: "voc",        Name: "Vocals"),
        };

    public static IEnumerable<Instrument> InstrumentsSequence { get => field.Any() ? field : NewInstruments(); private set; } = [];

    public static Instrument Instrument01 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument02 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument03 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument04 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument05 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument06 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument07 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument08 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument09 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument10 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument11 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument12 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument13 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument14 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument15 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument16 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument17 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument18 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument19 { get => field ?? (NewInstruments(), field!).Item2; private set; }
    public static Instrument Instrument20 { get => field ?? (NewInstruments(), field!).Item2; private set; }
}
