namespace vm2.RegexLibTests;

public partial class UrisTests
{
    public static UriTheoryData NetAddressData = new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine("DnsName"), true, "maria.vtmelamed.com:8080", new()
                                                        {
                                                            ["endpoint"] = "maria.vtmelamed.com:8080",
                                                            ["host"] = "maria.vtmelamed.com",
                                                            ["ipDnsName"] = "maria.vtmelamed.com",
                                                            ["port"] = "8080",
                                                        } },
        { TestFileLine("Incomplete IPv4"), false, "1.2.3:321", null },
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
        { TestFileLine("Complete unbracketed IPvF"), false, "v1a.skiledh.srethg.23546.:443", null },
        { TestFileLine("General name in Unicode"), false, "дир.бг:65534", null },
        { TestFileLine("General name in percent URL encoded"), false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:8080", null },
        { TestFileLine("General name in percent URL encoded"), false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:808080", null },
    };

    public static UriTheoryData NetAuthorityData = new() {
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
        { TestFileLine(), false, "john.doe:secret@1.2.3:321", null },
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
        { TestFileLine(), false, "john.doe:secret@v1a.skiledh.srethg.23546.:443", null },
        { TestFileLine(), false, "john.doe:secret@%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:8080", null },
    };
}
