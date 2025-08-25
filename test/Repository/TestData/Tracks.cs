namespace vm2.Repository.TestData;

public static class Tracks
{
    public static TrackId NextId => (TrackId)Ulid.NewUlid();

    // Track fields (one per track, named for clarity)
    static Track? _soWhat;
    static Track? _freddieFreeloader;
    static Track? _blueInGreen;
    static Track? _diminuendoCrescendoInBlue;
    static Track? _violinConcertoAllegro;
    static Track? _violinConcertoLarghetto;
    static Track? _celloSonata1Adagio;
    static Track? _celloSonata1Allegro;
    static Track? _celloSonata1Andante;
    static Track? _celloSonata1AllegroModerato;
    static Track? _celloSonata2Allegro;
    static Track? _celloSonata2Adagio;
    static Track? _celloSonata2Allegro2;
    static Track? _celloSonata3Vivace;
    static Track? _celloSonata3Adagio;
    static Track? _celloSonata3Allegro;
    static Track? _gloriasStep;
    static Track? _myMansGoneNow;
    static Track? _solar;
    static Track? _aliceInWonderland;
    static Track? _allOfYou;
    static Track? _jadeVisions;

    static IEnumerable<Track> _allTracks = [];

    internal static IEnumerable<Track> NewTracks()
    {
        #region The tracks
        // Album 1: Kind of Blue
        _soWhat = new(NextId, "So What", TimeSpan.FromSeconds(545), ["Jazz"], [
                new(PersonMilesDavis, "Miles Davis", ["Performer"], ["tp"]),
                new(PersonJohnColtrane, "John Coltrane", ["Performer"], ["ts"]),
                new(PersonJulianCannonballAdderley, "Julian 'Cannonball' Adderley", ["Performer"], ["as"]),
                new(PersonBillEvans, "Bill Evans", ["Performer"], ["p"]),
                new(PersonPaulChambers, "Paul Chambers", ["Performer"], ["b"]),
                new(PersonJimmyCobb, "Jimmy Cobb", ["Performer"], ["dr"]),
                new(PersonIrvingTownsend, "Irving Townsend", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _freddieFreeloader = new(NextId, "Freddie Freeloader", TimeSpan.FromSeconds(589), ["Jazz"], [
                new(PersonMilesDavis, "Miles Davis", ["Performer"], ["tp"]),
                new(PersonJohnColtrane, "John Coltrane", ["Performer"], ["ts"]),
                new(PersonWyntonKelly, "Wynton Kelly", ["Performer"], ["p"]),
                new(PersonPaulChambers, "Paul Chambers", ["Performer"], ["b"]),
                new(PersonJimmyCobb, "Jimmy Cobb", ["Performer"], ["dr"]),
                new(PersonIrvingTownsend, "Irving Townsend", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _blueInGreen = new(NextId, "Blue in Green", TimeSpan.FromSeconds(327), ["Jazz"], [
                new(PersonMilesDavis, "Miles Davis", ["Performer"], ["tp"]),
                new(PersonBillEvans, "Bill Evans", ["Performer"], ["p"]),
                new(PersonPaulChambers, "Paul Chambers", ["Performer"], ["b"]),
                new(PersonJimmyCobb, "Jimmy Cobb", ["Performer"], ["dr"]),
                new(PersonIrvingTownsend, "Irving Townsend", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        // Album 2: Ellington at Newport
        _diminuendoCrescendoInBlue = new(NextId, "Diminuendo and Crescendo in Blue", TimeSpan.FromSeconds(700), ["Jazz"], [
                new(PersonDukeEllington, "Duke Ellington", ["Performer", "Arranger"], ["p"]),
                new(PersonPaulGonsalves, "Paul Gonsalves", ["Performer"], ["ts"]),
                new(PersonJohnnyHodges, "Johnny Hodges", ["Performer"], ["as"]),
                new(PersonSamWoodyard, "Sam Woodyard", ["Performer"], ["dr"]),
                new(PersonJimmyWoode, "Jimmy Woode", ["Performer"], ["b"]),
                new(PersonGeorgeAvakian, "George Avakian", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        // Album 3: Beethoven: Violin Concerto
        _violinConcertoAllegro = new(NextId, "Violin Concerto in D major, Op. 61: I. Allegro ma non troppo", TimeSpan.FromSeconds(1420), ["Classical"], [
                new(PersonLudwigVanBeethoven, "Ludwig van Beethoven", ["Composer"], []),
                new(PersonHenrykSzeryng, "Henryk Szeryng", ["Soloist"], ["v"]),
                new(PersonBerlinerPhilharmoniker, "Berliner Philharmoniker", ["Orchestra"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _violinConcertoLarghetto = new(NextId, "Violin Concerto in D major, Op. 61: II. Larghetto", TimeSpan.FromSeconds(630), ["Classical"], [
                new(PersonLudwigVanBeethoven, "Ludwig van Beethoven", ["Composer"], []),
                new(PersonHenrykSzeryng, "Henryk Szeryng", ["Soloist"], ["v"]),
                new(PersonBerlinerPhilharmoniker, "Berliner Philharmoniker", ["Orchestra"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        // Album 5: Bach: Cello Sonatas
        _celloSonata1Adagio = new(NextId, "Cello Sonata No. 1 in G major, BWV 1027: I. Adagio", TimeSpan.FromSeconds(310), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _celloSonata1Allegro = new(NextId, "Cello Sonata No. 1 in G major, BWV 1027: II. Allegro ma non tanto", TimeSpan.FromSeconds(250), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _celloSonata1Andante = new(NextId, "Cello Sonata No. 1 in G major, BWV 1027: III. Andante", TimeSpan.FromSeconds(210), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _celloSonata1AllegroModerato = new(NextId, "Cello Sonata No. 1 in G major, BWV 1027: IV. Allegro moderato", TimeSpan.FromSeconds(205), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _celloSonata2Allegro = new(NextId, "Cello Sonata No. 2 in D major, BWV 1028: I. Allegro", TimeSpan.FromSeconds(274), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _celloSonata2Adagio = new(NextId, "Cello Sonata No. 2 in D major, BWV 1028: II. Adagio", TimeSpan.FromSeconds(235), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _celloSonata2Allegro2 = new(NextId, "Cello Sonata No. 2 in D major, BWV 1028: III. Allegro", TimeSpan.FromSeconds(240), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _celloSonata3Vivace = new(NextId, "Cello Sonata No. 3 in G minor, BWV 1029: I. Vivace", TimeSpan.FromSeconds(255), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _celloSonata3Adagio = new(NextId, "Cello Sonata No. 3 in G minor, BWV 1029: II. Adagio", TimeSpan.FromSeconds(215), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _celloSonata3Allegro = new(NextId, "Cello Sonata No. 3 in G minor, BWV 1029: III. Allegro", TimeSpan.FromSeconds(262), ["Classical"], [
                new(PersonJohannSebastianBach, "Johann Sebastian Bach", ["Composer"], []),
                new(PersonMstislavRostropovich, "Mstislav Rostropovich", ["Soloist"], ["vc"])
            ],
            NextDt, Creator, CurrentDt, Creator);

        // Album 7: Bill Evans Trio at the Village Vanguard
        _gloriasStep = new(NextId, "Gloria's Step", TimeSpan.FromSeconds(334), ["Jazz"], [
                new(PersonBillEvans, "Bill Evans", ["Piano", "Bandleader"], ["p"]),
                new(PersonScottLaFaro, "Scott LaFaro", ["Bass"], ["b"]),
                new(PersonPaulMotian, "Paul Motian", ["Drums"], ["dr"]),
                new(PersonOrrinKeepnews, "Orrin Keepnews", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _myMansGoneNow = new(NextId, "My Man's Gone Now", TimeSpan.FromSeconds(373), ["Jazz"], [
                new(PersonBillEvans, "Bill Evans", ["Piano", "Bandleader"], ["p"]),
                new(PersonScottLaFaro, "Scott LaFaro", ["Bass"], ["b"]),
                new(PersonPaulMotian, "Paul Motian", ["Drums"], ["dr"]),
                new(PersonOrrinKeepnews, "Orrin Keepnews", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _solar = new(NextId, "Solar", TimeSpan.FromSeconds(348), ["Jazz"], [
                new(PersonBillEvans, "Bill Evans", ["Piano", "Bandleader"], ["p"]),
                new(PersonScottLaFaro, "Scott LaFaro", ["Bass"], ["b"]),
                new(PersonPaulMotian, "Paul Motian", ["Drums"], ["dr"]),
                new(PersonOrrinKeepnews, "Orrin Keepnews", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _aliceInWonderland = new(NextId, "Alice in Wonderland", TimeSpan.FromSeconds(419), ["Jazz"], [
                new(PersonBillEvans, "Bill Evans", ["Piano", "Bandleader"], ["p"]),
                new(PersonScottLaFaro, "Scott LaFaro", ["Bass"], ["b"]),
                new(PersonPaulMotian, "Paul Motian", ["Drums"], ["dr"]),
                new(PersonOrrinKeepnews, "Orrin Keepnews", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _allOfYou = new(NextId, "All of You", TimeSpan.FromSeconds(491), ["Jazz"], [
                new(PersonBillEvans, "Bill Evans", ["Piano", "Bandleader"], ["p"]),
                new(PersonScottLaFaro, "Scott LaFaro", ["Bass"], ["b"]),
                new(PersonPaulMotian, "Paul Motian", ["Drums"], ["dr"]),
                new(PersonOrrinKeepnews, "Orrin Keepnews", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);

        _jadeVisions = new(NextId, "Jade Visions", TimeSpan.FromSeconds(232), ["Jazz"], [
                new(PersonBillEvans, "Bill Evans", ["Piano", "Bandleader"], ["p"]),
                new(PersonScottLaFaro, "Scott LaFaro", ["Bass"], ["b"]),
                new(PersonPaulMotian, "Paul Motian", ["Drums"], ["dr"]),
                new(PersonOrrinKeepnews, "Orrin Keepnews", ["Producer"], [])
            ],
            NextDt, Creator, CurrentDt, Creator);
        #endregion

        _allTracks = new[]
        {
            _soWhat,
            _freddieFreeloader,
            _blueInGreen,
            _diminuendoCrescendoInBlue,
            _violinConcertoAllegro,
            _violinConcertoLarghetto,
            _celloSonata1Adagio,
            _celloSonata1Allegro,
            _celloSonata1Andante,
            _celloSonata1AllegroModerato,
            _celloSonata2Allegro,
            _celloSonata2Adagio,
            _celloSonata2Allegro2,
            _celloSonata3Vivace,
            _celloSonata3Adagio,
            _celloSonata3Allegro,
            _gloriasStep,
            _myMansGoneNow,
            _solar,
            _aliceInWonderland,
            _allOfYou,
            _jadeVisions
        }.Where(t => t is not null)!;

        return _allTracks;
    }

    public static IEnumerable<Track> TracksSequence => _allTracks.Any() ? _allTracks : NewTracks();

    public static Track SoWhat => _soWhat ?? (NewTracks(), _soWhat!).Item2;
    public static Track FreddieFreeloader => _freddieFreeloader ?? (NewTracks(), _freddieFreeloader!).Item2;
    public static Track BlueInGreen => _blueInGreen ?? (NewTracks(), _blueInGreen!).Item2;
    public static Track DiminuendoCrescendoInBlue => _diminuendoCrescendoInBlue ?? (NewTracks(), _diminuendoCrescendoInBlue!).Item2;
    public static Track ViolinConcertoAllegro => _violinConcertoAllegro ?? (NewTracks(), _violinConcertoAllegro!).Item2;
    public static Track ViolinConcertoLarghetto => _violinConcertoLarghetto ?? (NewTracks(), _violinConcertoLarghetto!).Item2;
    public static Track CelloSonata1Adagio => _celloSonata1Adagio ?? (NewTracks(), _celloSonata1Adagio!).Item2;
    public static Track CelloSonata1Allegro => _celloSonata1Allegro ?? (NewTracks(), _celloSonata1Allegro!).Item2;
    public static Track CelloSonata1Andante => _celloSonata1Andante ?? (NewTracks(), _celloSonata1Andante!).Item2;
    public static Track CelloSonata1AllegroModerato => _celloSonata1AllegroModerato ?? (NewTracks(), _celloSonata1AllegroModerato!).Item2;
    public static Track CelloSonata2Allegro => _celloSonata2Allegro ?? (NewTracks(), _celloSonata2Allegro!).Item2;
    public static Track CelloSonata2Adagio => _celloSonata2Adagio ?? (NewTracks(), _celloSonata2Adagio!).Item2;
    public static Track CelloSonata2Allegro2 => _celloSonata2Allegro2 ?? (NewTracks(), _celloSonata2Allegro2!).Item2;
    public static Track CelloSonata3Vivace => _celloSonata3Vivace ?? (NewTracks(), _celloSonata3Vivace!).Item2;
    public static Track CelloSonata3Adagio => _celloSonata3Adagio ?? (NewTracks(), _celloSonata3Adagio!).Item2;
    public static Track CelloSonata3Allegro => _celloSonata3Allegro ?? (NewTracks(), _celloSonata3Allegro!).Item2;
    public static Track GloriasStep => _gloriasStep ?? (NewTracks(), _gloriasStep!).Item2;
    public static Track MyMansGoneNow => _myMansGoneNow ?? (NewTracks(), _myMansGoneNow!).Item2;
    public static Track Solar => _solar ?? (NewTracks(), _solar!).Item2;
    public static Track AliceInWonderland => _aliceInWonderland ?? (NewTracks(), _aliceInWonderland!).Item2;
    public static Track AllOfYou => _allOfYou ?? (NewTracks(), _allOfYou!).Item2;
    public static Track JadeVisions => _jadeVisions ?? (NewTracks(), _jadeVisions!).Item2;
}