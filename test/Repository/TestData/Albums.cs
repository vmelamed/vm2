namespace vm2.Repository.TestData;

using vm2.TestUtilities;

public static class Albums
{
    public static AlbumId NextId => new(UlidFactory.NewUlid());

    internal static IEnumerable<Album> NewAlbums()
        => AlbumsSequence = [
            AlbumKindOfBlue = new(
                id: NextId,
                tenantId: TestTenant.Current(),
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
            AlbumEllingtonAtNewport = new(
                id: NextId,
                tenantId: TestTenant.Current(),
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
            AlbumBeethovenViolinConcerto = new(
                id: NextId,
                tenantId: TestTenant.Current(),
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
            AlbumBachSonatasPartitasViolin = new(
                id: NextId,
                tenantId: TestTenant.Current(),
                title: "Bach: Sonatas and Partitas for Solo Violin",
                releaseYear: 1971,
                genres: ["Classical"],
                tracks: [],
                createdAt: NextDt,
                createdBy: Creator,
                updatedAt: CurrentDt,
                updatedBy: Creator
            ),
            AlbumBachCelloSonatas = new(
                id: NextId,
                tenantId: TestTenant.Current(),
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
            AlbumBillEvansVillageVanguard = new(
                id: NextId,
                tenantId: TestTenant.Current(),
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

    public static IEnumerable<Album> AlbumsSequence { get => field.Any() ? field : NewAlbums(); private set; } = [];

    public static Album AlbumKindOfBlue { get => field ?? (NewAlbums(), field!).Item2; private set; }
    public static Album AlbumEllingtonAtNewport { get => field ?? (NewAlbums(), field!).Item2; private set; }
    public static Album AlbumBeethovenViolinConcerto { get => field ?? (NewAlbums(), field!).Item2; private set; }
    public static Album AlbumBachSonatasPartitasViolin { get => field ?? (NewAlbums(), field!).Item2; private set; }
    public static Album AlbumBachCelloSonatas { get => field ?? (NewAlbums(), field!).Item2; private set; }
    public static Album AlbumBillEvansVillageVanguard { get => field ?? (NewAlbums(), field!).Item2; private set; }
}