namespace vm2.Repository.TestData;

using vm2.Repository.Domain.Dimensions;

public static class Countries
{
    static Country? _country01;
    static Country? _country02;
    static Country? _country03;
    static Country? _country04;
    static Country? _country05;
    static Country? _country06;
    static Country? _country07;
    static Country? _country08;
    static Country? _country09;
    static Country? _country10;
    static Country? _country11;
    static Country? _country12;
    static Country? _country13;
    static Country? _country14;
    static Country? _country15;
    static Country? _country16;
    static Country? _country17;
    static Country? _country18;
    static Country? _country19;
    static Country? _country20;

    static IEnumerable<Country> _allCountries = [];

    internal static IEnumerable<Country> NewCountries()
        => _allCountries = new[]
        {
            _country01 = new Country(Code: "AR", Name: "Argentina"),
            _country02 = new Country(Code: "AU", Name: "Australia"),
            _country03 = new Country(Code: "BR", Name: "Brazil"),
            _country04 = new Country(Code: "CA", Name: "Canada"),
            _country05 = new Country(Code: "CN", Name: "China"),
            _country06 = new Country(Code: "FR", Name: "France"),
            _country07 = new Country(Code: "DE", Name: "Germany"),
            _country08 = new Country(Code: "IN", Name: "India"),
            _country09 = new Country(Code: "ID", Name: "Indonesia"),
            _country10 = new Country(Code: "IT", Name: "Italy"),
            _country11 = new Country(Code: "JP", Name: "Japan"),
            _country12 = new Country(Code: "KR", Name: "South Korea"),
            _country13 = new Country(Code: "MX", Name: "Mexico"),
            _country14 = new Country(Code: "RU", Name: "Russia"),
            _country15 = new Country(Code: "SA", Name: "Saudi Arabia"),
            _country16 = new Country(Code: "ZA", Name: "South Africa"),
            _country17 = new Country(Code: "TR", Name: "Turkey"),
            _country18 = new Country(Code: "GB", Name: "United Kingdom"),
            _country19 = new Country(Code: "US", Name: "United States of America"),
            _country20 = new Country(Code: "ES", Name: "Spain"),
        };

    public static IEnumerable<Country> CountriesSequence => _allCountries.Any() ? _allCountries : NewCountries();

    public static Country Country01 => _country01 ?? (NewCountries(), _country01!).Item2;
    public static Country Country02 => _country02 ?? (NewCountries(), _country02!).Item2;
    public static Country Country03 => _country03 ?? (NewCountries(), _country03!).Item2;
    public static Country Country04 => _country04 ?? (NewCountries(), _country04!).Item2;
    public static Country Country05 => _country05 ?? (NewCountries(), _country05!).Item2;
    public static Country Country06 => _country06 ?? (NewCountries(), _country06!).Item2;
    public static Country Country07 => _country07 ?? (NewCountries(), _country07!).Item2;
    public static Country Country08 => _country08 ?? (NewCountries(), _country08!).Item2;
    public static Country Country09 => _country09 ?? (NewCountries(), _country09!).Item2;
    public static Country Country10 => _country10 ?? (NewCountries(), _country10!).Item2;
    public static Country Country11 => _country11 ?? (NewCountries(), _country11!).Item2;
    public static Country Country12 => _country12 ?? (NewCountries(), _country12!).Item2;
    public static Country Country13 => _country13 ?? (NewCountries(), _country13!).Item2;
    public static Country Country14 => _country14 ?? (NewCountries(), _country14!).Item2;
    public static Country Country15 => _country15 ?? (NewCountries(), _country15!).Item2;
    public static Country Country16 => _country16 ?? (NewCountries(), _country16!).Item2;
    public static Country Country17 => _country17 ?? (NewCountries(), _country17!).Item2;
    public static Country Country18 => _country18 ?? (NewCountries(), _country18!).Item2;
    public static Country Country19 => _country19 ?? (NewCountries(), _country19!).Item2;
    public static Country Country20 => _country20 ?? (NewCountries(), _country20!).Item2;
}
