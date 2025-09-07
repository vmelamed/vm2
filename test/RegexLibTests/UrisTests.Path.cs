namespace vm2.RegexLib.Tests;

public partial class UrisTests
{
    public static UriTheoryData PathAbsData = new () {
        { TestFileLine(), true, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), true, "aaa", new() {
            ["path"] = "aaa",
            ["pathNoScheme"] = "aaa",
        } },
        { TestFileLine(), true, "a:a", new() {
            ["path"] = "a:a",
            ["pathRootless"] = "a:a",
        } },
        { TestFileLine(), false, "\\aaa", null },
        { TestFileLine(), false, "?aaa", null },
        { TestFileLine(), true, ":aaa", new() {
            ["path"] = ":aaa",
            ["pathRootless"] = ":aaa",
        } },
        { TestFileLine(), true, "123", new() {
            ["path"] = "123",
            ["pathNoScheme"] = "123",
        } },
        { TestFileLine(), true, "123", new() {
            ["path"] = "123",
            ["pathNoScheme"] = "123",
        } },
        { TestFileLine(), true, "%D0%B4%D0%B8%D1%80", new() {
            ["path"] = "%D0%B4%D0%B8%D1%80", // →path← = →дир←
            ["pathNoScheme"] = "%D0%B4%D0%B8%D1%80", // →pathNoScheme← = →дир←
        } },
        { TestFileLine(), true, "/", new() {
            ["path"] = "/",
            ["pathAbsEmpty"] = "/",
        } },
        { TestFileLine(), true, "/aaa", new() {
            ["path"] = "/aaa",
            ["pathAbsEmpty"] = "/aaa",
        } },
        { TestFileLine(), true, "/12a.a@tyty", new() {
            ["path"] = "/12a.a@tyty",
            ["pathAbsEmpty"] = "/12a.a@tyty",
        } },
        { TestFileLine(), false, "/12aa#tyty", null },
        { TestFileLine(), true, "/12a/a@tyty", new() {
            ["path"] = "/12a/a@tyty",
            ["pathAbsEmpty"] = "/12a/a@tyty",
        } },
        { TestFileLine(), true, "/12a/a:tyty", new() {
            ["path"] = "/12a/a:tyty",
            ["pathAbsEmpty"] = "/12a/a:tyty",
        } },
        { TestFileLine(), true, "/12a/a@tyty", new() {
            ["path"] = "/12a/a@tyty",
            ["pathAbsEmpty"] = "/12a/a@tyty",
        } },
        { TestFileLine(), true, "/c:/12a/a@tyty", new() {
            ["path"] = "/c:/12a/a@tyty",
            ["pathAbsEmpty"] = "/c:/12a/a@tyty",
        } },
        { TestFileLine(), true, "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3", new() {
            ["path"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
            ["pathAbsEmpty"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
        } },
        { TestFileLine(), true, "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3/123", new() {
            ["path"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3/123",
            ["pathAbsEmpty"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3/123",
        } },
        { TestFileLine(), true, "//aaa", new() {
            ["path"] = "//aaa",
            ["pathAbsEmpty"] = "//aaa",
        } },
        { TestFileLine(), true, "////aaa", new() {
            ["path"] = "////aaa",
            ["pathAbsEmpty"] = "////aaa",
        } },
        { TestFileLine(), true, "/", new() {
            ["path"] = "/",
            ["pathAbsEmpty"] = "/",
        } },
        { TestFileLine(), true, "/bbb///aaa", new() {
            ["path"] = "/bbb///aaa",
            ["pathAbsEmpty"] = "/bbb///aaa",
        } },
    };

    public static UriTheoryData GeneralQueryData = new (){
        { TestFileLine(), true, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), true, "aaa", new() {
                                        ["query"] = "aaa",
                                    } },
        { TestFileLine(), true, "a:a", new() {
                                        ["query"] = "a:a",
                                    } },
        { TestFileLine(), false, "\\aaa", null },
        { TestFileLine(), true, "?aaa", new() {
                                        ["query"] = "?aaa",
                                    } },
        { TestFileLine(), true, ":aaa", new() {
                                        ["query"] = ":aaa",
                                    } },
        { TestFileLine(), true, "123", new() {
                                        ["query"] = "123",
                                    } },
        { TestFileLine(), true, "%D0%B4%D0%B8%D1%80", new() {
                                        ["query"] = "%D0%B4%D0%B8%D1%80",
                                    } },
        { TestFileLine(), true, "/", new() {
                                        ["query"] = "/",
                                    } },
    };

    public static UriTheoryData KeyValueQueryData = new (){
        { TestFileLine(), true, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), true, "aaa=bbb", new() {
                                        ["kvQuery"] = "aaa=bbb",
                                        ["1"] = "aaa",
                                        ["2"] = "bbb",
                                    } },
        { TestFileLine(), true, "aaa=bbb&ccc=ddd", new() {
                                        ["kvQuery"] = "aaa=bbb&ccc=ddd",
                                        ["1"] = "aaa",
                                        ["2"] = "bbb",
                                        ["3"] = "ccc",
                                        ["4"] = "ddd",
                                    } },
        { TestFileLine(), true, "aaa=bbb&ccc=ddd&eee=fff", new() {
                                        ["kvQuery"] = "aaa=bbb&ccc=ddd&eee=fff",
                                        ["1"] = "aaa",
                                        ["2"] = "bbb",
                                        ["3"] = "ccc",
                                        ["4"] = "ddd",
                                        ["5"] = "eee",
                                        ["6"] = "fff",
                                    } },
        { TestFileLine(), true, "aaa=bbb&eee=fff&ccc=", new() {
                                        ["kvQuery"] = "aaa=bbb&eee=fff&ccc=",
                                        ["1"] = "aaa",
                                        ["2"] = "bbb",
                                        ["3"] = "eee",
                                        ["4"] = "fff",
                                        ["5"] = "ccc",
                                        ["6"] = "",
                                    } },
        { TestFileLine(), false, "=aaa", null },
        { TestFileLine(), false, "=aaa", null },
        { TestFileLine(), true, "aaa=", new() {
                                        ["kvQuery"] = "aaa=",
                                        ["1"] = "aaa",
                                        ["2"] = "",
                                    } },
        { TestFileLine(), true, "aaa==", new() {
                                        ["kvQuery"] = "aaa==",
                                        ["1"] = "aaa",
                                        ["2"] = "=",
                                    } },
        { TestFileLine(), true, "aa&a?=bbb", new() {
                                        ["kvQuery"] = "aa&a?=bbb",
                                        ["1"] = "aa&a?",
                                        ["2"] = "bbb",
                                    } },
        { TestFileLine(), true, "aa&a?=bb?b=", new() {
                                        ["kvQuery"] = "aa&a?=bb?b=",
                                        ["1"] = "aa&a?",
                                        ["2"] = "bb?b=",
                                    } },
        { TestFileLine(), false, "aa&a?=bb?b=&bb", null },
        { TestFileLine(), true, "a:a=b:b", new() {
                                        ["kvQuery"] = "a:a=b:b",
                                        ["1"] = "a:a",
                                        ["2"] = "b:b",
                                    } },
        { TestFileLine(), false, "\\aaa", null },
        { TestFileLine(), false, "?aaa", null },
        { TestFileLine(), true, ":aaa=bbb:", new() {
                                        ["kvQuery"] = ":aaa=bbb:",
                                        ["1"] = ":aaa",
                                        ["2"] = "bbb:",
                                    } },
        { TestFileLine(), true, "123=456", new() {
                                        ["kvQuery"] = "123=456",
                                        ["1"] = "123",
                                        ["2"] = "456",
                                    } },
        { TestFileLine(), true, "%D0%B4%D0%B8%D1%80=%D0%B4%D0%B8%D1%80", new() {
                                        ["kvQuery"] = "%D0%B4%D0%B8%D1%80=%D0%B4%D0%B8%D1%80",
                                        ["1"] = "%D0%B4%D0%B8%D1%80",
                                        ["2"] = "%D0%B4%D0%B8%D1%80",
                                    } },
        { TestFileLine(), true, "dir=%D0%B4%D0%B8%D1%80", new() {
                                        ["kvQuery"] = "dir=%D0%B4%D0%B8%D1%80",
                                        ["1"] = "dir",
                                        ["2"] = "%D0%B4%D0%B8%D1%80",
                                    } },
        { TestFileLine(), true, "%D0%B4%D0%B8%D1%80=dir", new() {
                                        ["kvQuery"] = "%D0%B4%D0%B8%D1%80=dir",
                                        ["1"] = "%D0%B4%D0%B8%D1%80",
                                        ["2"] = "dir",
                                    } },
        { TestFileLine(), true, "aaa=&aa&a?=bbb", new() {
                                        ["kvQuery"] = "aaa=&aa&a?=bbb",
                                        ["1"] = "aaa",
                                        ["2"] = "",
                                        ["3"] = "aa&a?",
                                        ["4"] = "bbb",
                                    } },
    };
}