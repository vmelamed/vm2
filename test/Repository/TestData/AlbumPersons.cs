namespace vm2.Repository.TestData;

public static class AlbumPersons
{
    internal static IEnumerable<AlbumPerson> NewAlbumPersons()
        => AlbumPersonsSequence = new AlbumPerson[]
        {
            // Kind of Blue
            AlbumPersonKindOfBlueMilesDavis = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonMilesDavis, ["Performer", "Bandleader"], ["tp"]),
            AlbumPersonKindOfBlueJohnColtrane = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonJohnColtrane, ["Performer"], ["ts"]),
            AlbumPersonKindOfBlueJulianCannonballAdderley = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonJulianCannonballAdderley, ["Performer"], ["as"]),
            AlbumPersonKindOfBlueBillEvans = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonBillEvans, ["Performer"], ["p"]),
            AlbumPersonKindOfBlueWyntonKelly = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonWyntonKelly, ["Performer"], ["p"]),
            AlbumPersonKindOfBluePaulChambers = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonPaulChambers, ["Performer"], ["b"]),
            AlbumPersonKindOfBlueJimmyCobb = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonJimmyCobb, ["Performer"], ["dr"]),
            AlbumPersonKindOfBlueIrvingTownsend = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonIrvingTownsend, ["Producer"], []),

            // Ellington at Newport
            AlbumPersonEllingtonAtNewportDukeEllington = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonDukeEllington, ["Performer", "Bandleader"], ["p"]),
            AlbumPersonEllingtonAtNewportPaulGonsalves = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonPaulGonsalves, ["Performer"], ["ts"]),
            AlbumPersonEllingtonAtNewportJohnnyHodges = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonJohnnyHodges, ["Performer"], ["as"]),
            AlbumPersonEllingtonAtNewportSamWoodyard = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonSamWoodyard, ["Performer"], ["dr"]),
            AlbumPersonEllingtonAtNewportJimmyWoode = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonJimmyWoode, ["Performer"], ["b"]),
            AlbumPersonEllingtonAtNewportGeorgeAvakian = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonGeorgeAvakian, ["Producer"], []),
            AlbumPersonEllingtonAtNewportDukeEllingtonArranger = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonDukeEllington, ["Arranger"], []),

            // Beethoven: Violin Concerto
            AlbumPersonBeethovenViolinConcertoLudwigVanBeethoven = new AlbumPerson(Albums.AlbumBeethovenViolinConcerto, Persons.PersonLudwigVanBeethoven, ["Composer"], []),
            AlbumPersonBeethovenViolinConcertoHenrykSzeryng = new AlbumPerson(Albums.AlbumBeethovenViolinConcerto, Persons.PersonHenrykSzeryng, ["Soloist"], ["v"]),
            AlbumPersonBeethovenViolinConcertoBerlinerPhilharmoniker = new AlbumPerson(Albums.AlbumBeethovenViolinConcerto, Persons.PersonBerlinerPhilharmoniker, ["Orchestra"], []),

            // Bach: Sonatas and Partitas for Solo Violin
            AlbumPersonBachSonatasPartitasViolinJohannSebastianBach = new AlbumPerson(Albums.AlbumBachSonatasPartitasViolin, Persons.PersonJohannSebastianBach, ["Composer"], []),
            AlbumPersonBachSonatasPartitasViolinHenrykSzeryng = new AlbumPerson(Albums.AlbumBachSonatasPartitasViolin, Persons.PersonHenrykSzeryng, ["Soloist"], ["v"]),

            // Bach: Cello Sonatas
            AlbumPersonBachCelloSonatasJohannSebastianBach = new AlbumPerson(Albums.AlbumBachCelloSonatas, Persons.PersonJohannSebastianBach, ["Composer"], []),
            AlbumPersonBachCelloSonatasMstislavRostropovich = new AlbumPerson(Albums.AlbumBachCelloSonatas, Persons.PersonMstislavRostropovich, ["Soloist"], ["vc"]),

            // Bill Evans Trio at the Village Vanguard
            AlbumPersonBillEvansVillageVanguardBillEvans = new AlbumPerson(Albums.AlbumBillEvansVillageVanguard, Persons.PersonBillEvans, ["Piano", "Bandleader"], ["p"]),
            AlbumPersonBillEvansVillageVanguardScottLaFaro = new AlbumPerson(Albums.AlbumBillEvansVillageVanguard, Persons.PersonScottLaFaro, ["Bass"], ["b"]),
            AlbumPersonBillEvansVillageVanguardPaulMotian = new AlbumPerson(Albums.AlbumBillEvansVillageVanguard, Persons.PersonPaulMotian, ["Drums"], ["dr"]),
            AlbumPersonBillEvansVillageVanguardOrrinKeepnews = new AlbumPerson(Albums.AlbumBillEvansVillageVanguard, Persons.PersonOrrinKeepnews, ["Producer"], []),
        };

    public static IEnumerable<AlbumPerson> AlbumPersonsSequence { get => field.Any() ? field : NewAlbumPersons(); private set; } = [];

    // Kind of Blue
    public static AlbumPerson AlbumPersonKindOfBlueMilesDavis { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonKindOfBlueJohnColtrane { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonKindOfBlueJulianCannonballAdderley { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonKindOfBlueBillEvans { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonKindOfBlueWyntonKelly { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonKindOfBluePaulChambers { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonKindOfBlueJimmyCobb { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonKindOfBlueIrvingTownsend { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }

    // Ellington at Newport
    public static AlbumPerson AlbumPersonEllingtonAtNewportDukeEllington { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonEllingtonAtNewportPaulGonsalves { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonEllingtonAtNewportJohnnyHodges { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonEllingtonAtNewportSamWoodyard { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonEllingtonAtNewportJimmyWoode { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonEllingtonAtNewportGeorgeAvakian { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonEllingtonAtNewportDukeEllingtonArranger { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }

    // Beethoven: Violin Concerto
    public static AlbumPerson AlbumPersonBeethovenViolinConcertoLudwigVanBeethoven { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonBeethovenViolinConcertoHenrykSzeryng { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonBeethovenViolinConcertoBerlinerPhilharmoniker { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }

    // Bach: Sonatas and Partitas for Solo Violin
    public static AlbumPerson AlbumPersonBachSonatasPartitasViolinJohannSebastianBach { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonBachSonatasPartitasViolinHenrykSzeryng { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }

    // Bach: Cello Sonatas
    public static AlbumPerson AlbumPersonBachCelloSonatasJohannSebastianBach { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonBachCelloSonatasMstislavRostropovich { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }

    // Bill Evans Trio at the Village Vanguard
    public static AlbumPerson AlbumPersonBillEvansVillageVanguardBillEvans { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonBillEvansVillageVanguardScottLaFaro { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonBillEvansVillageVanguardPaulMotian { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
    public static AlbumPerson AlbumPersonBillEvansVillageVanguardOrrinKeepnews { get => field ?? (NewAlbumPersons(), field!).Item2; private set; }
}