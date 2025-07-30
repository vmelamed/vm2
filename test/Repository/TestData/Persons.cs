using vm2.Repository.Abstractions.Model;

namespace vm2.Repository.TestData;

public static class Persons
{
    static int id = 0;

    public static int NextId => Interlocked.Increment(ref id);

    static Person? _person01;
    static Person? _person02;
    static Person? _person03;
    static Person? _person04;
    static Person? _person05;
    static Person? _person06;
    static Person? _person07;
    static Person? _person08;
    static Person? _person09;
    static Person? _person10;
    static Person? _person11;
    static Person? _person12;
    static Person? _person13;
    static Person? _person14;
    static Person? _person15;
    static Person? _person16;
    static Person? _person17;
    static Person? _person18;
    static Person? _person19;
    static Person? _person20;
    static Person? _person21;
    static Person? _person22;
    static Person? _person23;
    static Person? _person24;
    static Person? _person25;
    static Person? _person26;
    static Person? _person27;
    static Person? _person28;
    static Person? _person29;
    static Person? _person30;
    static Person? _person31;
    static Person? _person32;
    static Person? _person33;
    static Person? _person34;
    static Person? _person35;
    static Person? _person36;
    static Person? _person37;
    static Person? _person38;
    static Person? _person39;
    static Person? _person40;
    static Person? _person41;
    static Person? _person42;

    static IEnumerable<Person> _allPersons = [];

    internal static IEnumerable<Person> NewPersons()
        => _allPersons = new Person[]
        {
            _person01 = new Person(id: NextId, name: "Miles Davis",                  birthYear: 1926, deathYear: 1991, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["tp"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person02 = new Person(id: NextId, name: "John Coltrane",                birthYear: 1926, deathYear: 1967, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["ts", "as", "ss"], createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person03 = new Person(id: NextId, name: "Bill Evans",                   birthYear: 1929, deathYear: 1980, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["p"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person04 = new Person(id: NextId, name: "Wynton Kelly",                 birthYear: 1931, deathYear: 1971, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["p"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person05 = new Person(id: NextId, name: "Julian 'Cannonball' Adderley", birthYear: 1928, deathYear: 1975, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["as"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person06 = new Person(id: NextId, name: "Paul Chambers",                birthYear: 1935, deathYear: 1969, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["b"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person07 = new Person(id: NextId, name: "Jimmy Cobb",                   birthYear: 1929, deathYear: 2020, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["dr"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person08 = new Person(id: NextId, name: "Lee Morgan",                   birthYear: 1938, deathYear: 1972, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["tp"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person09 = new Person(id: NextId, name: "Curtis Fuller",                birthYear: 1934, deathYear: 2021, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["tb"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person10 = new Person(id: NextId, name: "Kenny Drew",                   birthYear: 1928, deathYear: 1993, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["p"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person11 = new Person(id: NextId, name: "Philly Joe Jones",             birthYear: 1923, deathYear: 1985, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["dr"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person12 = new Person(id: NextId, name: "Herbie Hancock",               birthYear: 1940, deathYear: null, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["p"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person13 = new Person(id: NextId, name: "Freddie Hubbard",              birthYear: 1938, deathYear: 2008, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["tp"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person14 = new Person(id: NextId, name: "Ron Carter",                   birthYear: 1937, deathYear: null, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["b"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person15 = new Person(id: NextId, name: "Tony Williams",                birthYear: 1945, deathYear: 1997, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["dr"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person16 = new Person(id: NextId, name: "George Coleman",               birthYear: 1935, deathYear: null, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["ts", "as"],       createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person17 = new Person(id: NextId, name: "McCoy Tyner",                  birthYear: 1938, deathYear: 2020, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["p"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person18 = new Person(id: NextId, name: "Elvin Jones",                  birthYear: 1927, deathYear: 2004, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["dr"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person19 = new Person(id: NextId, name: "Jimmy Garrison",               birthYear: 1934, deathYear: 1976, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["b"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person20 = new Person(id: NextId, name: "Duke Ellington",               birthYear: 1899, deathYear: 1974, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["p"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person21 = new Person(id: NextId, name: "Paul Gonsalves",               birthYear: 1920, deathYear: 1974, roles: ["Performer", "Composer"],               genres: ["Jazz"],      instrumentCodes: ["ts"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person22 = new Person(id: NextId, name: "Johnny Hodges",                birthYear: 1907, deathYear: 1970, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["as"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person23 = new Person(id: NextId, name: "Sam Woodyard",                 birthYear: 1925, deathYear: 1988, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["dr"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person24 = new Person(id: NextId, name: "Jimmy Woode",                  birthYear: 1926, deathYear: 2005, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["b"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person25 = new Person(id: NextId, name: "Ella Fitzgerald",              birthYear: 1917, deathYear: 1996, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["voc"],            createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person26 = new Person(id: NextId, name: "Louis Armstrong",              birthYear: 1901, deathYear: 1971, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["tp", "voc"],      createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person27 = new Person(id: NextId, name: "Oscar Peterson",               birthYear: 1925, deathYear: 2007, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["p"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person28 = new Person(id: NextId, name: "Herb Ellis",                   birthYear: 1921, deathYear: 2010, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["g"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person29 = new Person(id: NextId, name: "Ray Brown",                    birthYear: 1926, deathYear: 2002, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["b"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person30 = new Person(id: NextId, name: "Buddy Rich",                   birthYear: 1917, deathYear: 1987, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["dr"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person31 = new Person(id: NextId, name: "Ludwig van Beethoven",         birthYear: 1770, deathYear: 1827, roles: ["Composer"],                            genres: ["Classical"], instrumentCodes: [],                 createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person32 = new Person(id: NextId, name: "Wolfgang Amadeus Mozart",      birthYear: 1756, deathYear: 1791, roles: ["Composer"],                            genres: ["Classical"], instrumentCodes: [],                 createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person33 = new Person(id: NextId, name: "Johann Sebastian Bach",        birthYear: 1685, deathYear: 1750, roles: ["Composer"],                            genres: ["Classical"], instrumentCodes: [],                 createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person34 = new Person(id: NextId, name: "Glenn Gould",                  birthYear: 1932, deathYear: 1982, roles: ["Performer"],                           genres: ["Classical"], instrumentCodes: ["p"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person35 = new Person(id: NextId, name: "Hilary Hahn",                  birthYear: 1979, deathYear: null, roles: ["Performer"],                           genres: ["Classical"], instrumentCodes: ["v"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person36 = new Person(id: NextId, name: "Henryk Szeryng",               birthYear: 1918, deathYear: 1988, roles: ["Performer"],                           genres: ["Classical"], instrumentCodes: ["v"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person37 = new Person(id: NextId, name: "Mstislav Rostropovich",        birthYear: 1927, deathYear: 2007, roles: ["Performer"],                           genres: ["Classical"], instrumentCodes: ["vc"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person38 = new Person(id: NextId, name: "Berliner Philharmoniker",      birthYear: 1882, deathYear: null, roles: ["Orchestra"],                           genres: ["Classical"], instrumentCodes: [],                 createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person39 = new Person(id: NextId, name: "Wiener Philharmoniker",        birthYear: 1843, deathYear: null, roles: ["Orchestra"],                           genres: ["Classical"], instrumentCodes: [],                 createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person40 = new Person(id: NextId, name: "London Symphony Orchestra",    birthYear: 1904, deathYear: null, roles: ["Orchestra"],                           genres: ["Classical"], instrumentCodes: [],                 createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person41 = new Person(id: NextId, name: "Scott LaFaro",                 birthYear: 1936, deathYear: 1961, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["b"],              createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person42 = new Person(id: NextId, name: "Paul Motian",                  birthYear: 1931, deathYear: 2011, roles: ["Performer"],                           genres: ["Jazz"],      instrumentCodes: ["dr"],             createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
            _person42 = new Person(id: NextId, name: "Wayne Shorter",                birthYear: 1933, deathYear: 2023, roles: ["Performer", "Composer", "Bandleader"], genres: ["Jazz"],      instrumentCodes: ["ts", "ss"],       createdAt: NextDt, createdBy: Creator, updatedAt: CurrentDt, updatedBy: Creator),
        };

    public static IEnumerable<Person> PersonsSequence => _allPersons.Any() ? _allPersons : NewPersons();

    public static Person Person01 => _person01 ?? (NewPersons(), _person01!).Item2;
    public static Person Person02 => _person02 ?? (NewPersons(), _person02!).Item2;
    public static Person Person03 => _person03 ?? (NewPersons(), _person03!).Item2;
    public static Person Person04 => _person04 ?? (NewPersons(), _person04!).Item2;
    public static Person Person05 => _person05 ?? (NewPersons(), _person05!).Item2;
    public static Person Person06 => _person06 ?? (NewPersons(), _person06!).Item2;
    public static Person Person07 => _person07 ?? (NewPersons(), _person07!).Item2;
    public static Person Person08 => _person08 ?? (NewPersons(), _person08!).Item2;
    public static Person Person09 => _person09 ?? (NewPersons(), _person09!).Item2;
    public static Person Person10 => _person10 ?? (NewPersons(), _person10!).Item2;
    public static Person Person11 => _person11 ?? (NewPersons(), _person11!).Item2;
    public static Person Person12 => _person12 ?? (NewPersons(), _person12!).Item2;
    public static Person Person13 => _person13 ?? (NewPersons(), _person13!).Item2;
    public static Person Person14 => _person14 ?? (NewPersons(), _person14!).Item2;
    public static Person Person15 => _person15 ?? (NewPersons(), _person15!).Item2;
    public static Person Person16 => _person16 ?? (NewPersons(), _person16!).Item2;
    public static Person Person17 => _person17 ?? (NewPersons(), _person17!).Item2;
    public static Person Person18 => _person18 ?? (NewPersons(), _person18!).Item2;
    public static Person Person19 => _person19 ?? (NewPersons(), _person19!).Item2;
    public static Person Person20 => _person20 ?? (NewPersons(), _person20!).Item2;
    public static Person Person21 => _person21 ?? (NewPersons(), _person21!).Item2;
    public static Person Person22 => _person22 ?? (NewPersons(), _person22!).Item2;
    public static Person Person23 => _person23 ?? (NewPersons(), _person23!).Item2;
    public static Person Person24 => _person24 ?? (NewPersons(), _person24!).Item2;
    public static Person Person25 => _person25 ?? (NewPersons(), _person25!).Item2;
    public static Person Person26 => _person26 ?? (NewPersons(), _person26!).Item2;
    public static Person Person27 => _person27 ?? (NewPersons(), _person27!).Item2;
    public static Person Person28 => _person28 ?? (NewPersons(), _person28!).Item2;
    public static Person Person29 => _person29 ?? (NewPersons(), _person29!).Item2;
    public static Person Person30 => _person30 ?? (NewPersons(), _person30!).Item2;
    public static Person Person31 => _person31 ?? (NewPersons(), _person31!).Item2;
    public static Person Person32 => _person32 ?? (NewPersons(), _person32!).Item2;
    public static Person Person33 => _person33 ?? (NewPersons(), _person33!).Item2;
    public static Person Person34 => _person34 ?? (NewPersons(), _person34!).Item2;
    public static Person Person35 => _person35 ?? (NewPersons(), _person35!).Item2;
    public static Person Person36 => _person36 ?? (NewPersons(), _person36!).Item2;
    public static Person Person37 => _person37 ?? (NewPersons(), _person37!).Item2;
    public static Person Person38 => _person38 ?? (NewPersons(), _person38!).Item2;
    public static Person Person39 => _person39 ?? (NewPersons(), _person39!).Item2;
    public static Person Person40 => _person40 ?? (NewPersons(), _person40!).Item2;
    public static Person Person41 => _person41 ?? (NewPersons(), _person41!).Item2;
    public static Person Person42 => _person42 ?? (NewPersons(), _person42!).Item2;
}

