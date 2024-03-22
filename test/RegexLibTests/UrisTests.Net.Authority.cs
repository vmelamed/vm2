﻿namespace vm2.RegexLibTests;

public partial class UrisTests
{
    public static UriTheoryData NetAddressData = new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine("DnsName"), true, "maria.vtmelamed.com:8080", new()
                                                        {
                                                            ["address"] = "maria.vtmelamed.com:8080",
                                                            ["host"] = "maria.vtmelamed.com",
                                                            ["ipDnsName"] = "maria.vtmelamed.com",
                                                            ["port"] = "8080",
                                                        } },
        { TestLine("Incomplete IPv4"), false, "1.2.3:321", null },
        { TestLine("Complete IPv4"), true, "1.2.3.4:8443", new()
                                                        {
                                                            ["address"] = "1.2.3.4:8443",
                                                            ["host"] = "1.2.3.4",
                                                            ["ipv4"] = "1.2.3.4",
                                                            ["port"] = "8443",
                                                        } },
        { TestLine("Complete unbracketed IPv6"), false, "1:2:3::4", null },
        { TestLine("Complete unbracketed IPv6"), false, "1:2:3::4:17", null },
        { TestLine("Complete IPv6"), true, "[1:2:3::4]:17", new()
                                                        {
                                                            ["address"] = "[1:2:3::4]:17",
                                                            ["host"] = "[1:2:3::4]",
                                                            ["ipv6"] = "1:2:3::4",
                                                            ["port"] = "17",
                                                        } },
        { TestLine("Complete IPvF"), true, "[v1a.skiledh.srethg.23546.]:80", new()
                                                        {
                                                            ["address"] = "[v1a.skiledh.srethg.23546.]:80",
                                                            ["host"] = "[v1a.skiledh.srethg.23546.]",
                                                            ["ipvF"] = "v1a.skiledh.srethg.23546.",
                                                            ["port"] = "80",
                                                        } },
        { TestLine("Complete unbracketed IPvF"), false, "v1a.skiledh.srethg.23546.:443", null },
        { TestLine("General name in Unicode"), false, "дир.бг:65534", null },
        { TestLine("General name in percent URL encoded"), false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:8080", null },
        { TestLine("General name in percent URL encoded"), false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:808080", null },
    };

    public static UriTheoryData NetAuthorityData = new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), true, "maria.vtmelamed.com:8080", new()
                                                {
                                                    ["authority"] = "maria.vtmelamed.com:8080",
                                                    ["address"] = "maria.vtmelamed.com:8080",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "8080",
                                                    ["user"] = "",
                                                    ["access"] = "",
                                                } },
        { TestLine(), true, "john.doe@maria.vtmelamed.com:8080", new()
                                                {
                                                    ["authority"] = "john.doe@maria.vtmelamed.com:8080",
                                                    ["address"] = "maria.vtmelamed.com:8080",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "8080",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "",
                                                } },
        { TestLine(), true, "john.doe@maria.vtmelamed.com", new()
                                                {
                                                    ["authority"] = "john.doe@maria.vtmelamed.com",
                                                    ["address"] = "maria.vtmelamed.com",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "",
                                                } },
        { TestLine(), true, "john.doe:@maria.vtmelamed.com", new()
                                                {
                                                    ["authority"] = "john.doe:@maria.vtmelamed.com",
                                                    ["address"] = "maria.vtmelamed.com",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "",
                                                } },
        { TestLine(), true, "john.doe:@maria.vtmelamed.com:8080", new()
                                                {
                                                    ["authority"] = "john.doe:@maria.vtmelamed.com:8080",
                                                    ["address"] = "maria.vtmelamed.com:8080",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "8080",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "",
                                                } },
        { TestLine(), true, "john.doe:secret@maria.vtmelamed.com:8080", new()
                                                {
                                                    ["authority"] = "john.doe:secret@maria.vtmelamed.com:8080",
                                                    ["address"] = "maria.vtmelamed.com:8080",
                                                    ["host"] = "maria.vtmelamed.com",
                                                    ["ipDnsName"] = "maria.vtmelamed.com",
                                                    ["port"] = "8080",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestLine(), false, "john.doe:secret@1.2.3:321", null },
        { TestLine("Complete IPv4"), true, "john.doe:secret@1.2.3.4:8443", new()
                                                {
                                                    ["authority"] = "john.doe:secret@1.2.3.4:8443",
                                                    ["address"] = "1.2.3.4:8443",
                                                    ["host"] = "1.2.3.4",
                                                    ["ipv4"] = "1.2.3.4",
                                                    ["port"] = "8443",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestLine(), true, "john.doe:secret@[1:2:3::4]:17", new()
                                                {
                                                    ["authority"] = "john.doe:secret@[1:2:3::4]:17",
                                                    ["address"] = "[1:2:3::4]:17",
                                                    ["host"] = "[1:2:3::4]",
                                                    ["ipv6"] = "1:2:3::4",
                                                    ["port"] = "17",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestLine(), true, "john.doe:secret@[v1a.skiledh.srethg.23546.]:80", new()
                                                {
                                                    ["authority"] = "john.doe:secret@[v1a.skiledh.srethg.23546.]:80",
                                                    ["address"] = "[v1a.skiledh.srethg.23546.]:80",
                                                    ["host"] = "[v1a.skiledh.srethg.23546.]",
                                                    ["ipvF"] = "v1a.skiledh.srethg.23546.",
                                                    ["port"] = "80",
                                                    ["user"] = "john.doe",
                                                    ["access"] = "secret",
                                                } },
        { TestLine(), false, "john.doe:secret@v1a.skiledh.srethg.23546.:443", null },
        { TestLine(), false, "john.doe:secret@%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3:8080", null },
    };
}