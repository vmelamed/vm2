namespace vm2.Repository.TestData;

public static class Persons
{
    public static PersonId NextId => (PersonId)UlidFactory.NewUlid();

    static Person? _personMilesDavis;
    static Person? _personJohnColtrane;
    static Person? _personBillEvans;
    static Person? _personWyntonKelly;
    static Person? _personJulianCannonballAdderley;
    static Person? _personPaulChambers;
    static Person? _personJimmyCobb;
    static Person? _personLeeMorgan;
    static Person? _personCurtisFuller;
    static Person? _personKennyDrew;
    static Person? _personPhillyJoeJones;
    static Person? _personHerbieHancock;
    static Person? _personFreddieHubbard;
    static Person? _personRonCarter;
    static Person? _personTonyWilliams;
    static Person? _personGeorgeColeman;
    static Person? _personMcCoyTyner;
    static Person? _personElvinJones;
    static Person? _personJimmyGarrison;
    static Person? _personDukeEllington;
    static Person? _personPaulGonsalves;
    static Person? _personJohnnyHodges;
    static Person? _personSamWoodyard;
    static Person? _personJimmyWoode;
    static Person? _personEllaFitzgerald;
    static Person? _personLouisArmstrong;
    static Person? _personOscarPeterson;
    static Person? _personHerbEllis;
    static Person? _personRayBrown;
    static Person? _personBuddyRich;
    static Person? _personLudwigVanBeethoven;
    static Person? _personWolfgangAmadeusMozart;
    static Person? _personJohannSebastianBach;
    static Person? _personGlennGould;
    static Person? _personHilaryHahn;
    static Person? _personHenrykSzeryng;
    static Person? _personMstislavRostropovich;
    static Person? _personBerlinerPhilharmoniker;
    static Person? _personWienerPhilharmoniker;
    static Person? _personLondonSymphonyOrchestra;
    static Person? _personScottLaFaro;
    static Person? _personPaulMotian;
    // Producers and Arrangers
    static Person? _personIrvingTownsend;
    static Person? _personGeorgeAvakian;
    static Person? _personOrrinKeepnews;

    static IEnumerable<Person> _allPersons = [];

    internal static IEnumerable<Person> NewPersons()
        => _allPersons = new Person[]
        {
            _personMilesDavis               = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Miles Davis",                  birthYear: 1926, deathYear: 1991, roles: ["Performer", "Composer", "Bandleader"], instruments: ["tp"],      genres: ["Jazz"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personJohnColtrane             = new Person(id: NextId, tenantId: TestTenant.Current(), name: "John Coltrane",                birthYear: 1926, deathYear: 1967, roles: ["Performer", "Composer", "Bandleader"], instruments: ["ts", "as", "ss"], genres: ["Jazz"],      createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personBillEvans                = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Bill Evans",                   birthYear: 1929, deathYear: 1980, roles: ["Performer", "Composer", "Bandleader"], instruments: ["p"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personWyntonKelly              = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Wynton Kelly",                 birthYear: 1931, deathYear: 1971, roles: ["Performer"],                           instruments: ["p"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personJulianCannonballAdderley = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Julian 'Cannonball' Adderley", birthYear: 1928, deathYear: 1975, roles: ["Performer", "Composer", "Bandleader"], instruments: ["as"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personPaulChambers             = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Paul Chambers",                birthYear: 1935, deathYear: 1969, roles: ["Performer"],                           instruments: ["b"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personJimmyCobb                = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Jimmy Cobb",                   birthYear: 1929, deathYear: 2020, roles: ["Performer"],                           instruments: ["dr"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personLeeMorgan                = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Lee Morgan",                   birthYear: 1938, deathYear: 1972, roles: ["Performer", "Composer", "Bandleader"], instruments: ["tp"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personCurtisFuller             = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Curtis Fuller",                birthYear: 1934, deathYear: 2021, roles: ["Performer", "Composer", "Bandleader"], instruments: ["tb"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personKennyDrew                = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Kenny Drew",                   birthYear: 1928, deathYear: 1993, roles: ["Performer"],                           instruments: ["p"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personPhillyJoeJones           = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Philly Joe Jones",             birthYear: 1923, deathYear: 1985, roles: ["Performer"],                           instruments: ["dr"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personHerbieHancock            = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Herbie Hancock",               birthYear: 1940, deathYear: null, roles: ["Performer", "Composer", "Bandleader"], instruments: ["p"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personFreddieHubbard           = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Freddie Hubbard",              birthYear: 1938, deathYear: 2008, roles: ["Performer", "Composer", "Bandleader"], instruments: ["tp"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personRonCarter                = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Ron Carter",                   birthYear: 1937, deathYear: null, roles: ["Performer", "Composer", "Bandleader"], instruments: ["b"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personTonyWilliams             = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Tony Williams",                birthYear: 1945, deathYear: 1997, roles: ["Performer"],                           instruments: ["dr"],      genres: ["Jazz"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personGeorgeColeman            = new Person(id: NextId, tenantId: TestTenant.Current(), name: "George Coleman",               birthYear: 1935, deathYear: null, roles: ["Performer"],                           instruments: ["ts", "as"], genres: ["Jazz"],            createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personMcCoyTyner               = new Person(id: NextId, tenantId: TestTenant.Current(), name: "McCoy Tyner",                  birthYear: 1938, deathYear: 2020, roles: ["Performer", "Composer", "Bandleader"], instruments: ["p"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personElvinJones               = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Elvin Jones",                  birthYear: 1927, deathYear: 2004, roles: ["Performer", "Composer", "Bandleader"], instruments: ["dr"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personJimmyGarrison            = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Jimmy Garrison",               birthYear: 1934, deathYear: 1976, roles: ["Performer"],                           instruments: ["b"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personDukeEllington            = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Duke Ellington",               birthYear: 1899, deathYear: 1974, roles: ["Performer", "Composer", "Bandleader", "Arranger"], instruments: ["p"], genres: ["Jazz"],       createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personPaulGonsalves            = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Paul Gonsalves",               birthYear: 1920, deathYear: 1974, roles: ["Performer", "Composer"],               instruments: ["ts"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personJohnnyHodges             = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Johnny Hodges",                birthYear: 1907, deathYear: 1970, roles: ["Performer", "Composer", "Bandleader"], instruments: ["as"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personSamWoodyard              = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Sam Woodyard",                 birthYear: 1925, deathYear: 1988, roles: ["Performer"],                           instruments: ["dr"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personJimmyWoode               = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Jimmy Woode",                  birthYear: 1926, deathYear: 2005, roles: ["Performer"],                           instruments: ["b"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personEllaFitzgerald           = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Ella Fitzgerald",              birthYear: 1917, deathYear: 1996, roles: ["Performer"],                           instruments: ["voc"],    genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personLouisArmstrong           = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Louis Armstrong",              birthYear: 1901, deathYear: 1971, roles: ["Performer", "Composer", "Bandleader"], instruments: ["tp", "voc"], genres: ["Jazz"],           createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personOscarPeterson            = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Oscar Peterson",               birthYear: 1925, deathYear: 2007, roles: ["Performer", "Composer", "Bandleader"], instruments: ["p"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personHerbEllis                = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Herb Ellis",                   birthYear: 1921, deathYear: 2010, roles: ["Performer", "Composer", "Bandleader"], instruments: ["g"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personRayBrown                 = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Ray Brown",                    birthYear: 1926, deathYear: 2002, roles: ["Performer"],                           instruments: ["b"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personBuddyRich                = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Buddy Rich",                   birthYear: 1917, deathYear: 1987, roles: ["Performer"],                           instruments: ["dr"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personLudwigVanBeethoven       = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Ludwig van Beethoven",         birthYear: 1770, deathYear: 1827, roles: ["Composer"],                            instruments: [],         genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personWolfgangAmadeusMozart    = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Wolfgang Amadeus Mozart",      birthYear: 1756, deathYear: 1791, roles: ["Composer"],                            instruments: [],         genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personJohannSebastianBach      = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Johann Sebastian Bach",        birthYear: 1685, deathYear: 1750, roles: ["Composer"],                            instruments: [],         genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personGlennGould               = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Glenn Gould",                  birthYear: 1932, deathYear: 1982, roles: ["Performer"],                           instruments: ["p"],      genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personHilaryHahn               = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Hilary Hahn",                  birthYear: 1979, deathYear: null, roles: ["Performer"],                           instruments: ["v"],      genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personHenrykSzeryng            = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Henryk Szeryng",               birthYear: 1918, deathYear: 1988, roles: ["Performer"],                           instruments: ["v"],      genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personMstislavRostropovich     = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Mstislav Rostropovich",        birthYear: 1927, deathYear: 2007, roles: ["Performer"],                           instruments: ["vc"],     genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personBerlinerPhilharmoniker   = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Berliner Philharmoniker",      birthYear: 1882, deathYear: null, roles: ["Orchestra"],                           instruments: [],         genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personWienerPhilharmoniker     = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Wiener Philharmoniker",        birthYear: 1843, deathYear: null, roles: ["Orchestra"],                           instruments: [],         genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personLondonSymphonyOrchestra  = new Person(id: NextId, tenantId: TestTenant.Current(), name: "London Symphony Orchestra",    birthYear: 1904, deathYear: null, roles: ["Orchestra"],                           instruments: [],         genres: ["Classical"],         createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personScottLaFaro              = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Scott LaFaro",                 birthYear: 1936, deathYear: 1961, roles: ["Performer"],                           instruments: ["b"],      genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personPaulMotian               = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Paul Motian",                  birthYear: 1931, deathYear: 2011, roles: ["Performer"],                           instruments: ["dr"],     genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            // Producers and Arrangers
            _personIrvingTownsend           = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Irving Townsend",              birthYear: 1920, deathYear: 1981, roles: ["Producer"],                            instruments: [],         genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personGeorgeAvakian            = new Person(id: NextId, tenantId: TestTenant.Current(), name: "George Avakian",               birthYear: 1919, deathYear: 2017, roles: ["Producer"],                            instruments: [],         genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _personOrrinKeepnews            = new Person(id: NextId, tenantId: TestTenant.Current(), name: "Orrin Keepnews",               birthYear: 1923, deathYear: 2015, roles: ["Producer"],                            instruments: [],         genres: ["Jazz"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
        };

    public static IEnumerable<Person> PersonsSequence => _allPersons.Any() ? _allPersons : NewPersons();

    public static Person PersonMilesDavis => _personMilesDavis ?? (NewPersons(), _personMilesDavis!).Item2;
    public static Person PersonJohnColtrane => _personJohnColtrane ?? (NewPersons(), _personJohnColtrane!).Item2;
    public static Person PersonBillEvans => _personBillEvans ?? (NewPersons(), _personBillEvans!).Item2;
    public static Person PersonWyntonKelly => _personWyntonKelly ?? (NewPersons(), _personWyntonKelly!).Item2;
    public static Person PersonJulianCannonballAdderley => _personJulianCannonballAdderley ?? (NewPersons(), _personJulianCannonballAdderley!).Item2;
    public static Person PersonPaulChambers => _personPaulChambers ?? (NewPersons(), _personPaulChambers!).Item2;
    public static Person PersonJimmyCobb => _personJimmyCobb ?? (NewPersons(), _personJimmyCobb!).Item2;
    public static Person PersonLeeMorgan => _personLeeMorgan ?? (NewPersons(), _personLeeMorgan!).Item2;
    public static Person PersonCurtisFuller => _personCurtisFuller ?? (NewPersons(), _personCurtisFuller!).Item2;
    public static Person PersonKennyDrew => _personKennyDrew ?? (NewPersons(), _personKennyDrew!).Item2;
    public static Person PersonPhillyJoeJones => _personPhillyJoeJones ?? (NewPersons(), _personPhillyJoeJones!).Item2;
    public static Person PersonHerbieHancock => _personHerbieHancock ?? (NewPersons(), _personHerbieHancock!).Item2;
    public static Person PersonFreddieHubbard => _personFreddieHubbard ?? (NewPersons(), _personFreddieHubbard!).Item2;
    public static Person PersonRonCarter => _personRonCarter ?? (NewPersons(), _personRonCarter!).Item2;
    public static Person PersonTonyWilliams => _personTonyWilliams ?? (NewPersons(), _personTonyWilliams!).Item2;
    public static Person PersonGeorgeColeman => _personGeorgeColeman ?? (NewPersons(), _personGeorgeColeman!).Item2;
    public static Person PersonMcCoyTyner => _personMcCoyTyner ?? (NewPersons(), _personMcCoyTyner!).Item2;
    public static Person PersonElvinJones => _personElvinJones ?? (NewPersons(), _personElvinJones!).Item2;
    public static Person PersonJimmyGarrison => _personJimmyGarrison ?? (NewPersons(), _personJimmyGarrison!).Item2;
    public static Person PersonDukeEllington => _personDukeEllington ?? (NewPersons(), _personDukeEllington!).Item2;
    public static Person PersonPaulGonsalves => _personPaulGonsalves ?? (NewPersons(), _personPaulGonsalves!).Item2;
    public static Person PersonJohnnyHodges => _personJohnnyHodges ?? (NewPersons(), _personJohnnyHodges!).Item2;
    public static Person PersonSamWoodyard => _personSamWoodyard ?? (NewPersons(), _personSamWoodyard!).Item2;
    public static Person PersonJimmyWoode => _personJimmyWoode ?? (NewPersons(), _personJimmyWoode!).Item2;
    public static Person PersonEllaFitzgerald => _personEllaFitzgerald ?? (NewPersons(), _personEllaFitzgerald!).Item2;
    public static Person PersonLouisArmstrong => _personLouisArmstrong ?? (NewPersons(), _personLouisArmstrong!).Item2;
    public static Person PersonOscarPeterson => _personOscarPeterson ?? (NewPersons(), _personOscarPeterson!).Item2;
    public static Person PersonHerbEllis => _personHerbEllis ?? (NewPersons(), _personHerbEllis!).Item2;
    public static Person PersonRayBrown => _personRayBrown ?? (NewPersons(), _personRayBrown!).Item2;
    public static Person PersonBuddyRich => _personBuddyRich ?? (NewPersons(), _personBuddyRich!).Item2;
    public static Person PersonLudwigVanBeethoven => _personLudwigVanBeethoven ?? (NewPersons(), _personLudwigVanBeethoven!).Item2;
    public static Person PersonWolfgangAmadeusMozart => _personWolfgangAmadeusMozart ?? (NewPersons(), _personWolfgangAmadeusMozart!).Item2;
    public static Person PersonJohannSebastianBach => _personJohannSebastianBach ?? (NewPersons(), _personJohannSebastianBach!).Item2;
    public static Person PersonGlennGould => _personGlennGould ?? (NewPersons(), _personGlennGould!).Item2;
    public static Person PersonHilaryHahn => _personHilaryHahn ?? (NewPersons(), _personHilaryHahn!).Item2;
    public static Person PersonHenrykSzeryng => _personHenrykSzeryng ?? (NewPersons(), _personHenrykSzeryng!).Item2;
    public static Person PersonMstislavRostropovich => _personMstislavRostropovich ?? (NewPersons(), _personMstislavRostropovich!).Item2;
    public static Person PersonBerlinerPhilharmoniker => _personBerlinerPhilharmoniker ?? (NewPersons(), _personBerlinerPhilharmoniker!).Item2;
    public static Person PersonWienerPhilharmoniker => _personWienerPhilharmoniker ?? (NewPersons(), _personWienerPhilharmoniker!).Item2;
    public static Person PersonLondonSymphonyOrchestra => _personLondonSymphonyOrchestra ?? (NewPersons(), _personLondonSymphonyOrchestra!).Item2;
    public static Person PersonScottLaFaro => _personScottLaFaro ?? (NewPersons(), _personScottLaFaro!).Item2;
    public static Person PersonPaulMotian => _personPaulMotian ?? (NewPersons(), _personPaulMotian!).Item2;
    // Producers and Arrangers
    public static Person PersonIrvingTownsend => _personIrvingTownsend ?? (NewPersons(), _personIrvingTownsend!).Item2;
    public static Person PersonGeorgeAvakian => _personGeorgeAvakian ?? (NewPersons(), _personGeorgeAvakian!).Item2;
    public static Person PersonOrrinKeepnews => _personOrrinKeepnews ?? (NewPersons(), _personOrrinKeepnews!).Item2;
}

