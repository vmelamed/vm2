namespace vm2.RegexLib.Tests;

public partial class UrisTests
{
    public static UriTheoryData AddressData = new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine("DnsName"), true, "maria.vtmelamed.com:8080", new()
                                                        {
                                                            ["endpoint"] = "maria.vtmelamed.com:8080",
                                                            ["host"] = "maria.vtmelamed.com",
                                                            ["ipDnsName"] = "maria.vtmelamed.com",
                                                            ["port"] = "8080",
                                                        } },
        { TestFileLine("Incomplete IPv4"), true, "1.2.3:321", new()
                                                        {
                                                            ["endpoint"] = "1.2.3:321",
                                                            ["host"] = "1.2.3",
                                                            ["ipRegName"] = "1.2.3",
                                                            ["port"] = "321",
                                                        } },
        { TestFileLine("Complete IPv4"), true, "1.2.3.4:8443", new()
                                                        {
                                                            ["endpoint"] = "1.2.3.4:8443",
                                                            ["host"] = "1.2.3.4",
                                                            ["ipv4"] = "1.2.3.4",
                                                            ["port"] = "8443",
                                                        } },
        { TestFileLine("Complete unbracketed IPv6"), false, "1:2:3::4", null },
        { TestFileLine("Complete unbracketed IPv6"), false, "1:2:3::4:17", null },
        { TestFileLine("Complete IPv6"), true, "[1:2:3::4]:17", new()
                                                        {
                                                            ["endpoint"] = "[1:2:3::4]:17",
                                                            ["host"] = "[1:2:3::4]",
                                                            ["ipv6nz"] = "1:2:3::4",
                                                            ["ipv6"] = "1:2:3::4",
                                                            ["port"] = "17",
                                                        } },
        { TestFileLine("Complete IPvF"), true, "[v1a.skiledh.srethg.23546.]:80", new()
                                                        {
                                                            ["endpoint"] = "[v1a.skiledh.srethg.23546.]:80",
                                                            ["host"] = "[v1a.skiledh.srethg.23546.]",
                                                            ["ipvF"] = "v1a.skiledh.srethg.23546.",
                                                            ["port"] = "80",
                                                        } },
        { TestFileLine("Complete unbracketed IPvF"), true, "v1a.skiledh.srethg.23546.:443", new()
                                                        {
                                                            ["endpoint"] = "v1a.skiledh.srethg.23546.:443",
                                                            ["host"] = "v1a.skiledh.srethg.23546.",
                                                            ["ipRegName"] = "v1a.skiledh.srethg.23546.",
                                                            ["port"] = "443",
                                                        } },
        { TestFileLine("General name in Unicode"), false, "дир.бг:65534", null },
        { TestFileLine("General name in percent URL encoded"), true, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:8080", new()
                                                        {
                                                            ["endpoint"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:8080",
                                                            ["host"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                            ["ipRegName"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                            ["port"] = "8080",
                                                        } },
        { TestFileLine("General name in percent URL encoded"), false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:808080", null },
    };

    public static UriTheoryData AuthorityData = new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), true, "maria.vtmelamed.com:8080", new()
                                                {
                                                    ["authority"] = "maria.vtmelamed.com:8080",
                                                    ["endpoint"] = "maria.vtmelamed.com:8080",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "8080",
                                                    ["user"] = "",
                                                    ["access"] = "",
                                                } },
        { TestFileLine(), true, "john.doe@maria.vtmelamed.com:8080", new()
                                                {
                                                    ["authority"] = "john.doe@maria.vtmelamed.com:8080",
                                                    ["endpoint"] = "maria.vtmelamed.com:8080",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "8080",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "",
                                                } },
        { TestFileLine(), true, "john.doe@maria.vtmelamed.com", new()
                                                {
                                                    ["authority"] = "john.doe@maria.vtmelamed.com",
                                                    ["endpoint"] = "maria.vtmelamed.com",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "",
                                                } },
        { TestFileLine(), true, "john.doe:@maria.vtmelamed.com", new()
                                                {
                                                    ["authority"] = "john.doe:@maria.vtmelamed.com",
                                                    ["endpoint"] = "maria.vtmelamed.com",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "",
                                                } },
        { TestFileLine(), true, "john.doe:@maria.vtmelamed.com:8080", new()
                                                {
                                                    ["authority"] = "john.doe:@maria.vtmelamed.com:8080",
                                                    ["endpoint"] = "maria.vtmelamed.com:8080",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "8080",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "",
                                                } },
        { TestFileLine(), true, "john.doe:secret@maria.vtmelamed.com:8080", new()
                                                {
                                                    ["authority"] = "john.doe:secret@maria.vtmelamed.com:8080",
                                                    ["endpoint"] = "maria.vtmelamed.com:8080",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "8080",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestFileLine(), true, "john.doe:secret@1.2.3:321", new()
                                                {
                                                    ["authority"] = "john.doe:secret@1.2.3:321",
                                                    ["endpoint"] = "1.2.3:321",
                                                    ["host"] = "1.2.3",
                                                    ["ipRegName"] = "1.2.3",
                                                    ["port"] = "321",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestFileLine("Complete IPv4"), true, "john.doe:secret@1.2.3.4:8443", new()
                                                {
                                                    ["authority"] = "john.doe:secret@1.2.3.4:8443",
                                                    ["endpoint"] = "1.2.3.4:8443",
                                                    ["host"] = "1.2.3.4",
                                                    ["ipv4"] = "1.2.3.4",
                                                    ["port"] = "8443",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestFileLine(), true, "john.doe:secret@[1:2:3::4]:17", new()
                                                {
                                                    ["authority"] = "john.doe:secret@[1:2:3::4]:17",
                                                    ["endpoint"] = "[1:2:3::4]:17",
                                                    ["host"] = "[1:2:3::4]",
                                                    ["ipv6nz"] = "1:2:3::4",
                                                    ["ipv6"] = "1:2:3::4",
                                                    ["port"] = "17",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestFileLine(), true, "john.doe:secret@[v1a.skiledh.srethg.23546.]:80", new()
                                                {
                                                    ["authority"] = "john.doe:secret@[v1a.skiledh.srethg.23546.]:80",
                                                    ["endpoint"] = "[v1a.skiledh.srethg.23546.]:80",
                                                    ["host"] = "[v1a.skiledh.srethg.23546.]",
                                                    ["ipvF"] = "v1a.skiledh.srethg.23546.",
                                                    ["port"] = "80",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestFileLine(), true, "john.doe:secret@v1a.skiledh.srethg.23546.:443", new()
                                                {
                                                    ["authority"] = "john.doe:secret@v1a.skiledh.srethg.23546.:443",
                                                    ["endpoint"] = "v1a.skiledh.srethg.23546.:443",
                                                    ["host"] = "v1a.skiledh.srethg.23546.",
                                                    ["ipRegName"] = "v1a.skiledh.srethg.23546.",
                                                    ["port"] = "443",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestFileLine(), true, "john.doe:secret@%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:8080", new()
                                                {
                                                    ["authority"] = "john.doe:secret@%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:8080",
                                                    ["endpoint"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:8080",
                                                    ["host"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                    ["ipRegName"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                    ["port"] = "8080",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
    };
}
