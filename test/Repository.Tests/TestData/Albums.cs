namespace vm2.Repository.Tests.TestData;

public static class Albums
{
    static int id = 0;

    public static int NextId => Interlocked.Increment(ref id);

    static Album? _albumKindOfBlue;
    static Album? _albumEllingtonAtNewport;
    static Album? _albumBeethovenViolinConcerto;
    static Album? _albumBachSonatasPartitasViolin;
    static Album? _albumBachCelloSonatas;
    static Album? _albumBillEvansVillageVanguard;

    static IEnumerable<Album> _allAlbums = [];

    internal static IEnumerable<Album> NewAlbums()
        => _allAlbums = [
            _albumKindOfBlue = new(
                id: NextId,
                title: "Kind of Blue",
                releaseYear: 1959,
                genres: ["Jazz"],
                tracks: [
                    new(SoWhat),
                    new(FreddieFreeloader),
                    new(BlueInGreen)
                ],
                createdAt: NextDt,
                createdBy: Creator,
                updatedAt: CurrentDt,
                updatedBy: Creator
            ),
            _albumEllingtonAtNewport = new(
                id: NextId,
                title: "Ellington at Newport",
                releaseYear: 1956,
                genres: ["Jazz"],
                tracks: [
                    new(DiminuendoCrescendoInBlue)
                ],
                createdAt: NextDt,
                createdBy: Creator,
                updatedAt: CurrentDt,
                updatedBy: Creator
            ),
            _albumBeethovenViolinConcerto = new(
                id: NextId,
                title: "Beethoven: Violin Concerto",
                releaseYear: 1962,
                genres: ["Classical"],
                tracks: [
                    new(ViolinConcertoAllegro),
                    new(ViolinConcertoLarghetto)
                ],
                createdAt: NextDt,
                createdBy: Creator,
                updatedAt: CurrentDt,
                updatedBy: Creator
            ),
            _albumBachSonatasPartitasViolin = new(
                id: NextId,
                title: "Bach: Sonatas and Partitas for Solo Violin",
                releaseYear: 1971,
                genres: ["Classical"],
                tracks: [],
                createdAt: NextDt,
                createdBy: Creator,
                updatedAt: CurrentDt,
                updatedBy: Creator
            ),
            _albumBachCelloSonatas = new(
                id: NextId,
                title: "Bach: Cello Sonatas",
                releaseYear: 1979,
                genres: ["Classical"],
                tracks: [
                    new(CelloSonata1Adagio),
                    new(CelloSonata1Allegro),
                    new(CelloSonata1Andante),
                    new(CelloSonata1AllegroModerato),
                    new(CelloSonata2Allegro),
                    new(CelloSonata2Adagio),
                    new(CelloSonata2Allegro2),
                    new(CelloSonata3Vivace),
                    new(CelloSonata3Adagio),
                    new(CelloSonata3Allegro)
                ],
                createdAt: NextDt,
                createdBy: Creator,
                updatedAt: CurrentDt,
                updatedBy: Creator
            ),
            _albumBillEvansVillageVanguard = new(
                id: NextId,
                title: "Bill Evans Trio at the Village Vanguard",
                releaseYear: 1961,
                genres: ["Jazz"],
                tracks: [
                    new(GloriasStep),
                    new(MyMansGoneNow),
                    new(Solar),
                    new(AliceInWonderland),
                    new(AllOfYou),
                    new(JadeVisions)
                ],
                createdAt: NextDt,
                createdBy: Creator,
                updatedAt: CurrentDt,
                updatedBy: Creator
            )
        ];

    public static IEnumerable<Album> AlbumsSequence => _allAlbums.Any() ? _allAlbums : NewAlbums();

    public static Album AlbumKindOfBlue => _albumKindOfBlue ?? (NewAlbums(), _albumKindOfBlue!).Item2;
    public static Album AlbumEllingtonAtNewport => _albumEllingtonAtNewport ?? (NewAlbums(), _albumEllingtonAtNewport!).Item2;
    public static Album AlbumBeethovenViolinConcerto => _albumBeethovenViolinConcerto ?? (NewAlbums(), _albumBeethovenViolinConcerto!).Item2;
    public static Album AlbumBachSonatasPartitasViolin => _albumBachSonatasPartitasViolin ?? (NewAlbums(), _albumBachSonatasPartitasViolin!).Item2;
    public static Album AlbumBachCelloSonatas => _albumBachCelloSonatas ?? (NewAlbums(), _albumBachCelloSonatas!).Item2;
    public static Album AlbumBillEvansVillageVanguard => _albumBillEvansVillageVanguard ?? (NewAlbums(), _albumBillEvansVillageVanguard!).Item2;
}