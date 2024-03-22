namespace vm2.RegexLibTests;

public partial class UrisTests
{
    public static UriTheoryData PathAbsData = new () {
        { TestLine(), true, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), true, "aaa", new() {
            ["path"] = "aaa",
            ["pathRootless"] = "aaa",
        } },
        { TestLine(), true, "a:a", new() {
            ["path"] = "a:a",
            ["pathRootless"] = "a:a",
        } },
        { TestLine(), false, "\\aaa", null },
        { TestLine(), false, "?aaa", null },
        { TestLine(), true, ":aaa", new() {
            ["path"] = ":aaa",
            ["pathRootless"] = ":aaa",
        } },
        { TestLine(), true, "123", new() {
            ["path"] = "123",
            ["pathRootless"] = "123",
        } },
        { TestLine(), true, "123", new() {
            ["path"] = "123",
            ["pathRootless"] = "123",
        } },
        { TestLine(), true, "%D0%B4%D0%B8%D1%80", new() {
            ["path"] = "%D0%B4%D0%B8%D1%80", // →path← = →дир←
            ["pathRootless"] = "%D0%B4%D0%B8%D1%80", // →pathRootless← = →дир←
        } },
        { TestLine(), true, "/", new() {
            ["path"] = "/",
            ["pathAbsEmpty"] = "/",
            ["pathAbsEmpty"] = "/",
        } },
        { TestLine(), true, "/aaa", new() {
            ["path"] = "/aaa",
            ["pathAbsEmpty"] = "/aaa",
            ["pathAbsEmpty"] = "/aaa",
        } },
        { TestLine(), true, "/12a.a@tyty", new() {
            ["path"] = "/12a.a@tyty",
            ["pathAbsEmpty"] = "/12a.a@tyty",
            ["pathAbsEmpty"] = "/12a.a@tyty",
        } },
        { TestLine(), false, "/12aa#tyty", null },
        { TestLine(), true, "/12a/a@tyty", new() {
            ["path"] = "/12a/a@tyty",
            ["pathAbsEmpty"] = "/12a/a@tyty",
            ["pathAbsEmpty"] = "/12a/a@tyty",
        } },
        { TestLine(), true, "/12a/a:tyty", new() {
            ["path"] = "/12a/a:tyty",
            ["pathAbsEmpty"] = "/12a/a:tyty",
            ["pathAbsEmpty"] = "/12a/a:tyty",
        } },
        { TestLine(), true, "/12a/a@tyty", new() {
            ["path"] = "/12a/a@tyty",
            ["pathAbsEmpty"] = "/12a/a@tyty",
            ["pathAbsEmpty"] = "/12a/a@tyty",
        } },
        { TestLine(), true, "/c:/12a/a@tyty", new() {
            ["path"] = "/c:/12a/a@tyty",
            ["pathAbsEmpty"] = "/c:/12a/a@tyty",
            ["pathAbsEmpty"] = "/c:/12a/a@tyty",
        } },
        { TestLine(), true, "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3", new() {
            ["path"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
            ["pathAbsEmpty"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
            ["pathAbsEmpty"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
        } },
        { TestLine(), true, "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3/123", new() {
            ["path"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3/123",
            ["pathAbsEmpty"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3/123",
            ["pathAbsEmpty"] = "/12a/a@tyty/%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3/123",
        } },
        { TestLine(), true, "//aaa", new() {
            ["path"] = "//aaa",
            ["pathAbsEmpty"] = "//aaa",
            ["pathAbsEmpty"] = "//aaa",
        } },
        { TestLine(), true, "////aaa", new() {
            ["path"] = "////aaa",
            ["pathAbsEmpty"] = "////aaa",
            ["pathAbsEmpty"] = "////aaa",
        } },
        { TestLine(), true, "/", new() {
            ["path"] = "/",
            ["pathAbsEmpty"] = "/",
            ["pathAbsEmpty"] = "/",
        } },
        { TestLine(), true, "/bbb///aaa", new() {
            ["path"] = "/bbb///aaa",
            ["pathAbsEmpty"] = "/bbb///aaa",
            ["pathAbsEmpty"] = "/bbb///aaa",
        } },
    };

    public static UriTheoryData GeneralQueryData = new (){
        { TestLine(), true, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), true, "aaa", new() {
                                        ["query"] = "aaa",
                                    } },
        { TestLine(), true, "a:a", new() {
                                        ["query"] = "a:a",
                                    } },
        { TestLine(), false, "\\aaa", null },
        { TestLine(), true, "?aaa", new() {
                                        ["query"] = "?aaa",
                                    } },
        { TestLine(), true, ":aaa", new() {
                                        ["query"] = ":aaa",
                                    } },
        { TestLine(), true, "123", new() {
                                        ["query"] = "123",
                                    } },
        { TestLine(), true, "%D0%B4%D0%B8%D1%80", new() {
                                        ["query"] = "%D0%B4%D0%B8%D1%80",
                                    } },
        { TestLine(), true, "/", new() {
                                        ["query"] = "/",
                                    } },
    };

    public static UriTheoryData KeyValueQueryData = new (){
        { TestLine(), true, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), true, "aaa=bbb", new() {
                                        ["kvQuery"] = "aaa=bbb",
                                        ["1"] = "aaa",
                                        ["2"] = "bbb",
                                    } },
        { TestLine(), true, "aaa=bbb&ccc=ddd", new() {
                                        ["kvQuery"] = "aaa=bbb&ccc=ddd",
                                        ["1"] = "aaa",
                                        ["2"] = "bbb",
                                        ["3"] = "ccc",
                                        ["4"] = "ddd",
                                    } },
        { TestLine(), true, "aaa=bbb&ccc=ddd&eee=fff", new() {
                                        ["kvQuery"] = "aaa=bbb&ccc=ddd&eee=fff",
                                        ["1"] = "aaa",
                                        ["2"] = "bbb",
                                        ["3"] = "ccc",
                                        ["4"] = "ddd",
                                        ["5"] = "eee",
                                        ["6"] = "fff",
                                    } },
        { TestLine(), true, "aaa=bbb&eee=fff&ccc=", new() {
                                        ["kvQuery"] = "aaa=bbb&eee=fff&ccc=",
                                        ["1"] = "aaa",
                                        ["2"] = "bbb",
                                        ["3"] = "eee",
                                        ["4"] = "fff",
                                        ["5"] = "ccc",
                                        ["6"] = "",
                                    } },
        { TestLine(), false, "=aaa", null },
        { TestLine(), false, "=aaa", null },
        { TestLine(), true, "aaa=", new() {
                                        ["kvQuery"] = "aaa=",
                                        ["1"] = "aaa",
                                        ["2"] = "",
                                    } },
        { TestLine(), true, "aaa==", new() {
                                        ["kvQuery"] = "aaa==",
                                        ["1"] = "aaa",
                                        ["2"] = "=",
                                    } },
        { TestLine(), true, "aa&a?=bbb", new() {
                                        ["kvQuery"] = "aa&a?=bbb",
                                        ["1"] = "aa&a?",
                                        ["2"] = "bbb",
                                    } },
        { TestLine(), true, "aa&a?=bb?b=", new() {
                                        ["kvQuery"] = "aa&a?=bb?b=",
                                        ["1"] = "aa&a?",
                                        ["2"] = "bb?b=",
                                    } },
        { TestLine(), false, "aa&a?=bb?b=&bb", null },
        { TestLine(), true, "a:a=b:b", new() {
                                        ["kvQuery"] = "a:a=b:b",
                                        ["1"] = "a:a",
                                        ["2"] = "b:b",
                                    } },
        { TestLine(), false, "\\aaa", null },
        { TestLine(), false, "?aaa", null },
        { TestLine(), true, ":aaa=bbb:", new() {
                                        ["kvQuery"] = ":aaa=bbb:",
                                        ["1"] = ":aaa",
                                        ["2"] = "bbb:",
                                    } },
        { TestLine(), true, "123=456", new() {
                                        ["kvQuery"] = "123=456",
                                        ["1"] = "123",
                                        ["2"] = "456",
                                    } },
        { TestLine(), true, "%D0%B4%D0%B8%D1%80=%D0%B4%D0%B8%D1%80", new() {
                                        ["kvQuery"] = "%D0%B4%D0%B8%D1%80=%D0%B4%D0%B8%D1%80",
                                        ["1"] = "%D0%B4%D0%B8%D1%80",
                                        ["2"] = "%D0%B4%D0%B8%D1%80",
                                    } },
        { TestLine(), true, "dir=%D0%B4%D0%B8%D1%80", new() {
                                        ["kvQuery"] = "dir=%D0%B4%D0%B8%D1%80",
                                        ["1"] = "dir",
                                        ["2"] = "%D0%B4%D0%B8%D1%80",
                                    } },
        { TestLine(), true, "%D0%B4%D0%B8%D1%80=dir", new() {
                                        ["kvQuery"] = "%D0%B4%D0%B8%D1%80=dir",
                                        ["1"] = "%D0%B4%D0%B8%D1%80",
                                        ["2"] = "dir",
                                    } },
        { TestLine(), true, "aaa=&aa&a?=bbb", new() {
                                        ["kvQuery"] = "aaa=&aa&a?=bbb",
                                        ["1"] = "aaa",
                                        ["2"] = "",
                                        ["3"] = "aa&a?",
                                        ["4"] = "bbb",
                                    } },
    };
}