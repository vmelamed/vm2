namespace vm2.Repository.Tests.TestData;

public static class AlbumPersons
{
    // Kind of Blue
    static AlbumPerson? _albumPersonKindOfBlueMilesDavis;
    static AlbumPerson? _albumPersonKindOfBlueJohnColtrane;
    static AlbumPerson? _albumPersonKindOfBlueJulianCannonballAdderley;
    static AlbumPerson? _albumPersonKindOfBlueBillEvans;
    static AlbumPerson? _albumPersonKindOfBlueWyntonKelly;
    static AlbumPerson? _albumPersonKindOfBluePaulChambers;
    static AlbumPerson? _albumPersonKindOfBlueJimmyCobb;
    static AlbumPerson? _albumPersonKindOfBlueIrvingTownsend; // Producer

    // Ellington at Newport
    static AlbumPerson? _albumPersonEllingtonAtNewportDukeEllington;
    static AlbumPerson? _albumPersonEllingtonAtNewportPaulGonsalves;
    static AlbumPerson? _albumPersonEllingtonAtNewportJohnnyHodges;
    static AlbumPerson? _albumPersonEllingtonAtNewportSamWoodyard;
    static AlbumPerson? _albumPersonEllingtonAtNewportJimmyWoode;
    static AlbumPerson? _albumPersonEllingtonAtNewportGeorgeAvakian; // Producer
    static AlbumPerson? _albumPersonEllingtonAtNewportDukeEllingtonArranger; // Arranger

    // Beethoven: Violin Concerto
    static AlbumPerson? _albumPersonBeethovenViolinConcertoLudwigVanBeethoven;
    static AlbumPerson? _albumPersonBeethovenViolinConcertoHenrykSzeryng;
    static AlbumPerson? _albumPersonBeethovenViolinConcertoBerlinerPhilharmoniker;

    // Bach: Sonatas and Partitas for Solo Violin
    static AlbumPerson? _albumPersonBachSonatasPartitasViolinJohannSebastianBach;
    static AlbumPerson? _albumPersonBachSonatasPartitasViolinHenrykSzeryng;

    // Bach: Cello Sonatas
    static AlbumPerson? _albumPersonBachCelloSonatasJohannSebastianBach;
    static AlbumPerson? _albumPersonBachCelloSonatasMstislavRostropovich;

    // Bill Evans Trio at the Village Vanguard
    static AlbumPerson? _albumPersonBillEvansVillageVanguardBillEvans;
    static AlbumPerson? _albumPersonBillEvansVillageVanguardScottLaFaro;
    static AlbumPerson? _albumPersonBillEvansVillageVanguardPaulMotian;
    static AlbumPerson? _albumPersonBillEvansVillageVanguardOrrinKeepnews; // Producer

    static IEnumerable<AlbumPerson> _allAlbumPersons = [];

    internal static IEnumerable<AlbumPerson> NewAlbumPersons()
        => _allAlbumPersons = new AlbumPerson[]
        {
            // Kind of Blue
            _albumPersonKindOfBlueMilesDavis = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonMilesDavis, ["Performer", "Bandleader"], ["tp"]),
            _albumPersonKindOfBlueJohnColtrane = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonJohnColtrane, ["Performer"], ["ts"]),
            _albumPersonKindOfBlueJulianCannonballAdderley = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonJulianCannonballAdderley, ["Performer"], ["as"]),
            _albumPersonKindOfBlueBillEvans = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonBillEvans, ["Performer"], ["p"]),
            _albumPersonKindOfBlueWyntonKelly = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonWyntonKelly, ["Performer"], ["p"]),
            _albumPersonKindOfBluePaulChambers = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonPaulChambers, ["Performer"], ["b"]),
            _albumPersonKindOfBlueJimmyCobb = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonJimmyCobb, ["Performer"], ["dr"]),
            _albumPersonKindOfBlueIrvingTownsend = new AlbumPerson(Albums.AlbumKindOfBlue, Persons.PersonIrvingTownsend, ["Producer"], []),

            // Ellington at Newport
            _albumPersonEllingtonAtNewportDukeEllington = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonDukeEllington, ["Performer", "Bandleader"], ["p"]),
            _albumPersonEllingtonAtNewportPaulGonsalves = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonPaulGonsalves, ["Performer"], ["ts"]),
            _albumPersonEllingtonAtNewportJohnnyHodges = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonJohnnyHodges, ["Performer"], ["as"]),
            _albumPersonEllingtonAtNewportSamWoodyard = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonSamWoodyard, ["Performer"], ["dr"]),
            _albumPersonEllingtonAtNewportJimmyWoode = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonJimmyWoode, ["Performer"], ["b"]),
            _albumPersonEllingtonAtNewportGeorgeAvakian = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonGeorgeAvakian, ["Producer"], []),
            _albumPersonEllingtonAtNewportDukeEllingtonArranger = new AlbumPerson(Albums.AlbumEllingtonAtNewport, Persons.PersonDukeEllington, ["Arranger"], []),

            // Beethoven: Violin Concerto
            _albumPersonBeethovenViolinConcertoLudwigVanBeethoven = new AlbumPerson(Albums.AlbumBeethovenViolinConcerto, Persons.PersonLudwigVanBeethoven, ["Composer"], []),
            _albumPersonBeethovenViolinConcertoHenrykSzeryng = new AlbumPerson(Albums.AlbumBeethovenViolinConcerto, Persons.PersonHenrykSzeryng, ["Soloist"], ["v"]),
            _albumPersonBeethovenViolinConcertoBerlinerPhilharmoniker = new AlbumPerson(Albums.AlbumBeethovenViolinConcerto, Persons.PersonBerlinerPhilharmoniker, ["Orchestra"], []),

            // Bach: Sonatas and Partitas for Solo Violin
            _albumPersonBachSonatasPartitasViolinJohannSebastianBach = new AlbumPerson(Albums.AlbumBachSonatasPartitasViolin, Persons.PersonJohannSebastianBach, ["Composer"], []),
            _albumPersonBachSonatasPartitasViolinHenrykSzeryng = new AlbumPerson(Albums.AlbumBachSonatasPartitasViolin, Persons.PersonHenrykSzeryng, ["Soloist"], ["v"]),

            // Bach: Cello Sonatas
            _albumPersonBachCelloSonatasJohannSebastianBach = new AlbumPerson(Albums.AlbumBachCelloSonatas, Persons.PersonJohannSebastianBach, ["Composer"], []),
            _albumPersonBachCelloSonatasMstislavRostropovich = new AlbumPerson(Albums.AlbumBachCelloSonatas, Persons.PersonMstislavRostropovich, ["Soloist"], ["vc"]),

            // Bill Evans Trio at the Village Vanguard
            _albumPersonBillEvansVillageVanguardBillEvans = new AlbumPerson(Albums.AlbumBillEvansVillageVanguard, Persons.PersonBillEvans, ["Piano", "Bandleader"], ["p"]),
            _albumPersonBillEvansVillageVanguardScottLaFaro = new AlbumPerson(Albums.AlbumBillEvansVillageVanguard, Persons.PersonScottLaFaro, ["Bass"], ["b"]),
            _albumPersonBillEvansVillageVanguardPaulMotian = new AlbumPerson(Albums.AlbumBillEvansVillageVanguard, Persons.PersonPaulMotian, ["Drums"], ["dr"]),
            _albumPersonBillEvansVillageVanguardOrrinKeepnews = new AlbumPerson(Albums.AlbumBillEvansVillageVanguard, Persons.PersonOrrinKeepnews, ["Producer"], []),
        };

    public static IEnumerable<AlbumPerson> AlbumPersonsSequence => _allAlbumPersons.Any() ? _allAlbumPersons : NewAlbumPersons();

    // Kind of Blue
    public static AlbumPerson AlbumPersonKindOfBlueMilesDavis => _albumPersonKindOfBlueMilesDavis ?? (NewAlbumPersons(), _albumPersonKindOfBlueMilesDavis!).Item2;
    public static AlbumPerson AlbumPersonKindOfBlueJohnColtrane => _albumPersonKindOfBlueJohnColtrane ?? (NewAlbumPersons(), _albumPersonKindOfBlueJohnColtrane!).Item2;
    public static AlbumPerson AlbumPersonKindOfBlueJulianCannonballAdderley => _albumPersonKindOfBlueJulianCannonballAdderley ?? (NewAlbumPersons(), _albumPersonKindOfBlueJulianCannonballAdderley!).Item2;
    public static AlbumPerson AlbumPersonKindOfBlueBillEvans => _albumPersonKindOfBlueBillEvans ?? (NewAlbumPersons(), _albumPersonKindOfBlueBillEvans!).Item2;
    public static AlbumPerson AlbumPersonKindOfBlueWyntonKelly => _albumPersonKindOfBlueWyntonKelly ?? (NewAlbumPersons(), _albumPersonKindOfBlueWyntonKelly!).Item2;
    public static AlbumPerson AlbumPersonKindOfBluePaulChambers => _albumPersonKindOfBluePaulChambers ?? (NewAlbumPersons(), _albumPersonKindOfBluePaulChambers!).Item2;
    public static AlbumPerson AlbumPersonKindOfBlueJimmyCobb => _albumPersonKindOfBlueJimmyCobb ?? (NewAlbumPersons(), _albumPersonKindOfBlueJimmyCobb!).Item2;
    public static AlbumPerson AlbumPersonKindOfBlueIrvingTownsend => _albumPersonKindOfBlueIrvingTownsend ?? (NewAlbumPersons(), _albumPersonKindOfBlueIrvingTownsend!).Item2;

    // Ellington at Newport
    public static AlbumPerson AlbumPersonEllingtonAtNewportDukeEllington => _albumPersonEllingtonAtNewportDukeEllington ?? (NewAlbumPersons(), _albumPersonEllingtonAtNewportDukeEllington!).Item2;
    public static AlbumPerson AlbumPersonEllingtonAtNewportPaulGonsalves => _albumPersonEllingtonAtNewportPaulGonsalves ?? (NewAlbumPersons(), _albumPersonEllingtonAtNewportPaulGonsalves!).Item2;
    public static AlbumPerson AlbumPersonEllingtonAtNewportJohnnyHodges => _albumPersonEllingtonAtNewportJohnnyHodges ?? (NewAlbumPersons(), _albumPersonEllingtonAtNewportJohnnyHodges!).Item2;
    public static AlbumPerson AlbumPersonEllingtonAtNewportSamWoodyard => _albumPersonEllingtonAtNewportSamWoodyard ?? (NewAlbumPersons(), _albumPersonEllingtonAtNewportSamWoodyard!).Item2;
    public static AlbumPerson AlbumPersonEllingtonAtNewportJimmyWoode => _albumPersonEllingtonAtNewportJimmyWoode ?? (NewAlbumPersons(), _albumPersonEllingtonAtNewportJimmyWoode!).Item2;
    public static AlbumPerson AlbumPersonEllingtonAtNewportGeorgeAvakian => _albumPersonEllingtonAtNewportGeorgeAvakian ?? (NewAlbumPersons(), _albumPersonEllingtonAtNewportGeorgeAvakian!).Item2;
    public static AlbumPerson AlbumPersonEllingtonAtNewportDukeEllingtonArranger => _albumPersonEllingtonAtNewportDukeEllingtonArranger ?? (NewAlbumPersons(), _albumPersonEllingtonAtNewportDukeEllingtonArranger!).Item2;

    // Beethoven: Violin Concerto
    public static AlbumPerson AlbumPersonBeethovenViolinConcertoLudwigVanBeethoven => _albumPersonBeethovenViolinConcertoLudwigVanBeethoven ?? (NewAlbumPersons(), _albumPersonBeethovenViolinConcertoLudwigVanBeethoven!).Item2;
    public static AlbumPerson AlbumPersonBeethovenViolinConcertoHenrykSzeryng => _albumPersonBeethovenViolinConcertoHenrykSzeryng ?? (NewAlbumPersons(), _albumPersonBeethovenViolinConcertoHenrykSzeryng!).Item2;
    public static AlbumPerson AlbumPersonBeethovenViolinConcertoBerlinerPhilharmoniker => _albumPersonBeethovenViolinConcertoBerlinerPhilharmoniker ?? (NewAlbumPersons(), _albumPersonBeethovenViolinConcertoBerlinerPhilharmoniker!).Item2;

    // Bach: Sonatas and Partitas for Solo Violin
    public static AlbumPerson AlbumPersonBachSonatasPartitasViolinJohannSebastianBach => _albumPersonBachSonatasPartitasViolinJohannSebastianBach ?? (NewAlbumPersons(), _albumPersonBachSonatasPartitasViolinJohannSebastianBach!).Item2;
    public static AlbumPerson AlbumPersonBachSonatasPartitasViolinHenrykSzeryng => _albumPersonBachSonatasPartitasViolinHenrykSzeryng ?? (NewAlbumPersons(), _albumPersonBachSonatasPartitasViolinHenrykSzeryng!).Item2;

    // Bach: Cello Sonatas
    public static AlbumPerson AlbumPersonBachCelloSonatasJohannSebastianBach => _albumPersonBachCelloSonatasJohannSebastianBach ?? (NewAlbumPersons(), _albumPersonBachCelloSonatasJohannSebastianBach!).Item2;
    public static AlbumPerson AlbumPersonBachCelloSonatasMstislavRostropovich => _albumPersonBachCelloSonatasMstislavRostropovich ?? (NewAlbumPersons(), _albumPersonBachCelloSonatasMstislavRostropovich!).Item2;

    // Bill Evans Trio at the Village Vanguard
    public static AlbumPerson AlbumPersonBillEvansVillageVanguardBillEvans => _albumPersonBillEvansVillageVanguardBillEvans ?? (NewAlbumPersons(), _albumPersonBillEvansVillageVanguardBillEvans!).Item2;
    public static AlbumPerson AlbumPersonBillEvansVillageVanguardScottLaFaro => _albumPersonBillEvansVillageVanguardScottLaFaro ?? (NewAlbumPersons(), _albumPersonBillEvansVillageVanguardScottLaFaro!).Item2;
    public static AlbumPerson AlbumPersonBillEvansVillageVanguardPaulMotian => _albumPersonBillEvansVillageVanguardPaulMotian ?? (NewAlbumPersons(), _albumPersonBillEvansVillageVanguardPaulMotian!).Item2;
    public static AlbumPerson AlbumPersonBillEvansVillageVanguardOrrinKeepnews => _albumPersonBillEvansVillageVanguardOrrinKeepnews ?? (NewAlbumPersons(), _albumPersonBillEvansVillageVanguardOrrinKeepnews!).Item2;
}