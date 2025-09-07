namespace vm2.RegexLib.Tests;

public partial class NetTests
{
    // Almost all test cases from https://www.fortra.com/product-lines/intermapper/ipv6-test-address-validation
    public static TheoryData<string, bool, string, Captures?> Ipv6AddressData = new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false, ".", null },
        { TestFileLine(), false, ":", null },
        { TestFileLine(), false, "::", null },
        { TestFileLine(), false, "::0", null },
        { TestFileLine(), false, "0:0:0:0:0:0:0:0", null },
        { TestFileLine(), true, "1::", new() { ["ipv6nz"] = "1::", ["ipv6"] = "1::" }  },
        { TestFileLine(), true, "1:2::", new() { ["ipv6nz"] = "1:2::", ["ipv6"] = "1:2::" }  },
        { TestFileLine(), true, "1:2:3::", new() { ["ipv6nz"] = "1:2:3::", ["ipv6"] = "1:2:3::" }  },
        { TestFileLine(), true, "1:2:3:4::", new() { ["ipv6nz"] = "1:2:3:4::", ["ipv6"] = "1:2:3:4::" }  },
        { TestFileLine(), true, "1:2:3:4:5::", new() { ["ipv6nz"] = "1:2:3:4:5::", ["ipv6"] = "1:2:3:4:5::" }  },
        { TestFileLine(), true, "1:2:3:4:5:6::", new() { ["ipv6nz"] = "1:2:3:4:5:6::", ["ipv6"] = "1:2:3:4:5:6::" }  },
        { TestFileLine(), true, "1:2:3:4:5:6:7::", new() { ["ipv6nz"] = "1:2:3:4:5:6:7::", ["ipv6"] = "1:2:3:4:5:6:7::" }  },
        { TestFileLine(), true, "::8", new() { ["ipv6nz"] = "::8", ["ipv6"] = "::8" }  },
        { TestFileLine(), true, "1::8", new() { ["ipv6nz"] = "1::8", ["ipv6"] = "1::8" }  },
        { TestFileLine(), true, "1:2::8", new() { ["ipv6nz"] = "1:2::8", ["ipv6"] = "1:2::8" }  },
        { TestFileLine(), true, "1:2:3::8", new() { ["ipv6nz"] = "1:2:3::8", ["ipv6"] = "1:2:3::8" }  },
        { TestFileLine(), true, "1:2:3:4::8", new() { ["ipv6nz"] = "1:2:3:4::8", ["ipv6"] = "1:2:3:4::8" }  },
        { TestFileLine(), true, "1:2:3:4:5::8", new() { ["ipv6nz"] = "1:2:3:4:5::8", ["ipv6"] = "1:2:3:4:5::8" }  },
        { TestFileLine(), true, "1:2:3:4:5:6::8", new() { ["ipv6nz"] = "1:2:3:4:5:6::8", ["ipv6"] = "1:2:3:4:5:6::8" }  },
        { TestFileLine(), true, "::7:8", new() { ["ipv6nz"] = "::7:8", ["ipv6"] = "::7:8" }  },
        { TestFileLine(), true, "1::7:8", new() { ["ipv6nz"] = "1::7:8", ["ipv6"] = "1::7:8" }  },
        { TestFileLine(), true, "1:2::7:8", new() { ["ipv6nz"] = "1:2::7:8", ["ipv6"] = "1:2::7:8" }  },
        { TestFileLine(), true, "1:2:3::7:8", new() { ["ipv6nz"] = "1:2:3::7:8", ["ipv6"] = "1:2:3::7:8" }  },
        { TestFileLine(), true, "1:2:3:4::7:8", new() { ["ipv6nz"] = "1:2:3:4::7:8", ["ipv6"] = "1:2:3:4::7:8" }  },
        { TestFileLine(), true, "1:2:3:4:5::7:8", new() { ["ipv6nz"] = "1:2:3:4:5::7:8", ["ipv6"] = "1:2:3:4:5::7:8" }  },
        { TestFileLine(), true, "::6:7:8", new() { ["ipv6nz"] = "::6:7:8", ["ipv6"] = "::6:7:8" }  },
        { TestFileLine(), true, "1::6:7:8", new() { ["ipv6nz"] = "1::6:7:8", ["ipv6"] = "1::6:7:8" }  },
        { TestFileLine(), true, "1:2::6:7:8", new() { ["ipv6nz"] = "1:2::6:7:8", ["ipv6"] = "1:2::6:7:8" }  },
        { TestFileLine(), true, "1:2:3::6:7:8", new() { ["ipv6nz"] = "1:2:3::6:7:8", ["ipv6"] = "1:2:3::6:7:8" }  },
        { TestFileLine(), true, "1:2:3:4::6:7:8", new() { ["ipv6nz"] = "1:2:3:4::6:7:8", ["ipv6"] = "1:2:3:4::6:7:8" }  },
        { TestFileLine(), true, "::5:6:7:8", new() { ["ipv6nz"] = "::5:6:7:8", ["ipv6"] = "::5:6:7:8" }  },
        { TestFileLine(), true, "1::5:6:7:8", new() { ["ipv6nz"] = "1::5:6:7:8", ["ipv6"] = "1::5:6:7:8" }  },
        { TestFileLine(), true, "1:2::5:6:7:8", new() { ["ipv6nz"] = "1:2::5:6:7:8", ["ipv6"] = "1:2::5:6:7:8" }  },
        { TestFileLine(), true, "1:2:3::5:6:7:8", new() { ["ipv6nz"] = "1:2:3::5:6:7:8", ["ipv6"] = "1:2:3::5:6:7:8" }  },
        { TestFileLine(), true, "::4:5:6:7:8", new() { ["ipv6nz"] = "::4:5:6:7:8", ["ipv6"] = "::4:5:6:7:8" }  },
        { TestFileLine(), true, "1::4:5:6:7:8", new() { ["ipv6nz"] = "1::4:5:6:7:8", ["ipv6"] = "1::4:5:6:7:8" }  },
        { TestFileLine(), true, "1:2::4:5:6:7:8", new() { ["ipv6nz"] = "1:2::4:5:6:7:8", ["ipv6"] = "1:2::4:5:6:7:8" }  },
        { TestFileLine(), true, "::3:4:5:6:7:8", new() { ["ipv6nz"] = "::3:4:5:6:7:8", ["ipv6"] = "::3:4:5:6:7:8" }  },
        { TestFileLine(), true, "1::3:4:5:6:7:8", new() { ["ipv6nz"] = "1::3:4:5:6:7:8", ["ipv6"] = "1::3:4:5:6:7:8" }  },
        { TestFileLine(), true, "::2:3:4:5:6:7:8", new() { ["ipv6nz"] = "::2:3:4:5:6:7:8", ["ipv6"] = "::2:3:4:5:6:7:8" }  },
        { TestFileLine(), true, "1:2:3:4:5:6:7:8", new() { ["ipv6nz"] = "1:2:3:4:5:6:7:8", ["ipv6"] = "1:2:3:4:5:6:7:8" }  },
        { TestFileLine(), true, "0:0:0:0:0:0:1.2.3.4", new() { ["ipv6nz"] = "0:0:0:0:0:0:1.2.3.4", ["ipv6"] = "0:0:0:0:0:0:1.2.3.4" }  },
        { TestFileLine(), true, "0:0:0:0:0:ffff:1.2.3.4", new() { ["ipv6nz"] = "0:0:0:0:0:ffff:1.2.3.4", ["ipv6"] = "0:0:0:0:0:ffff:1.2.3.4" }  },
        { TestFileLine(), true, "0:0:0:0::0:1.2.3.4", new() { ["ipv6nz"] = "0:0:0:0::0:1.2.3.4", ["ipv6"] = "0:0:0:0::0:1.2.3.4" }  },
        { TestFileLine(), true, "0:0:0:0::ffff:1.2.3.4", new() { ["ipv6nz"] = "0:0:0:0::ffff:1.2.3.4", ["ipv6"] = "0:0:0:0::ffff:1.2.3.4" }  },
        { TestFileLine(), true, "0:0:0::0:1.2.3.4", new() { ["ipv6nz"] = "0:0:0::0:1.2.3.4", ["ipv6"] = "0:0:0::0:1.2.3.4" }  },
        { TestFileLine(), true, "0:0:0::ffff:1.2.3.4", new() { ["ipv6nz"] = "0:0:0::ffff:1.2.3.4", ["ipv6"] = "0:0:0::ffff:1.2.3.4" }  },
        { TestFileLine(), true, "0:0::0:1.2.3.4", new() { ["ipv6nz"] = "0:0::0:1.2.3.4", ["ipv6"] = "0:0::0:1.2.3.4" }  },
        { TestFileLine(), true, "0:0::ffff:1.2.3.4", new() { ["ipv6nz"] = "0:0::ffff:1.2.3.4", ["ipv6"] = "0:0::ffff:1.2.3.4" }  },
        { TestFileLine(), true, "0::0:1.2.3.4", new() { ["ipv6nz"] = "0::0:1.2.3.4", ["ipv6"] = "0::0:1.2.3.4" }  },
        { TestFileLine(), true, "0::ffff:1.2.3.4", new() { ["ipv6nz"] = "0::ffff:1.2.3.4", ["ipv6"] = "0::ffff:1.2.3.4" }  },
        { TestFileLine(), true, "::0:1.2.3.4", new() { ["ipv6nz"] = "::0:1.2.3.4", ["ipv6"] = "::0:1.2.3.4" }  },
        { TestFileLine(), true, "::ffff:1.2.3.4", new() { ["ipv6nz"] = "::ffff:1.2.3.4", ["ipv6"] = "::ffff:1.2.3.4" }  },
        { TestFileLine(), true, "::0:1.2.3.4", new() { ["ipv6nz"] = "::0:1.2.3.4", ["ipv6"] = "::0:1.2.3.4" }  },
        { TestFileLine(), true, "::1.2.3.4", new() { ["ipv6nz"] = "::1.2.3.4", ["ipv6"] = "::1.2.3.4" }  },
        { TestFileLine(), false, "::", null },

        { TestFileLine(), false, "", null }, // empty string 
        { TestFileLine(), true, "::1", new() { ["ipv6nz"] = "::1", ["ipv6"] = "::1" }  }, // loopback, compressed, non-routable 
        { TestFileLine(), false, "::", null }, // unspecified, compressed, non-routable 
        { TestFileLine(), true, "0:0:0:0:0:0:0:1", new() { ["ipv6nz"] = "0:0:0:0:0:0:0:1", ["ipv6"] = "0:0:0:0:0:0:0:1" }  }, // loopback, full 
        { TestFileLine(), false, "0:0:0:0:0:0:0:0", null }, // unspecified, full 
        { TestFileLine(), true, "2001:DB8:0:0:8:800:200C:417A", new() { ["ipv6nz"] = "2001:DB8:0:0:8:800:200C:417A", ["ipv6"] = "2001:DB8:0:0:8:800:200C:417A" }  }, // unicast, full 
        { TestFileLine(), true, "FF01:0:0:0:0:0:0:101", new() { ["ipv6nz"] = "FF01:0:0:0:0:0:0:101", ["ipv6"] = "FF01:0:0:0:0:0:0:101" }  }, // multicast, full 
        { TestFileLine(), true, "2001:DB8::8:800:200C:417A", new() { ["ipv6nz"] = "2001:DB8::8:800:200C:417A", ["ipv6"] = "2001:DB8::8:800:200C:417A" }  }, // unicast, compressed 
        { TestFileLine(), true, "FF01::101", new() { ["ipv6nz"] = "FF01::101", ["ipv6"] = "FF01::101" }  }, // multicast, compressed 
        { TestFileLine(), false, "2001:DB8:0:0:8:800:200C:417A:221", null }, // unicast, full 
        { TestFileLine(), false, "FF01::101::2", null }, // multicast, compressed 
        { TestFileLine(), true, "fe80::217:f2ff:fe07:ed62", new() { ["ipv6nz"] = "fe80::217:f2ff:fe07:ed62", ["ipv6"] = "fe80::217:f2ff:fe07:ed62" }  },
        { TestFileLine(), true, "fe80::217:f2ff:fe07:ed62%eth0", new() { ["ipv6nz"] = "fe80::217:f2ff:fe07:ed62", ["ipv6"] = "fe80::217:f2ff:fe07:ed62%eth0", ["zoneId"] = "eth0" } },

        { TestFileLine(), true, "2001:0000:1234:0000:0000:C1C0:ABCD:0876", new() { ["ipv6nz"] = "2001:0000:1234:0000:0000:C1C0:ABCD:0876", ["ipv6"] = "2001:0000:1234:0000:0000:C1C0:ABCD:0876" }  },
        { TestFileLine(), true, "3ffe:0b00:0000:0000:0001:0000:0000:000a", new() { ["ipv6nz"] = "3ffe:0b00:0000:0000:0001:0000:0000:000a", ["ipv6"] = "3ffe:0b00:0000:0000:0001:0000:0000:000a" }  },
        { TestFileLine(), true, "FF02:0000:0000:0000:0000:0000:0000:0001", new() { ["ipv6nz"] = "FF02:0000:0000:0000:0000:0000:0000:0001", ["ipv6"] = "FF02:0000:0000:0000:0000:0000:0000:0001" }  },
        { TestFileLine(), true, "0000:0000:0000:0000:0000:0000:0000:0001", new() { ["ipv6nz"] = "0000:0000:0000:0000:0000:0000:0000:0001", ["ipv6"] = "0000:0000:0000:0000:0000:0000:0000:0001" }  },
        { TestFileLine(), false, "0000:0000:0000:0000:0000:0000:0000:0000", null },
        { TestFileLine(), false, "02001:0000:1234:0000:0000:C1C0:ABCD:0876", null },	    // extra 0 not allowed! 
        { TestFileLine(), false, "2001:0000:1234:0000:00001:C1C0:ABCD:0876", null },	    // extra 0 not allowed! 
        { TestFileLine(), false, " 2001:0000:1234:0000:0000:C1C0:ABCD:0876", null },      // leading space
        { TestFileLine(), false, "2001:0000:1234:0000:0000:C1C0:ABCD:0876 ", null },	    // trailing space
        { TestFileLine(), false, " 2001:0000:1234:0000:0000:C1C0:ABCD:0876  ", null },	// leading and trailing space
        { TestFileLine(), false, "2001:0000:1234:0000:0000:C1C0:ABCD:0876  0", null },    // junk after valid address
        { TestFileLine(), false, "2001:0000:1234: 0000:0000:C1C0:ABCD:0876", null },	    // internal space

        { TestFileLine(), false, "3ffe:0b00:0000:0001:0000:0000:000a", null },			 // seven segments
        { TestFileLine(), false, "FF02:0000:0000:0000:0000:0000:0000:0000:0001", null },	 // nine segments
        { TestFileLine(), false, "3ffe:b00::1::a", null },								 // double "::"
        { TestFileLine(), false, "::1111:2222:3333:4444:5555:6666::", null },			 // double "::"
        { TestFileLine(), true, "2::10", new() { ["ipv6nz"] = "2::10", ["ipv6"] = "2::10" }  },
        { TestFileLine(), true, "ff02::1", new() { ["ipv6nz"] = "ff02::1", ["ipv6"] = "ff02::1" }  },
        { TestFileLine(), true, "fe80::", new() { ["ipv6nz"] = "fe80::", ["ipv6"] = "fe80::" }  },
        { TestFileLine(), true, "fe80::%7", new() { ["ipv6nz"] = "fe80::", ["ipv6"] = "fe80::%7", ["zoneId"] = "7" }  },
        { TestFileLine(), true, "2002::", new() { ["ipv6nz"] = "2002::", ["ipv6"] = "2002::" }  },
        { TestFileLine(), true, "2001:db8::", new() { ["ipv6nz"] = "2001:db8::", ["ipv6"] = "2001:db8::" }  },
        { TestFileLine(), true, "2001:0db8:1234::", new() { ["ipv6nz"] = "2001:0db8:1234::", ["ipv6"] = "2001:0db8:1234::" }  },
        { TestFileLine(), true, "::ffff:0:0", new() { ["ipv6nz"] = "::ffff:0:0", ["ipv6"] = "::ffff:0:0" }  },
        { TestFileLine(), true, "::1", new() { ["ipv6nz"] = "::1", ["ipv6"] = "::1" }  },
        { TestFileLine(), true, "1:2:3:4:5:6:7:8", new() { ["ipv6nz"] = "1:2:3:4:5:6:7:8", ["ipv6"] = "1:2:3:4:5:6:7:8" }  },
        { TestFileLine(), true, "1:2:3:4:5:6::8", new() { ["ipv6nz"] = "1:2:3:4:5:6::8", ["ipv6"] = "1:2:3:4:5:6::8" }  },
        { TestFileLine(), true, "1:2:3:4:5::8", new() { ["ipv6nz"] = "1:2:3:4:5::8", ["ipv6"] = "1:2:3:4:5::8" }  },
        { TestFileLine(), true, "1:2:3:4::8", new() { ["ipv6nz"] = "1:2:3:4::8", ["ipv6"] = "1:2:3:4::8" }  },
        { TestFileLine(), true, "1:2:3::8", new() { ["ipv6nz"] = "1:2:3::8", ["ipv6"] = "1:2:3::8" }  },
        { TestFileLine(), true, "1:2::8", new() { ["ipv6nz"] = "1:2::8", ["ipv6"] = "1:2::8" }  },
        { TestFileLine(), true, "1::8", new() { ["ipv6nz"] = "1::8", ["ipv6"] = "1::8" }  },
        { TestFileLine(), true, "1::2:3:4:5:6:7", new() { ["ipv6nz"] = "1::2:3:4:5:6:7", ["ipv6"] = "1::2:3:4:5:6:7" }  },
        { TestFileLine(), true, "1::2:3:4:5:6", new() { ["ipv6nz"] = "1::2:3:4:5:6", ["ipv6"] = "1::2:3:4:5:6" }  },
        { TestFileLine(), true, "1::2:3:4:5", new() { ["ipv6nz"] = "1::2:3:4:5", ["ipv6"] = "1::2:3:4:5" }  },
        { TestFileLine(), true, "1::2:3:4", new() { ["ipv6nz"] = "1::2:3:4", ["ipv6"] = "1::2:3:4" }  },
        { TestFileLine(), true, "1::2:3", new() { ["ipv6nz"] = "1::2:3", ["ipv6"] = "1::2:3" }  },
        { TestFileLine(), true, "1::8", new() { ["ipv6nz"] = "1::8", ["ipv6"] = "1::8" }  },
        { TestFileLine(), true, "::2:3:4:5:6:7:8", new() { ["ipv6nz"] = "::2:3:4:5:6:7:8", ["ipv6"] = "::2:3:4:5:6:7:8" }  },
        { TestFileLine(), true, "::2:3:4:5:6:7", new() { ["ipv6nz"] = "::2:3:4:5:6:7", ["ipv6"] = "::2:3:4:5:6:7" }  },
        { TestFileLine(), true, "::2:3:4:5:6", new() { ["ipv6nz"] = "::2:3:4:5:6", ["ipv6"] = "::2:3:4:5:6" }  },
        { TestFileLine(), true, "::2:3:4:5", new() { ["ipv6nz"] = "::2:3:4:5", ["ipv6"] = "::2:3:4:5" }  },
        { TestFileLine(), true, "::2:3:4", new() { ["ipv6nz"] = "::2:3:4", ["ipv6"] = "::2:3:4" }  },
        { TestFileLine(), true, "::2:3", new() { ["ipv6nz"] = "::2:3", ["ipv6"] = "::2:3" }  },
        { TestFileLine(), true, "::8", new() { ["ipv6nz"] = "::8", ["ipv6"] = "::8" }  },
        { TestFileLine(), true, "1:2:3:4:5:6::", new() { ["ipv6nz"] = "1:2:3:4:5:6::", ["ipv6"] = "1:2:3:4:5:6::" }  },
        { TestFileLine(), true, "1:2:3:4:5::", new() { ["ipv6nz"] = "1:2:3:4:5::", ["ipv6"] = "1:2:3:4:5::" }  },
        { TestFileLine(), true, "1:2:3:4::", new() { ["ipv6nz"] = "1:2:3:4::", ["ipv6"] = "1:2:3:4::" }  },
        { TestFileLine(), true, "1:2:3::", new() { ["ipv6nz"] = "1:2:3::", ["ipv6"] = "1:2:3::" }  },
        { TestFileLine(), true, "1:2::", new() { ["ipv6nz"] = "1:2::", ["ipv6"] = "1:2::" }  },
        { TestFileLine(), true, "1::", new() { ["ipv6nz"] = "1::", ["ipv6"] = "1::" }  },
        { TestFileLine(), true, "1:2:3:4:5::7:8", new() { ["ipv6nz"] = "1:2:3:4:5::7:8", ["ipv6"] = "1:2:3:4:5::7:8" }  },
        { TestFileLine(), false, "1:2:3::4:5::7:8", null },							 // Double "::"
        { TestFileLine(), false, "12345::6:7:8", null },
        { TestFileLine(), true, "1:2:3:4::7:8", new() { ["ipv6nz"] = "1:2:3:4::7:8", ["ipv6"] = "1:2:3:4::7:8" }  },
        { TestFileLine(), true, "1:2:3::7:8", new() { ["ipv6nz"] = "1:2:3::7:8", ["ipv6"] = "1:2:3::7:8" }  },
        { TestFileLine(), true, "1:2::7:8", new() { ["ipv6nz"] = "1:2::7:8", ["ipv6"] = "1:2::7:8" }  },
        { TestFileLine(), true, "1::7:8", new() { ["ipv6nz"] = "1::7:8", ["ipv6"] = "1::7:8" }  },

        // IPv4 addresses as dotted-quads
        { TestFileLine(), true, "0:0:0:0:0:ffff:1.2.3.4", new() { ["ipv6nz"] = "0:0:0:0:0:ffff:1.2.3.4", ["ipv6"] = "0:0:0:0:0:ffff:1.2.3.4" }  },
        { TestFileLine(), true, "0:0:0:0:0::1.2.3.4", new() { ["ipv6nz"] = "0:0:0:0:0::1.2.3.4", ["ipv6"] = "0:0:0:0:0::1.2.3.4" }  },
        { TestFileLine(), true, "0:0:0::1.2.3.4", new() { ["ipv6nz"] = "0:0:0::1.2.3.4", ["ipv6"] = "0:0:0::1.2.3.4" }  },
        { TestFileLine(), true, "0:0::1.2.3.4", new() { ["ipv6nz"] = "0:0::1.2.3.4", ["ipv6"] = "0:0::1.2.3.4" }  },
        { TestFileLine(), true, "0::1.2.3.4", new() { ["ipv6nz"] = "0::1.2.3.4", ["ipv6"] = "0::1.2.3.4" }  },
        { TestFileLine(), true, "0:0:0:0::0:1.2.3.4", new() { ["ipv6nz"] = "0:0:0:0::0:1.2.3.4", ["ipv6"] = "0:0:0:0::0:1.2.3.4" }  },
        { TestFileLine(), true, "0:0:0::0:1.2.3.4", new() { ["ipv6nz"] = "0:0:0::0:1.2.3.4", ["ipv6"] = "0:0:0::0:1.2.3.4" }  },
        { TestFileLine(), true, "0:0::0:1.2.3.4", new() { ["ipv6nz"] = "0:0::0:1.2.3.4", ["ipv6"] = "0:0::0:1.2.3.4" }  },
        { TestFileLine(), true, "0::0:1.2.3.4", new() { ["ipv6nz"] = "0::0:1.2.3.4", ["ipv6"] = "0::0:1.2.3.4" }  },
        { TestFileLine(), false, "2001:1:1:1:1:1:255Z255X255Y255", null },				 // garbage instead of "." in IPv4
        { TestFileLine(), false, "::ffff:192x168.1.26", null },							 // ditto
        { TestFileLine(), true, "::ffff:192.168.1.1", new() { ["ipv6nz"] = "::ffff:192.168.1.1", ["ipv6"] = "::ffff:192.168.1.1" }  },
        { TestFileLine(), true, "0:0:0:0:0:0:13.1.68.3", new() { ["ipv6nz"] = "0:0:0:0:0:0:13.1.68.3", ["ipv6"] = "0:0:0:0:0:0:13.1.68.3" }  }, // IPv4-compatible IPv6 address, full, deprecated 
        { TestFileLine(), true, "0:0:0:0:0:FFFF:129.144.52.38", new() { ["ipv6nz"] = "0:0:0:0:0:FFFF:129.144.52.38", ["ipv6"] = "0:0:0:0:0:FFFF:129.144.52.38" }  }, // IPv4-mapped IPv6 address, full 
        { TestFileLine(), true, "::13.1.68.3", new() { ["ipv6nz"] = "::13.1.68.3", ["ipv6"] = "::13.1.68.3" }  }, // IPv4-compatible IPv6 address, compressed, deprecated 
        { TestFileLine(), true, "::FFFF:129.144.52.38", new() { ["ipv6nz"] = "::FFFF:129.144.52.38", ["ipv6"] = "::FFFF:129.144.52.38" }  }, // IPv4-mapped IPv6 address, compressed 
        { TestFileLine(), true, "::ffff:12.34.56.78", new() { ["ipv6nz"] = "::ffff:12.34.56.78", ["ipv6"] = "::ffff:12.34.56.78" }  },
        { TestFileLine(), false, "::ffff:2.3.4", null },
        { TestFileLine(), false, "::ffff:257.1.2.3", null },
        { TestFileLine(), false, "1.2.3.4", null },

        { TestFileLine(), false, "1.2.3.4:1111:2222:3333:4444::5555", null }, // Aeron
        { TestFileLine(), false, "1.2.3.4:1111:2222:3333::5555", null },
        { TestFileLine(), false, "1.2.3.4:1111:2222::5555", null },
        { TestFileLine(), false, "1.2.3.4:1111::5555", null },
        { TestFileLine(), false, "1.2.3.4::5555", null },
        { TestFileLine(), false, "1.2.3.4::", null },

        // Testing IPv4 addresses represented as dotted-quads
        // Leading zero's in IPv4 addresses not allowed: some systems treat the leading "0" in ".086" as the start of an octal number
        // Update: The BNF in RFC-3986 explicitly defines the dec-octet (for IPv4 addresses) not to have a leading zero
        { TestFileLine(), false, "fe80:0000:0000:0000:0204:61ff:254.157.241.086", null },
        { TestFileLine(), false, "fe80:0000:0000:0000:0204:61ff:254.157.241.086%eth1", null },
        { TestFileLine(), false, "::ffff:192.0.2.128", null }, // but this is OK, since there's a single digit
        { TestFileLine(), true, "::ffff:192.1.2.128", new() { ["ipv6nz"] = "::ffff:192.1.2.128", ["ipv6"] = "::ffff:192.1.2.128" }  }, // but this is OK, since there's a single digit
        { TestFileLine(), false, "XXXX:XXXX:XXXX:XXXX:XXXX:XXXX:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:00.00.00.00", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:000.000.000.000", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:256.256.256.256", null },

        // Not testing address with subnet mask
        // { TestFileLine(), true, "2001:0DB8:0000:CD30:0000:0000:0000:0000/60", new() { ["ipv6nz"] = "2001:0DB8:0000:CD30:0000:0000:0000:0000/60", ["ipv6"] = "2001:0DB8:0000:CD30:0000:0000:0000:0000/60" }  }, // full, with prefix 
        // { TestFileLine(), true, "2001:0DB8::CD30:0:0:0:0/60", new() { ["ipv6nz"] = "2001:0DB8::CD30:0:0:0:0/60", ["ipv6"] = "2001:0DB8::CD30:0:0:0:0/60" }  }, // compressed, with prefix 
        // { TestFileLine(), true, "2001:0DB8:0:CD30::/60", new() { ["ipv6nz"] = "2001:0DB8:0:CD30::/60", ["ipv6"] = "2001:0DB8:0:CD30::/60" }  }, // compressed, with prefix // 2 
        // { TestFileLine(), true, "::/128", new() { ["ipv6nz"] = "::/128", ["ipv6"] = "::/128" }  }, // compressed, unspecified address type, non-routable 
        // { TestFileLine(), true, "::1/128", new() { ["ipv6nz"] = "::1/128", ["ipv6"] = "::1/128" }  }, // compressed, loopback address type, non-routable 
        // { TestFileLine(), true, "FF00::/8", new() { ["ipv6nz"] = "FF00::/8", ["ipv6"] = "FF00::/8" }  }, // compressed, multicast address type 
        // { TestFileLine(), true, "FE80::/10", new() { ["ipv6nz"] = "FE80::/10", ["ipv6"] = "FE80::/10" }  }, // compressed, link-local unicast, non-routable 
        // { TestFileLine(), true, "FEC0::/10", new() { ["ipv6nz"] = "FEC0::/10", ["ipv6"] = "FEC0::/10" }  }, // compressed, site-local unicast, deprecated 
        // { TestFileLine(), false, "124.15.6.89/60", null }, // standard IPv4, prefix not allowed 

        { TestFileLine(), true, "fe80:0000:0000:0000:0204:61ff:fe9d:f156", new() { ["ipv6nz"] = "fe80:0000:0000:0000:0204:61ff:fe9d:f156", ["ipv6"] = "fe80:0000:0000:0000:0204:61ff:fe9d:f156" }  },
        { TestFileLine(), true, "fe80:0000:0000:0000:0204:61ff:fe9d:f156%ab%64%65", new() { ["ipv6nz"] = "fe80:0000:0000:0000:0204:61ff:fe9d:f156", ["ipv6"] = "fe80:0000:0000:0000:0204:61ff:fe9d:f156%ab%64%65", ["zoneId"] = "ab%64%65" }  },
        { TestFileLine(), true, "fe80:0:0:0:204:61ff:fe9d:f156", new() { ["ipv6nz"] = "fe80:0:0:0:204:61ff:fe9d:f156", ["ipv6"] = "fe80:0:0:0:204:61ff:fe9d:f156" }  },
        { TestFileLine(), true, "fe80::204:61ff:fe9d:f156", new() { ["ipv6nz"] = "fe80::204:61ff:fe9d:f156", ["ipv6"] = "fe80::204:61ff:fe9d:f156" }  },
        { TestFileLine(), true, "::1", new() { ["ipv6nz"] = "::1", ["ipv6"] = "::1" }  },
        { TestFileLine(), true, "fe80::", new() { ["ipv6nz"] = "fe80::", ["ipv6"] = "fe80::" }  },
        { TestFileLine(), true, "fe80::1", new() { ["ipv6nz"] = "fe80::1", ["ipv6"] = "fe80::1" }  },
        { TestFileLine(), false, ":", null },
        { TestFileLine(), true, "::ffff:c000:280", new() { ["ipv6nz"] = "::ffff:c000:280", ["ipv6"] = "::ffff:c000:280" }  },

        // Aeron supplied these test cases
        { TestFileLine(), false, "1111:2222:3333:4444::5555:", null },
        { TestFileLine(), false, "1111:2222:3333::5555:", null },
        { TestFileLine(), false, "1111:2222::5555:", null },
        { TestFileLine(), false, "1111::5555:", null },
        { TestFileLine(), false, "::5555:", null },
        { TestFileLine(), false, ":::", null },
        { TestFileLine(), false, "1111:", null },
        { TestFileLine(), false, ":", null },

        { TestFileLine(), false, ":1111:2222:3333:4444::5555", null },
        { TestFileLine(), false, ":1111:2222:3333::5555", null },
        { TestFileLine(), false, ":1111:2222::5555", null },
        { TestFileLine(), false, ":1111::5555", null },
        { TestFileLine(), false, ":::5555", null },
        { TestFileLine(), false, ":::", null },

        // Additional test cases
        // from http://rt.cpan.org/Public/Bug/Display.html?id=50693

        { TestFileLine(), true, "2001:0db8:85a3:0000:0000:8a2e:0370:7334", new() { ["ipv6nz"] = "2001:0db8:85a3:0000:0000:8a2e:0370:7334", ["ipv6"] = "2001:0db8:85a3:0000:0000:8a2e:0370:7334" }  },
        { TestFileLine(), true, "2001:db8:85a3:0:0:8a2e:370:7334", new() { ["ipv6nz"] = "2001:db8:85a3:0:0:8a2e:370:7334", ["ipv6"] = "2001:db8:85a3:0:0:8a2e:370:7334" }  },
        { TestFileLine(), true, "2001:db8:85a3::8a2e:370:7334", new() { ["ipv6nz"] = "2001:db8:85a3::8a2e:370:7334", ["ipv6"] = "2001:db8:85a3::8a2e:370:7334" }  },
        { TestFileLine(), true, "2001:0db8:0000:0000:0000:0000:1428:57ab", new() { ["ipv6nz"] = "2001:0db8:0000:0000:0000:0000:1428:57ab", ["ipv6"] = "2001:0db8:0000:0000:0000:0000:1428:57ab" }  },
        { TestFileLine(), true, "2001:0db8:0000:0000:0000::1428:57ab", new() { ["ipv6nz"] = "2001:0db8:0000:0000:0000::1428:57ab", ["ipv6"] = "2001:0db8:0000:0000:0000::1428:57ab" }  },
        { TestFileLine(), true, "2001:0db8:0:0:0:0:1428:57ab", new() { ["ipv6nz"] = "2001:0db8:0:0:0:0:1428:57ab", ["ipv6"] = "2001:0db8:0:0:0:0:1428:57ab" }  },
        { TestFileLine(), true, "2001:0db8:0:0::1428:57ab", new() { ["ipv6nz"] = "2001:0db8:0:0::1428:57ab", ["ipv6"] = "2001:0db8:0:0::1428:57ab" }  },
        { TestFileLine(), true, "2001:0db8::1428:57ab", new() { ["ipv6nz"] = "2001:0db8::1428:57ab", ["ipv6"] = "2001:0db8::1428:57ab" }  },
        { TestFileLine(), true, "2001:db8::1428:57ab", new() { ["ipv6nz"] = "2001:db8::1428:57ab", ["ipv6"] = "2001:db8::1428:57ab" }  },
        { TestFileLine(), true, "0000:0000:0000:0000:0000:0000:0000:0001", new() { ["ipv6nz"] = "0000:0000:0000:0000:0000:0000:0000:0001", ["ipv6"] = "0000:0000:0000:0000:0000:0000:0000:0001" }  },
        { TestFileLine(), true, "::1", new() { ["ipv6nz"] = "::1", ["ipv6"] = "::1" }  },
        { TestFileLine(), true, "::ffff:0c22:384e", new() { ["ipv6nz"] = "::ffff:0c22:384e", ["ipv6"] = "::ffff:0c22:384e" }  },
        { TestFileLine(), true, "2001:0db8:1234:0000:0000:0000:0000:0000", new() { ["ipv6nz"] = "2001:0db8:1234:0000:0000:0000:0000:0000", ["ipv6"] = "2001:0db8:1234:0000:0000:0000:0000:0000" }  },
        { TestFileLine(), true, "2001:0db8:1234:ffff:ffff:ffff:ffff:ffff", new() { ["ipv6nz"] = "2001:0db8:1234:ffff:ffff:ffff:ffff:ffff", ["ipv6"] = "2001:0db8:1234:ffff:ffff:ffff:ffff:ffff" }  },
        { TestFileLine(), true, "2001:db8:a::123", new() { ["ipv6nz"] = "2001:db8:a::123", ["ipv6"] = "2001:db8:a::123" }  },
        { TestFileLine(), true, "fe80::", new() { ["ipv6nz"] = "fe80::", ["ipv6"] = "fe80::" }  },

        { TestFileLine(), false, "123", null },
        { TestFileLine(), false, "ldkfj", null },
        { TestFileLine(), false, "2001::FFD3::57ab", null },
        { TestFileLine(), false, "2001:db8:85a3::8a2e:37023:7334", null },
        { TestFileLine(), false, "2001:db8:85a3::8a2e:370k:7334", null },
        { TestFileLine(), false, "1:2:3:4:5:6:7:8:9", null },
        { TestFileLine(), false, "1::2::3", null },
        { TestFileLine(), false, "1:::3:4:5", null },
        { TestFileLine(), false, "1:2:3::4:5:6:7:8:9", null },

        // New from Aeron 
        { TestFileLine(), true, "1111:2222:3333:4444:5555:6666:7777:8888", new() { ["ipv6nz"] = "1111:2222:3333:4444:5555:6666:7777:8888", ["ipv6"] = "1111:2222:3333:4444:5555:6666:7777:8888" }  },
        { TestFileLine(), true, "1111:2222:3333:4444:5555:6666:7777::", new() { ["ipv6nz"] = "1111:2222:3333:4444:5555:6666:7777::", ["ipv6"] = "1111:2222:3333:4444:5555:6666:7777::" }  },
        { TestFileLine(), true, "1111:2222:3333:4444:5555:6666::", new() { ["ipv6nz"] = "1111:2222:3333:4444:5555:6666::", ["ipv6"] = "1111:2222:3333:4444:5555:6666::" }  },
        { TestFileLine(), true, "1111:2222:3333:4444:5555::", new() { ["ipv6nz"] = "1111:2222:3333:4444:5555::", ["ipv6"] = "1111:2222:3333:4444:5555::" }  },
        { TestFileLine(), true, "1111:2222:3333:4444::", new() { ["ipv6nz"] = "1111:2222:3333:4444::", ["ipv6"] = "1111:2222:3333:4444::" }  },
        { TestFileLine(), true, "1111:2222:3333::", new() { ["ipv6nz"] = "1111:2222:3333::", ["ipv6"] = "1111:2222:3333::" }  },
        { TestFileLine(), true, "1111:2222::", new() { ["ipv6nz"] = "1111:2222::", ["ipv6"] = "1111:2222::" }  },
        { TestFileLine(), true, "1111::", new() { ["ipv6nz"] = "1111::", ["ipv6"] = "1111::" }  },
        // { TestFileLine(), true, "::", new() { ["ipv6nz"] = "::", ["ipv6"] = "::" }  }, // duplicate
        { TestFileLine(), true, "1111:2222:3333:4444:5555:6666::8888", new() { ["ipv6nz"] = "1111:2222:3333:4444:5555:6666::8888", ["ipv6"] = "1111:2222:3333:4444:5555:6666::8888" }  },
        { TestFileLine(), true, "1111:2222:3333:4444:5555::8888", new() { ["ipv6nz"] = "1111:2222:3333:4444:5555::8888", ["ipv6"] = "1111:2222:3333:4444:5555::8888" }  },
        { TestFileLine(), true, "1111:2222:3333:4444::8888", new() { ["ipv6nz"] = "1111:2222:3333:4444::8888", ["ipv6"] = "1111:2222:3333:4444::8888" }  },
        { TestFileLine(), true, "1111:2222:3333::8888", new() { ["ipv6nz"] = "1111:2222:3333::8888", ["ipv6"] = "1111:2222:3333::8888" }  },
        { TestFileLine(), true, "1111:2222::8888", new() { ["ipv6nz"] = "1111:2222::8888", ["ipv6"] = "1111:2222::8888" }  },
        { TestFileLine(), true, "1111::8888", new() { ["ipv6nz"] = "1111::8888", ["ipv6"] = "1111::8888" }  },
        { TestFileLine(), true, "::8888", new() { ["ipv6nz"] = "::8888", ["ipv6"] = "::8888" }  },
        { TestFileLine(), true, "1111:2222:3333:4444:5555::7777:8888", new() { ["ipv6nz"] = "1111:2222:3333:4444:5555::7777:8888", ["ipv6"] = "1111:2222:3333:4444:5555::7777:8888" }  },
        { TestFileLine(), true, "1111:2222:3333:4444::7777:8888", new() { ["ipv6nz"] = "1111:2222:3333:4444::7777:8888", ["ipv6"] = "1111:2222:3333:4444::7777:8888" }  },
        { TestFileLine(), true, "1111:2222:3333::7777:8888", new() { ["ipv6nz"] = "1111:2222:3333::7777:8888", ["ipv6"] = "1111:2222:3333::7777:8888" }  },
        { TestFileLine(), true, "1111:2222::7777:8888", new() { ["ipv6nz"] = "1111:2222::7777:8888", ["ipv6"] = "1111:2222::7777:8888" }  },
        { TestFileLine(), true, "1111::7777:8888", new() { ["ipv6nz"] = "1111::7777:8888", ["ipv6"] = "1111::7777:8888" }  },
        { TestFileLine(), true, "::7777:8888", new() { ["ipv6nz"] = "::7777:8888", ["ipv6"] = "::7777:8888" }  },
        { TestFileLine(), true, "1111:2222:3333:4444::6666:7777:8888", new() { ["ipv6nz"] = "1111:2222:3333:4444::6666:7777:8888", ["ipv6"] = "1111:2222:3333:4444::6666:7777:8888" }  },
        { TestFileLine(), true, "1111:2222:3333::6666:7777:8888", new() { ["ipv6nz"] = "1111:2222:3333::6666:7777:8888", ["ipv6"] = "1111:2222:3333::6666:7777:8888" }  },
        { TestFileLine(), true, "1111:2222::6666:7777:8888", new() { ["ipv6nz"] = "1111:2222::6666:7777:8888", ["ipv6"] = "1111:2222::6666:7777:8888" }  },
        { TestFileLine(), true, "1111::6666:7777:8888", new() { ["ipv6nz"] = "1111::6666:7777:8888", ["ipv6"] = "1111::6666:7777:8888" }  },
        { TestFileLine(), true, "::6666:7777:8888", new() { ["ipv6nz"] = "::6666:7777:8888", ["ipv6"] = "::6666:7777:8888" }  },
        { TestFileLine(), true, "1111:2222:3333::5555:6666:7777:8888", new() { ["ipv6nz"] = "1111:2222:3333::5555:6666:7777:8888", ["ipv6"] = "1111:2222:3333::5555:6666:7777:8888" }  },
        { TestFileLine(), true, "1111:2222::5555:6666:7777:8888", new() { ["ipv6nz"] = "1111:2222::5555:6666:7777:8888", ["ipv6"] = "1111:2222::5555:6666:7777:8888" }  },
        { TestFileLine(), true, "1111::5555:6666:7777:8888", new() { ["ipv6nz"] = "1111::5555:6666:7777:8888", ["ipv6"] = "1111::5555:6666:7777:8888" }  },
        { TestFileLine(), true, "::5555:6666:7777:8888", new() { ["ipv6nz"] = "::5555:6666:7777:8888", ["ipv6"] = "::5555:6666:7777:8888" }  },
        { TestFileLine(), true, "1111:2222::4444:5555:6666:7777:8888", new() { ["ipv6nz"] = "1111:2222::4444:5555:6666:7777:8888", ["ipv6"] = "1111:2222::4444:5555:6666:7777:8888" }  },
        { TestFileLine(), true, "1111::4444:5555:6666:7777:8888", new() { ["ipv6nz"] = "1111::4444:5555:6666:7777:8888", ["ipv6"] = "1111::4444:5555:6666:7777:8888" }  },
        { TestFileLine(), true, "::4444:5555:6666:7777:8888", new() { ["ipv6nz"] = "::4444:5555:6666:7777:8888", ["ipv6"] = "::4444:5555:6666:7777:8888" }  },
        { TestFileLine(), true, "1111::3333:4444:5555:6666:7777:8888", new() { ["ipv6nz"] = "1111::3333:4444:5555:6666:7777:8888", ["ipv6"] = "1111::3333:4444:5555:6666:7777:8888" }  },
        { TestFileLine(), true, "::3333:4444:5555:6666:7777:8888", new() { ["ipv6nz"] = "::3333:4444:5555:6666:7777:8888", ["ipv6"] = "::3333:4444:5555:6666:7777:8888" }  },
        { TestFileLine(), true, "::2222:3333:4444:5555:6666:7777:8888", new() { ["ipv6nz"] = "::2222:3333:4444:5555:6666:7777:8888", ["ipv6"] = "::2222:3333:4444:5555:6666:7777:8888" }  },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:123.123.123.123", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555::123.123.123.123", null },
        { TestFileLine(), false, "1111:2222:3333:4444::123.123.123.123", null },
        { TestFileLine(), false, "1111:2222:3333::123.123.123.123", null },
        { TestFileLine(), false, "1111:2222::123.123.123.123", null },
        { TestFileLine(), false, "1111::123.123.123.123", null },
        { TestFileLine(), true, "::123.123.123.123", new() { ["ipv6nz"] = "::123.123.123.123", ["ipv6"] = "::123.123.123.123" }  },
        { TestFileLine(), false, "1111:2222:3333:4444::6666:123.123.123.123", null },
        { TestFileLine(), false, "1111:2222:3333::6666:123.123.123.123", null },
        { TestFileLine(), false, "1111:2222::6666:123.123.123.123", null },
        { TestFileLine(), false, "1111::6666:123.123.123.123", null },
        { TestFileLine(), false, "::6666:123.123.123.123", null },
        { TestFileLine(), false, "1111:2222:3333::5555:6666:123.123.123.123", null },
        { TestFileLine(), false, "1111:2222::5555:6666:123.123.123.123", null },
        { TestFileLine(), false, "1111::5555:6666:123.123.123.123", null },
        { TestFileLine(), false, "::5555:6666:123.123.123.123", null },
        { TestFileLine(), false, "1111:2222::4444:5555:6666:123.123.123.123", null },
        { TestFileLine(), false, "1111::4444:5555:6666:123.123.123.123", null },
        { TestFileLine(), false, "::4444:5555:6666:123.123.123.123", null },
        { TestFileLine(), false, "1111::3333:4444:5555:6666:123.123.123.123", null },
        { TestFileLine(), false, "::2222:3333:4444:5555:6666:123.123.123.123", null },

        // Playing with combinations of "0" and "::"
        // NB: these are all sytactically correct, but are bad form 
        // because "0" adjacent to "::" should be combined into "::"
        { TestFileLine(), false, "::0:0:0:0:0:0:0", null },
        { TestFileLine(), false, "::0:0:0:0:0:0", null },
        { TestFileLine(), false, "::0:0:0:0:0", null },
        { TestFileLine(), false, "::0:0:0:0", null },
        { TestFileLine(), false, "::0:0:0", null },
        { TestFileLine(), false, "::0:0", null },
        { TestFileLine(), false, "::0", null },
        { TestFileLine(), false, "0:0:0:0:0:0:0::", null },
        { TestFileLine(), false, "0:0:0:0:0:0::", null },
        { TestFileLine(), false, "0:0:0:0:0::", null },
        { TestFileLine(), false, "0:0:0:0::", null },
        { TestFileLine(), false, "0:0:0::", null },
        { TestFileLine(), false, "0:0::", null },
        { TestFileLine(), false, "0::", null },

        // New invalid from Aeron
        // Invalid data
        { TestFileLine(), false, "XXXX:XXXX:XXXX:XXXX:XXXX:XXXX:XXXX:XXXX", null },

        // Too many components
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:7777:8888:9999", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:7777:8888::", null },
        { TestFileLine(), false, "::2222:3333:4444:5555:6666:7777:8888:9999", null },

        // Too few components
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:7777", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555", null },
        { TestFileLine(), false, "1111:2222:3333:4444", null },
        { TestFileLine(), false, "1111:2222:3333", null },
        { TestFileLine(), false, "1111:2222", null },
        { TestFileLine(), false, "1111", null },

        // Missing :
        { TestFileLine(), false, "11112222:3333:4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, "1111:22223333:4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, "1111:2222:33334444:5555:6666:7777:8888", null },
        { TestFileLine(), false, "1111:2222:3333:44445555:6666:7777:8888", null },
        { TestFileLine(), false, "1111:2222:3333:4444:55556666:7777:8888", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:66667777:8888", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:77778888", null },

        // Missing : intended for ::
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:7777:8888:", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:7777:", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:", null },
        { TestFileLine(), false, "1111:2222:3333:4444:", null },
        { TestFileLine(), false, "1111:2222:3333:", null },
        { TestFileLine(), false, "1111:2222:", null },
        { TestFileLine(), false, "1111:", null },
        { TestFileLine(), false, ":", null },
        { TestFileLine(), false, ":8888", null },
        { TestFileLine(), false, ":7777:8888", null },
        { TestFileLine(), false, ":6666:7777:8888", null },
        { TestFileLine(), false, ":5555:6666:7777:8888", null },
        { TestFileLine(), false, ":4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, ":3333:4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, ":2222:3333:4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, ":1111:2222:3333:4444:5555:6666:7777:8888", null },

        // :::
        { TestFileLine(), false, ":::2222:3333:4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, "1111:::3333:4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, "1111:2222:::4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, "1111:2222:3333:::5555:6666:7777:8888", null },
        { TestFileLine(), false, "1111:2222:3333:4444:::6666:7777:8888", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:::7777:8888", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:::8888", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:7777:::", null },

        // Double ::");
        { TestFileLine(), false, "::2222::4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, "::2222:3333::5555:6666:7777:8888", null },
        { TestFileLine(), false, "::2222:3333:4444::6666:7777:8888", null },
        { TestFileLine(), false, "::2222:3333:4444:5555::7777:8888", null },
        { TestFileLine(), false, "::2222:3333:4444:5555:7777::8888", null },
        { TestFileLine(), false, "::2222:3333:4444:5555:7777:8888::", null },

        { TestFileLine(), false, "1111::3333::5555:6666:7777:8888", null },
        { TestFileLine(), false, "1111::3333:4444::6666:7777:8888", null },
        { TestFileLine(), false, "1111::3333:4444:5555::7777:8888", null },
        { TestFileLine(), false, "1111::3333:4444:5555:6666::8888", null },
        { TestFileLine(), false, "1111::3333:4444:5555:6666:7777::", null },

        { TestFileLine(), false, "1111:2222::4444::6666:7777:8888", null },
        { TestFileLine(), false, "1111:2222::4444:5555::7777:8888", null },
        { TestFileLine(), false, "1111:2222::4444:5555:6666::8888", null },
        { TestFileLine(), false, "1111:2222::4444:5555:6666:7777::", null },

        { TestFileLine(), false, "1111:2222:3333::5555::7777:8888", null },
        { TestFileLine(), false, "1111:2222:3333::5555:6666::8888", null },
        { TestFileLine(), false, "1111:2222:3333::5555:6666:7777::", null },

        { TestFileLine(), false, "1111:2222:3333:4444::6666::8888", null },
        { TestFileLine(), false, "1111:2222:3333:4444::6666:7777::", null },

        { TestFileLine(), false, "1111:2222:3333:4444:5555::7777::", null },

        // Too many components"
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:7777:8888:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:7777:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666::1.2.3.4", null },
        { TestFileLine(), false, "::2222:3333:4444:5555:6666:7777:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:1.2.3.4.5", null },

        // Too few components
        { TestFileLine(), false, "1111:2222:3333:4444:5555:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:4444:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:1.2.3.4", null },
        { TestFileLine(), false, "1111:1.2.3.4", null },
        { TestFileLine(), false, "1.2.3.4", null },

        // Missing :
        { TestFileLine(), false, "11112222:3333:4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:22223333:4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:33334444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:44445555:6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:4444:55556666:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:66661.2.3.4", null },

        // Missing .
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:255255.255.255", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:255.255255.255", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:255.255.255255", null },

        // Missing : intended for ::
        { TestFileLine(), false, ":1.2.3.4", null },
        { TestFileLine(), false, ":6666:1.2.3.4", null },
        { TestFileLine(), false, ":5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":3333:4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":2222:3333:4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222:3333:4444:5555:6666:1.2.3.4", null },

        // :::
        { TestFileLine(), false, ":::2222:3333:4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:::3333:4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:::4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:::5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:4444:::6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:::1.2.3.4", null },

        // Double ::
        { TestFileLine(), false, "::2222::4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "::2222:3333::5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "::2222:3333:4444::6666:1.2.3.4", null },
        { TestFileLine(), false, "::2222:3333:4444:5555::1.2.3.4", null },

        { TestFileLine(), false, "1111::3333::5555:6666:1.2.3.4", null },
        { TestFileLine(), false, "1111::3333:4444::6666:1.2.3.4", null },
        { TestFileLine(), false, "1111::3333:4444:5555::1.2.3.4", null },

        { TestFileLine(), false, "1111:2222::4444::6666:1.2.3.4", null },
        { TestFileLine(), false, "1111:2222::4444:5555::1.2.3.4", null },

        { TestFileLine(), false, "1111:2222:3333::5555::1.2.3.4", null },

        // Missing parts
        { TestFileLine(), false, "::.", null },
        { TestFileLine(), false, "::..", null },
        { TestFileLine(), false, "::...", null },
        { TestFileLine(), false, "::1...", null },
        { TestFileLine(), false, "::1.2..", null },
        { TestFileLine(), false, "::1.2.3.", null },
        { TestFileLine(), false, "::.2..", null },
        { TestFileLine(), false, "::.2.3.", null },
        { TestFileLine(), false, "::.2.3.4", null },
        { TestFileLine(), false, "::..3.", null },
        { TestFileLine(), false, "::..3.4", null },
        { TestFileLine(), false, "::...4", null },

        // Extra : in front
        { TestFileLine(), false, ":1111:2222:3333:4444:5555:6666:7777::", null },
        { TestFileLine(), false, ":1111:2222:3333:4444:5555:6666::", null },
        { TestFileLine(), false, ":1111:2222:3333:4444:5555::", null },
        { TestFileLine(), false, ":1111:2222:3333:4444::", null },
        { TestFileLine(), false, ":1111:2222:3333::", null },
        { TestFileLine(), false, ":1111:2222::", null },
        { TestFileLine(), false, ":1111::", null },
        { TestFileLine(), false, ":::", null },
        { TestFileLine(), false, ":1111:2222:3333:4444:5555:6666::8888", null },
        { TestFileLine(), false, ":1111:2222:3333:4444:5555::8888", null },
        { TestFileLine(), false, ":1111:2222:3333:4444::8888", null },
        { TestFileLine(), false, ":1111:2222:3333::8888", null },
        { TestFileLine(), false, ":1111:2222::8888", null },
        { TestFileLine(), false, ":1111::8888", null },
        { TestFileLine(), false, ":::8888", null },
        { TestFileLine(), false, ":1111:2222:3333:4444:5555::7777:8888", null },
        { TestFileLine(), false, ":1111:2222:3333:4444::7777:8888", null },
        { TestFileLine(), false, ":1111:2222:3333::7777:8888", null },
        { TestFileLine(), false, ":1111:2222::7777:8888", null },
        { TestFileLine(), false, ":1111::7777:8888", null },
        { TestFileLine(), false, ":::7777:8888", null },
        { TestFileLine(), false, ":1111:2222:3333:4444::6666:7777:8888", null },
        { TestFileLine(), false, ":1111:2222:3333::6666:7777:8888", null },
        { TestFileLine(), false, ":1111:2222::6666:7777:8888", null },
        { TestFileLine(), false, ":1111::6666:7777:8888", null },
        { TestFileLine(), false, ":::6666:7777:8888", null },
        { TestFileLine(), false, ":1111:2222:3333::5555:6666:7777:8888", null },
        { TestFileLine(), false, ":1111:2222::5555:6666:7777:8888", null },
        { TestFileLine(), false, ":1111::5555:6666:7777:8888", null },
        { TestFileLine(), false, ":::5555:6666:7777:8888", null },
        { TestFileLine(), false, ":1111:2222::4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, ":1111::4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, ":::4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, ":1111::3333:4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, ":::3333:4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, ":::2222:3333:4444:5555:6666:7777:8888", null },
        { TestFileLine(), false, ":1111:2222:3333:4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222:3333:4444:5555::1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222:3333:4444::1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222:3333::1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222::1.2.3.4", null },
        { TestFileLine(), false, ":1111::1.2.3.4", null },
        { TestFileLine(), false, ":::1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222:3333:4444::6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222:3333::6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222::6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111::6666:1.2.3.4", null },
        { TestFileLine(), false, ":::6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222:3333::5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222::5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111::5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":::5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111:2222::4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111::4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":::4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":1111::3333:4444:5555:6666:1.2.3.4", null },
        { TestFileLine(), false, ":::2222:3333:4444:5555:6666:1.2.3.4", null },

        // Extra : at end
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:7777:::", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666:::", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:::", null },
        { TestFileLine(), false, "1111:2222:3333:4444:::", null },
        { TestFileLine(), false, "1111:2222:3333:::", null },
        { TestFileLine(), false, "1111:2222:::", null },
        { TestFileLine(), false, "1111:::", null },
        { TestFileLine(), false, ":::", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555:6666::8888:", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555::8888:", null },
        { TestFileLine(), false, "1111:2222:3333:4444::8888:", null },
        { TestFileLine(), false, "1111:2222:3333::8888:", null },
        { TestFileLine(), false, "1111:2222::8888:", null },
        { TestFileLine(), false, "1111::8888:", null },
        { TestFileLine(), false, "::8888:", null },
        { TestFileLine(), false, "1111:2222:3333:4444:5555::7777:8888:", null },
        { TestFileLine(), false, "1111:2222:3333:4444::7777:8888:", null },
        { TestFileLine(), false, "1111:2222:3333::7777:8888:", null },
        { TestFileLine(), false, "1111:2222::7777:8888:", null },
        { TestFileLine(), false, "1111::7777:8888:", null },
        { TestFileLine(), false, "::7777:8888:", null },
        { TestFileLine(), false, "1111:2222:3333:4444::6666:7777:8888:", null },
        { TestFileLine(), false, "1111:2222:3333::6666:7777:8888:", null },
        { TestFileLine(), false, "1111:2222::6666:7777:8888:", null },
        { TestFileLine(), false, "1111::6666:7777:8888:", null },
        { TestFileLine(), false, "::6666:7777:8888:", null },
        { TestFileLine(), false, "1111:2222:3333::5555:6666:7777:8888:", null },
        { TestFileLine(), false, "1111:2222::5555:6666:7777:8888:", null },
        { TestFileLine(), false, "1111::5555:6666:7777:8888:", null },
        { TestFileLine(), false, "::5555:6666:7777:8888:", null },
        { TestFileLine(), false, "1111:2222::4444:5555:6666:7777:8888:", null },
        { TestFileLine(), false, "1111::4444:5555:6666:7777:8888:", null },
        { TestFileLine(), false, "::4444:5555:6666:7777:8888:", null },
        { TestFileLine(), false, "1111::3333:4444:5555:6666:7777:8888:", null },
        { TestFileLine(), false, "::3333:4444:5555:6666:7777:8888:", null },
        { TestFileLine(), false, "::2222:3333:4444:5555:6666:7777:8888:", null },

        // Additional cases: http://crisp.tweakblogs.net/blog/2031/ipv6-validation-%28and-caveats%29.html
        { TestFileLine(), true, "0:a:b:c:d:e:f::", new() { ["ipv6nz"] = "0:a:b:c:d:e:f::", ["ipv6"] = "0:a:b:c:d:e:f::" }  },
        { TestFileLine(), true, "::0:a:b:c:d:e:f", new() { ["ipv6nz"] = "::0:a:b:c:d:e:f", ["ipv6"] = "::0:a:b:c:d:e:f" }  }, // syntactically correct, but bad form (::0:... could be combined)
        { TestFileLine(), true, "a:b:c:d:e:f:0::", new() { ["ipv6nz"] = "a:b:c:d:e:f:0::", ["ipv6"] = "a:b:c:d:e:f:0::" }  },
        { TestFileLine(), false, "':10.0.0.1", null },

        { TestFileLine(), false, "::", null },  // Unspecified,RFC4291
        { TestFileLine(), true, "::1", new() { ["ipv6nz"] = "::1", ["ipv6"] = "::1" }  },  // Loopback,RFC4291
        { TestFileLine(), false, "::ffff:192.0.2.128", null },  // IPv4-mapped IPv6,RFC4291
        { TestFileLine(), true, "::ffff:192.2.2.128", new() { ["ipv6nz"] = "::ffff:192.2.2.128", ["ipv6"] = "::ffff:192.2.2.128" }  },  // IPv4-mapped IPv6,RFC4291
        { TestFileLine(), true, "0:0:0:0:0:0:0:1", new() { ["ipv6nz"] = "0:0:0:0:0:0:0:1", ["ipv6"] = "0:0:0:0:0:0:0:1" }  },  // Loopback,RFC4291
        { TestFileLine(), true, "1050:0:0:0:5:600:300c:1", new() { ["ipv6nz"] = "1050:0:0:0:5:600:300c:1", ["ipv6"] = "1050:0:0:0:5:600:300c:1" }  },  // Global Unicast,RFC2373
        { TestFileLine(), true, "2001::1", new() { ["ipv6nz"] = "2001::1", ["ipv6"] = "2001::1" }  },  // Global Unicast,RFC4291
        { TestFileLine(), true, "2001:0:9D38:953C:10EF:EE22:FFDD:AABB", new() { ["ipv6nz"] = "2001:0:9D38:953C:10EF:EE22:FFDD:AABB", ["ipv6"] = "2001:0:9D38:953C:10EF:EE22:FFDD:AABB" }  },  // Global Unicast,RFC3587
        { TestFileLine(), true, "2001:0DA8:0200:0012:0000:00B8:0000:02AA", new() { ["ipv6nz"] = "2001:0DA8:0200:0012:0000:00B8:0000:02AA", ["ipv6"] = "2001:0DA8:0200:0012:0000:00B8:0000:02AA" }  },  // Global Unicast,RFC4291
        { TestFileLine(), true, "2001:0db8::1", new() { ["ipv6nz"] = "2001:0db8::1", ["ipv6"] = "2001:0db8::1" }  },  // Documentation,RFC3849
        { TestFileLine(), true, "2001:0db8::1:0:0:1", new() { ["ipv6nz"] = "2001:0db8::1:0:0:1", ["ipv6"] = "2001:0db8::1:0:0:1" }  },  // Documentation,RFC4291
        { TestFileLine(), true, "2001:0DB8::4152:EBAF:CE01:0001", new() { ["ipv6nz"] = "2001:0DB8::4152:EBAF:CE01:0001", ["ipv6"] = "2001:0DB8::4152:EBAF:CE01:0001" }  },  // Global Unicast,RFC4291
        { TestFileLine(), true, "2001:0db8:0:0:1:0:0:1", new() { ["ipv6nz"] = "2001:0db8:0:0:1:0:0:1", ["ipv6"] = "2001:0db8:0:0:1:0:0:1" }  },  // Documentation,RFC4291
        { TestFileLine(), true, "2001:0DB8:0000:CD30:0000:0000:0000:0000", new() { ["ipv6nz"] = "2001:0DB8:0000:CD30:0000:0000:0000:0000", ["ipv6"] = "2001:0DB8:0000:CD30:0000:0000:0000:0000" }  },  // Global Unicast,RFC4291
        { TestFileLine(), true, "2001:0DB8:1234:5678:ABCD:EF01:2345:6789", new() { ["ipv6nz"] = "2001:0DB8:1234:5678:ABCD:EF01:2345:6789", ["ipv6"] = "2001:0DB8:1234:5678:ABCD:EF01:2345:6789" }  },  // Global Unicast,RFC4291
        { TestFileLine(), true, "2001:0db8:85a3:0000:0000:8a2e:0370:7334", new() { ["ipv6nz"] = "2001:0db8:85a3:0000:0000:8a2e:0370:7334", ["ipv6"] = "2001:0db8:85a3:0000:0000:8a2e:0370:7334" }  },  // Global Unicast,RFC2373
        { TestFileLine(), true, "2001:0db8:85a3:08d3:1319:8a2e:0370:7344", new() { ["ipv6nz"] = "2001:0db8:85a3:08d3:1319:8a2e:0370:7344", ["ipv6"] = "2001:0db8:85a3:08d3:1319:8a2e:0370:7344" }  },  // Global Unicast,RFC2373
        { TestFileLine(), true, "2001:0DB8:aaaa:0007:0000:0000:0000:0001", new() { ["ipv6nz"] = "2001:0DB8:aaaa:0007:0000:0000:0000:0001", ["ipv6"] = "2001:0DB8:aaaa:0007:0000:0000:0000:0001" }  },  // Global Unicast,RFC2373
        { TestFileLine(), true, "2001:2::10", new() { ["ipv6nz"] = "2001:2::10", ["ipv6"] = "2001:2::10" }  },  // Global Unicast,RFC4291
        { TestFileLine(), true, "2001:44b8:4126:f600:91bd:970c:9073:12df", new() { ["ipv6nz"] = "2001:44b8:4126:f600:91bd:970c:9073:12df", ["ipv6"] = "2001:44b8:4126:f600:91bd:970c:9073:12df" }  },  // Global Unicast,RIPE NCC (Europe) [RFC 4193]
        { TestFileLine(), true, "2001:4860:4860::8888", new() { ["ipv6nz"] = "2001:4860:4860::8888", ["ipv6"] = "2001:4860:4860::8888" }  },  // Global Unicast,"Google, Inc. [RFC 7607]", ["ipv6"] = "2001:4860:4860::8888" }  },  // Global Unicast,"Google, Inc. [RFC 7607]"
        { TestFileLine(), true, "2001:500:2d::d", new() { ["ipv6nz"] = "2001:500:2d::d", ["ipv6"] = "2001:500:2d::d" }  },  // Anycast,IANA [RFC 7526]
        { TestFileLine(), true, "2001:558:fc03:11:5e63:3eff:fe67:edf9", new() { ["ipv6nz"] = "2001:558:fc03:11:5e63:3eff:fe67:edf9", ["ipv6"] = "2001:558:fc03:11:5e63:3eff:fe67:edf9" }  },  // Global Unicast,Akamai Technologies
        { TestFileLine(), true, "2001:acad:abad:1::bc", new() { ["ipv6nz"] = "2001:acad:abad:1::bc", ["ipv6"] = "2001:acad:abad:1::bc" }  },  // Unique Local Unicast,Private-Use [RFC 4193]
        { TestFileLine(), true, "2001:b50:ffd3:76:ce58:32ff:fe00:e7", new() { ["ipv6nz"] = "2001:b50:ffd3:76:ce58:32ff:fe00:e7", ["ipv6"] = "2001:b50:ffd3:76:ce58:32ff:fe00:e7" }  },  // Global Unicast,Sprint [RFC 3627]
        { TestFileLine(), true, "2001:db8::0:1:0:0:1", new() { ["ipv6nz"] = "2001:db8::0:1:0:0:1", ["ipv6"] = "2001:db8::0:1:0:0:1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8::1", new() { ["ipv6nz"] = "2001:db8::1", ["ipv6"] = "2001:db8::1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8::1:0:0:1", new() { ["ipv6nz"] = "2001:db8::1:0:0:1", ["ipv6"] = "2001:db8::1:0:0:1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8::212:7403:ace0:1", new() { ["ipv6nz"] = "2001:db8::212:7403:ace0:1", ["ipv6"] = "2001:db8::212:7403:ace0:1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:DB8::4:5:6:7", new() { ["ipv6nz"] = "2001:DB8::4:5:6:7", ["ipv6"] = "2001:DB8::4:5:6:7" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8::5", new() { ["ipv6nz"] = "2001:db8::5", ["ipv6"] = "2001:db8::5" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:DB8::8:800:200C:417A", new() { ["ipv6nz"] = "2001:DB8::8:800:200C:417A", ["ipv6"] = "2001:DB8::8:800:200C:417A" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8::aaaa:0:0:1", new() { ["ipv6nz"] = "2001:db8::aaaa:0:0:1", ["ipv6"] = "2001:db8::aaaa:0:0:1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8:0::1", new() { ["ipv6nz"] = "2001:db8:0::1", ["ipv6"] = "2001:db8:0::1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8:0:0::1", new() { ["ipv6nz"] = "2001:db8:0:0::1", ["ipv6"] = "2001:db8:0:0::1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8:0:0:0::1", new() { ["ipv6nz"] = "2001:db8:0:0:0::1", ["ipv6"] = "2001:db8:0:0:0::1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8:0:0:1::1", new() { ["ipv6nz"] = "2001:db8:0:0:1::1", ["ipv6"] = "2001:db8:0:0:1::1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:DB8:0:0:1::1", new() { ["ipv6nz"] = "2001:DB8:0:0:1::1", ["ipv6"] = "2001:DB8:0:0:1::1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:db8:0:0:1:0:0:1", new() { ["ipv6nz"] = "2001:db8:0:0:1:0:0:1", ["ipv6"] = "2001:db8:0:0:1:0:0:1" }  },  // Documentation,Private-Use [RFC 3849]
        { TestFileLine(), true, "2001:DB8:0:0:8:800:200C:417A", new() { ["ipv6nz"] = "2001:DB8:0:0:8:800:200C:417A", ["ipv6"] = "2001:DB8:0:0:8:800:200C:417A" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:0:0:aaaa::1", new() { ["ipv6nz"] = "2001:db8:0:0:aaaa::1", ["ipv6"] = "2001:db8:0:0:aaaa::1" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:0000:0:1::1", new() { ["ipv6nz"] = "2001:db8:0000:0:1::1", ["ipv6"] = "2001:db8:0000:0:1::1" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:3c4d:15::1", new() { ["ipv6nz"] = "2001:db8:3c4d:15::1", ["ipv6"] = "2001:db8:3c4d:15::1" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:DB8:85A3::8A2E:370:7334", new() { ["ipv6nz"] = "2001:DB8:85A3::8A2E:370:7334", ["ipv6"] = "2001:DB8:85A3::8A2E:370:7334" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:aaaa:bbbb:cccc:dddd::1", new() { ["ipv6nz"] = "2001:db8:aaaa:bbbb:cccc:dddd::1", ["ipv6"] = "2001:db8:aaaa:bbbb:cccc:dddd::1" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:aaaa:bbbb:cccc:dddd:0:1", new() { ["ipv6nz"] = "2001:db8:aaaa:bbbb:cccc:dddd:0:1", ["ipv6"] = "2001:db8:aaaa:bbbb:cccc:dddd:0:1" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:aaaa:bbbb:cccc:dddd:eeee:0001", new() { ["ipv6nz"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:0001", ["ipv6"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:0001" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:aaaa:bbbb:cccc:dddd:eeee:001", new() { ["ipv6nz"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:001", ["ipv6"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:001" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:aaaa:bbbb:cccc:dddd:eeee:01", new() { ["ipv6nz"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:01", ["ipv6"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:01" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:aaaa:bbbb:cccc:dddd:eeee:1", new() { ["ipv6nz"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:1", ["ipv6"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:1" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:aaaa:bbbb:cccc:dddd:eeee:aaaa", new() { ["ipv6nz"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:aaaa", ["ipv6"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:aaaa" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:aaaa:bbbb:cccc:dddd:eeee:AAAA", new() { ["ipv6nz"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:AAAA", ["ipv6"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:AAAA" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:aaaa:bbbb:cccc:dddd:eeee:AaAa", new() { ["ipv6nz"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:AaAa", ["ipv6"] = "2001:db8:aaaa:bbbb:cccc:dddd:eeee:AaAa" }  },  // Unicast address,Reserved for Documentation [RFC 5952]
        { TestFileLine(), true, "2001:db8:d03:bd70:fede:5c4d:8969:12c4", new() { ["ipv6nz"] = "2001:db8:d03:bd70:fede:5c4d:8969:12c4", ["ipv6"] = "2001:db8:d03:bd70:fede:5c4d:8969:12c4" }  },  // Unicast address,Reserved for Documentation [RFC 4291]
        { TestFileLine(), true, "2002::8364:7777", new() { ["ipv6nz"] = "2002::8364:7777", ["ipv6"] = "2002::8364:7777" }  },  // 6to4 address,Allocated to 6to4 [RFC 3056]
        { TestFileLine(), true, "2002:4559:1FE2::4559:1FE2", new() { ["ipv6nz"] = "2002:4559:1FE2::4559:1FE2", ["ipv6"] = "2002:4559:1FE2::4559:1FE2" }  },  // 6to4 address,Allocated to 6to4 [RFC 3056]
        { TestFileLine(), true, "2002:C000:203:200::", new() { ["ipv6nz"] = "2002:C000:203:200::", ["ipv6"] = "2002:C000:203:200::" }  },  // 6to4 address,Allocated to 6to4 [RFC 3056]
        { TestFileLine(), true, "2002:cb0a:3cdd:1:1:1:1:1", new() { ["ipv6nz"] = "2002:cb0a:3cdd:1:1:1:1:1", ["ipv6"] = "2002:cb0a:3cdd:1:1:1:1:1" }  },  // 6to4 Relay anycast address,Allocated to 6to4 Anycast Relays [RFC 6343]
        { TestFileLine(), true, "2400:8902::f03c:92ff:feb5:f66d", new() { ["ipv6nz"] = "2400:8902::f03c:92ff:feb5:f66d", ["ipv6"] = "2400:8902::f03c:92ff:feb5:f66d" }  },  // Unicast address,APNIC
        { TestFileLine(), true, "2400:c980:0:e206:b07d:8cf9:2b05:fb06", new() { ["ipv6nz"] = "2400:c980:0:e206:b07d:8cf9:2b05:fb06", ["ipv6"] = "2400:c980:0:e206:b07d:8cf9:2b05:fb06" }  },  // Unicast,APNIC
        { TestFileLine(), true, "2400:cb00:2048:1::6814:507", new() { ["ipv6nz"] = "2400:cb00:2048:1::6814:507", ["ipv6"] = "2400:cb00:2048:1::6814:507" }  },  // Anycast,RIPE NCC
        { TestFileLine(), true, "2404:6800:4009:805::2004", new() { ["ipv6nz"] = "2404:6800:4009:805::2004", ["ipv6"] = "2404:6800:4009:805::2004" }  },  // Unicast,Google
        { TestFileLine(), true, "2607:f8b0:4005:80b::200e", new() { ["ipv6nz"] = "2607:f8b0:4005:80b::200e", ["ipv6"] = "2607:f8b0:4005:80b::200e" }  },  // Unicast,Google
        { TestFileLine(), true, "2607:f8b0:400a:809::200e", new() { ["ipv6nz"] = "2607:f8b0:400a:809::200e", ["ipv6"] = "2607:f8b0:400a:809::200e" }  },  // Unicast,Google
        { TestFileLine(), true, "2620:0:1cfe:face:b00c::3", new() { ["ipv6nz"] = "2620:0:1cfe:face:b00c::3", ["ipv6"] = "2620:0:1cfe:face:b00c::3" }  },  // Unicast,ARIN
        { TestFileLine(), true, "2620:0:2d0:200::7", new() { ["ipv6nz"] = "2620:0:2d0:200::7", ["ipv6"] = "2620:0:2d0:200::7" }  },  // Unicast,ARIN
        { TestFileLine(), true, "3fff:ffff:3:1:0:0:0:7", new() { ["ipv6nz"] = "3fff:ffff:3:1:0:0:0:7", ["ipv6"] = "3fff:ffff:3:1:0:0:0:7" }  },  // Unicast,IANA
        { TestFileLine(), true, "ABCD:EF01:2345:6789:ABCD:EF01:2345:6789", new() { ["ipv6nz"] = "ABCD:EF01:2345:6789:ABCD:EF01:2345:6789", ["ipv6"] = "ABCD:EF01:2345:6789:ABCD:EF01:2345:6789" }  },  // Unicast,RFC 4291
        { TestFileLine(), true, "abcd:ef01:2345:6789:abcd:ef01:2345:6789%1", new() { ["ipv6nz"] = "abcd:ef01:2345:6789:abcd:ef01:2345:6789", ["ipv6"] = "abcd:ef01:2345:6789:abcd:ef01:2345:6789%1", ["zoneId"] = "1" } },  // RFC 4007 Section 11 - Unsure if global address can be used
        { TestFileLine(), true, "fc00::", new() { ["ipv6nz"] = "fc00::", ["ipv6"] = "fc00::" }  },  // Unicast,RFC 4193
        { TestFileLine(), true, "fd3b:d101:e37f:9713::1", new() { ["ipv6nz"] = "fd3b:d101:e37f:9713::1", ["ipv6"] = "fd3b:d101:e37f:9713::1" }  },  // Unicast,Unique Local
        { TestFileLine(), true, "fd44:a77b:40ca:db17:37df:f4c4:f38a:fc81", new() { ["ipv6nz"] = "fd44:a77b:40ca:db17:37df:f4c4:f38a:fc81", ["ipv6"] = "fd44:a77b:40ca:db17:37df:f4c4:f38a:fc81" }  },  // Unicast,ISP
        { TestFileLine(), true, "FD92:7065:891e:8c71:d2e7:d3f3:f595:d7d8%tun0", new() { ["ipv6nz"] = "FD92:7065:891e:8c71:d2e7:d3f3:f595:d7d8", ["ipv6"] = "FD92:7065:891e:8c71:d2e7:d3f3:f595:d7d8%tun0", ["zoneId"] = "tun0", } },  // Unicast,RFC 4007 Section 11
        { TestFileLine(), true, "fe80::", new() { ["ipv6nz"] = "fe80::", ["ipv6"] = "fe80::" }  },  // Link-Local,RFC 4291
        { TestFileLine(), true, "fe80::cd8:95bf:afbb:9622%eth0", new() { ["ipv6nz"] = "fe80::cd8:95bf:afbb:9622", ["ipv6"] = "fe80::cd8:95bf:afbb:9622%eth0", ["zoneId"] = "eth0", } },  // Link-Local,RFC 4007
        { TestFileLine(), true, "FE80:0000:0000:0000:0202:B3FF:FE1E:8329", new() { ["ipv6nz"] = "FE80:0000:0000:0000:0202:B3FF:FE1E:8329", ["ipv6"] = "FE80:0000:0000:0000:0202:B3FF:FE1E:8329" }  },  // Link-Local,RFC 2373
        { TestFileLine(), true, "FE80:0000:0000:0000:0202:B3FF:FE1E:8329%eth0", new() { ["ipv6nz"] = "FE80:0000:0000:0000:0202:B3FF:FE1E:8329", ["ipv6"] = "FE80:0000:0000:0000:0202:B3FF:FE1E:8329%eth0", ["zoneId"] = "eth0", } },  // Link-Local,RFC 4007 Section 11
        { TestFileLine(), true, "fe80:dead:beef:cafe:face:feed:f12d:bedd%2", new() { ["ipv6nz"] = "fe80:dead:beef:cafe:face:feed:f12d:bedd", ["ipv6"] = "fe80:dead:beef:cafe:face:feed:f12d:bedd%2", ["zoneId"] = "2", } },  // Link-Local,RFC 4007 Section 11
        { TestFileLine(), true, "fec0:0:0:1::1", new() { ["ipv6nz"] = "fec0:0:0:1::1", ["ipv6"] = "fec0:0:0:1::1" }  }, //  Deprecated,Site-Local Unicast,RFC 3879/ RFC 4193 (deprecated by RFC 4291)
        { TestFileLine(), true, "FEDC:BA98:7654:3210:FEDC:BA98:7654:3210", new() { ["ipv6nz"] = "FEDC:BA98:7654:3210:FEDC:BA98:7654:3210", ["ipv6"] = "FEDC:BA98:7654:3210:FEDC:BA98:7654:3210" }  },  // Global Unicast,IANA
        { TestFileLine(), true, "FF01::101", new() { ["ipv6nz"] = "FF01::101", ["ipv6"] = "FF01::101" }  },  // Multicast (Reserved),RFC4291 Section 2.7
        { TestFileLine(), true, "FF01:0:0:0:0:0:0:1", new() { ["ipv6nz"] = "FF01:0:0:0:0:0:0:1", ["ipv6"] = "FF01:0:0:0:0:0:0:1" }  },  // Multicast (Reserved),RFC4291 Section 2.7
        { TestFileLine(), true, "FF01:0:0:0:0:0:0:101", new() { ["ipv6nz"] = "FF01:0:0:0:0:0:0:101", ["ipv6"] = "FF01:0:0:0:0:0:0:101" }  },  // Multicast (Reserved),RFC4291 Section 2.7
        { TestFileLine(), true, "FF02::1", new() { ["ipv6nz"] = "FF02::1", ["ipv6"] = "FF02::1" }  },  // Link-Local Multicast,IANA RFC4291 Section 2.7
        { TestFileLine(), true, "FF02:0:0:0:0:0:0:1", new() { ["ipv6nz"] = "FF02:0:0:0:0:0:0:1", ["ipv6"] = "FF02:0:0:0:0:0:0:1" }  },  // Link-Local Multicast,IANA RFC4291 Section 2.7
        { TestFileLine(), true, "FF02:0:0:0:0:0:0:a", new() { ["ipv6nz"] = "FF02:0:0:0:0:0:0:a", ["ipv6"] = "FF02:0:0:0:0:0:0:a" }  },  // Link-Local Multicast,IANA RFC7346 Section 2.1
        { TestFileLine(), true, "FF05:15:25:df:20:4a:b4:24", new() { ["ipv6nz"] = "FF05:15:25:df:20:4a:b4:24", ["ipv6"] = "FF05:15:25:df:20:4a:b4:24" }  },  // Link-Local Multicast,IANA RFC4291 Section 2.7
        { TestFileLine(), true, "FF08:0:0:0:0:0:0:fc", new() { ["ipv6nz"] = "FF08:0:0:0:0:0:0:fc", ["ipv6"] = "FF08:0:0:0:0:0:0:fc" }  },  // Reserved for future use (Reserved),IANA RFC4291 Section 2.8
        { TestFileLine(), false, "::-1", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""-"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "::/0/0", null },  // RFC4291 - IPv6 address only request should not match CIDR notation.
        { TestFileLine(), false, "::%eth0", null },  // RFC4007 Section 11 - IPv6 addressing architecture allows the link-local scope to be delimited using a zone index suffix.This is not a link local context.
        { TestFileLine(), true, "::10.0.0.1", new() { ["ipv6nz"] = "::10.0.0.1", ["ipv6"] = "::10.0.0.1" }  },  //  RFC 4291 2.5.5.2 - IPv4 mapped addresses in IPv6 always begin with the prefix ::ffff: followed by the IPv4 address.
        { TestFileLine(), false, "::255.255.255.255", null },  //  RFC 4291 2.5.5.2 - IPv4 mapped addresses in IPv6 always begin with the prefix ::ffff: followed by the IPv4 address.
        { TestFileLine(), false, "::ffff:0.0.0.256", null },  //  RFC 4291 2.5.5.2 - IPv4-mapped IPv6 address - The IPv4 Section of the address contains an invalid octet.
        { TestFileLine(), false, "::ffff:127.0.0.1/96", null },  //  RFC4291 - IPv6 address only request should not match CIDR notation.
        { TestFileLine(), false, "::ffff:192.0.2.128/33", null },  //  RFC4291 - IPv6 address only request should not match CIDR notation.
        { TestFileLine(), false, "::ffff:192.0.2.256", null },  //  RFC 4291 2.5.5.2 - IPv4-mapped IPv6 address - The IPv4 Section of the address contains an invalid octet.
        { TestFileLine(), false, "::ffff:192.168.1.256", null },  //  RFC 4291 2.5.5.2 - IPv4-mapped IPv6 address - The IPv4 Section of the address contains an invalid octet.
        { TestFileLine(), true, "1080:0:0:0:8:800:200C:417", new() { ["ipv6nz"] = "1080:0:0:0:8:800:200C:417", ["ipv6"] = "1080:0:0:0:8:800:200C:417" }  },
        { TestFileLine(), true, "2001:cdba:0000:0000:0000:0000:3257:9652%4294967295", new() { ["ipv6nz"] = "2001:cdba:0000:0000:0000:0000:3257:9652", ["ipv6"] = "2001:cdba:0000:0000:0000:0000:3257:9652%4294967295", ["zoneId"] = "4294967295", } }, //  RFC4007 - IPv6 addresses may include a zone ID
        { TestFileLine(), true, "0:0:0:0:0:0:192.168.0.1", new() { ["ipv6nz"] = "0:0:0:0:0:0:192.168.0.1", ["ipv6"] = "0:0:0:0:0:0:192.168.0.1" }  },  //  RFC 4291 2.5.5.2 - IPv4 mapped addresses in IPv6 always begin with the prefix ::ffff: followed by the IPv4 address.Too few IPv6 groups.
        { TestFileLine(), false, "1:2:3:4:5:6:7:8:9", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups."
        { TestFileLine(), false, "1080:0:0:0:0:0:0:192.88.99", null },  //  RFC 4291 2.5.5.2 - IPv4 mapped addresses in IPv6 always begin with the prefix ::ffff: followed by the IPv4 address.
        { TestFileLine(), false, "2001::0223:dead:beef::1", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups due to extraneous ""::""."
        { TestFileLine(), false, "2001::192.168.0.1", null },  //  RFC 4291 2.5.5.2 - IPv4 mapped addresses in IPv6 always begin with the prefix ::ffff: followed by the IPv4 address.
        { TestFileLine(), false, "2001::dead::beef", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups due to extraneous ""::""."
        { TestFileLine(), false, "2001::ff4:2:1:1:1:1:1", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups."
        { TestFileLine(), false, "2001:0DB8:0:CD3", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Few Groups."
        { TestFileLine(), false, "2001:0db8:1234:5678:90AB:CDEF:0012:3456:789a", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups."
        { TestFileLine(), false, "2001:10:240:ab::192.19.24.1", null },  //  RFC 4291 2.5.5.2 - IPv4 mapped addresses in IPv6 always begin with the prefix ::ffff: followed by the IPv4 address.
        { TestFileLine(), false, "2001:db8:::1:0", null },  //  RFC4291 - This address is invalid as it contains consecutive colons that do not represent the elision of any parts of the address.
        { TestFileLine(), false, "2001:db8::1 ::2", null },  //  RFC4291 - This address is invalid as it contains consecutive colons that do not represent the elision of any parts of the address.
        { TestFileLine(), false, "2001:db8::192.168.0.1", null },  //  RFC 4291 2.5.5.2 - IPv4 mapped addresses in IPv6 always begin with the prefix ::ffff: followed by the IPv4 address.
        { TestFileLine(), false, "2001:db8:/60", null },  //  RFC4291 - IPv6 address only request should not match CIDR notation.
        { TestFileLine(), false, "2001:db8:0:0:0:0:0/64", null },  //  RFC4291 - IPv6 address only request should not match CIDR notation.
        { TestFileLine(), false, "2001:db8:0:0:0:0:f:1g", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "2001:db8:0:0:0g00:1428:57ab", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "2001:db8:0:1:::1", null },  // "RFC2373 - Too many colons, only one set of ""::"" may exist and no more than 2 colons may exist consecutively in a valid address."
        { TestFileLine(), false, "2001:db8:0:1::/129", null },  //  RFC4291 - The netmask length (/129) exceeds 128. Addresses should also not match CIDR notation.
        { TestFileLine(), false, "2001:db8:0:1::1::1", null },  // "RFC2373 - Too many colons, only one set of ""::"" may exist and no more than 2 colons may exist consecutively in a valid address."
        { TestFileLine(), false, "2001:db8:0:1::192.0.2.128", null },  // RFC 4291 2.5.5.2 - IPv4 mapped addresses in IPv6 always begin with the prefix ::ffff: followed by the IPv4 address.
        { TestFileLine(), false, "2001:db8:0:1::a:b:c:d:e:f", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups."
        { TestFileLine(), false, "2001:db8:0:1:/64", null },  //  RFC4291 - IPv6 address only request should not match CIDR notation.
        { TestFileLine(), false, "2001:db8:0:1:1:1:1::1", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups due to extraneous ""::""."
        { TestFileLine(), false, "2001:db8:0:1:1:1:1:1:1", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups."
        { TestFileLine(), false, "2001:db8:0:1:1:1:1:1#test", null },  // "RFC6874 - The ""#"" character is not permitted in an IPv6 address"
        { TestFileLine(), false, "2001:dh8:0:1:1:1:256:1", null },  //  RFC4291 - Octets should only contain values from 0 to FF (hex) or 0 to 255 (decimal)
        { TestFileLine(), true, "2001:db8:0:1:1:1:256:1", new() { ["ipv6nz"] = "2001:db8:0:1:1:1:256:1", ["ipv6"] = "2001:db8:0:1:1:1:256:1" }  },  //  RFC4291 - Octets should only contain values from 0 to FF (hex) or 0 to 255 (decimal)
        { TestFileLine(), false, "2001:db8:0:1g:0:0:0:1", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "2001:db8:aaaa:bbbb:cccc:dddd-eeee:ffff", null },  // "RFC8200 - IPv6 address contains invalid character ""-"" in between segments."
        { TestFileLine(), false, "2001:db8:aaaa:bbbb:cccc:dddd-eeee:ffff", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. The hyphen character (""-"") is not permitted in an IPv6 address."
        { TestFileLine(), false, "2001:dg8:0:0:0:0:1428:57ab", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "2001:dg8:0:0:0:0:1428:57ab", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "2001:gdba:0000:0000:0000:0000:3257:9652", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "2001:gdba:0000:0000:0000:0000:3257:9652", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "2001:ggg:0:0:0:0:1428:57ab", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "2001:ggg:0:0:0:0:1428:57ab", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "2001.x:0:0:0:0:0:0:1", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. The ""."" character is not permitted in an IPv6 address."
        { TestFileLine(), false, "20011:db8:0:1:1:1:1:1", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too many characters in first group."
        { TestFileLine(), false, "2403:780:f:102:a:a:1:0:0", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups."
        { TestFileLine(), false, "2403:780:f:102:a:a:1:0:0", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups."
        { TestFileLine(), false, "2403:780:f:102:g:a:1:0", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Many Groups."
        { TestFileLine(), false, "2403:780:f:102:g:a:1:0", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. ""g"" is not a valid hexadecimal digit"
        { TestFileLine(), false, "260.02:00a:b:10:abc:def:123f:2552", null },  // "RFC4291 Section 2.2 - The ""::"" can only appear once in an address. RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons."
        { TestFileLine(), false, "260.02:00a:b:10:abc:def:123f:2552", null },  //  RFC4291 - Octets should only contain values from 0 to FF (hex) or 0 to 255 (decimal)
        { TestFileLine(), false, "fe80:::1", null },  // "RFC2373 - Too many colons, only one set of ""::"" may exist and no more than 2 colons may exist consecutively in a valid address."
        { TestFileLine(), false, "fe80:::1", null },  // "RFC2373 - Too many colons, only one set of ""::"" may exist and no more than 2 colons may exist consecutively in a valid address."
        { TestFileLine(), false, "fe80::/130", null },  //  RFC4291 - /130 is not allowed in IPv6 address space and IPv6 address only request should not match CIDR notation.
        { TestFileLine(), false, "fe80::/130", null },  //  RFC4291 - IPv6 address only request should not match CIDR notation.
        { TestFileLine(), false, "fe80::7::8", null },  // "RFC2373 - Too many colons, only one set of ""::"" may exist and no more than 2 colons may exist consecutively in a valid address."
        { TestFileLine(), false, "fe80::7::8", null },  // "RFC2373 - Too many colons, only one set of ""::"" may exist and no more than 2 colons may exist consecutively in a valid address."
        { TestFileLine(), false, "2001:0DB8:0:CD3", null },  // "RFC4291 Section 2.2 specifies that an IPv6 address should be written as eight groups of four hexadecimal digits, separated by colons. Too Few Groups."    };
    };
}
