namespace vm2.Repository.TestData.Dimensions;

public static class Countries
{
    internal static IEnumerable<Country> NewCountries()
        => CountriesSequence = new[]
        {
            Country01 = new Country(Code: "AR", Name: "Argentina"),
            Country02 = new Country(Code: "AU", Name: "Australia"),
            Country03 = new Country(Code: "BR", Name: "Brazil"),
            Country04 = new Country(Code: "CA", Name: "Canada"),
            Country05 = new Country(Code: "CN", Name: "China"),
            Country06 = new Country(Code: "FR", Name: "France"),
            Country07 = new Country(Code: "DE", Name: "Germany"),
            Country08 = new Country(Code: "IN", Name: "India"),
            Country09 = new Country(Code: "ID", Name: "Indonesia"),
            Country10 = new Country(Code: "IT", Name: "Italy"),
            Country11 = new Country(Code: "JP", Name: "Japan"),
            Country12 = new Country(Code: "KR", Name: "South Korea"),
            Country13 = new Country(Code: "MX", Name: "Mexico"),
            Country14 = new Country(Code: "RU", Name: "Russia"),
            Country15 = new Country(Code: "SA", Name: "Saudi Arabia"),
            Country16 = new Country(Code: "ZA", Name: "South Africa"),
            Country17 = new Country(Code: "TR", Name: "Turkey"),
            Country18 = new Country(Code: "GB", Name: "United Kingdom"),
            Country19 = new Country(Code: "US", Name: "United States of America"),
            Country20 = new Country(Code: "ES", Name: "Spain"),
        };

    public static IEnumerable<Country> CountriesSequence { get => field.Any() ? field : NewCountries(); private set; } = [];

    public static Country Country01 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country02 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country03 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country04 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country05 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country06 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country07 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country08 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country09 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country10 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country11 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country12 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country13 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country14 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country15 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country16 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country17 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country18 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country19 { get => field ?? (NewCountries(), field!).Item2; private set; }
    public static Country Country20 { get => field ?? (NewCountries(), field!).Item2; private set; }
}
