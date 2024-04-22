namespace vm2.ExpressionSerialization.XmlTests;

public partial class BinaryExpressionTests
{
    public static readonly TheoryData<string, string, string> BinaryExpressionData = new ()
    {
        { TestLine(), "(a, b) => checked(a - b)",   "SubtractChecked.xml" },
        { TestLine(), "(a, b) => a - b",            "Subtract.xml" },
        { TestLine(), "(a, b) => a >> b",           "RightShift.xml" },
        { TestLine(), "(a, b) => a ^ b",            "Power.xml" },
        { TestLine(), "(a, b) => a || b",           "OrElse.xml" },
        { TestLine(), "(a, b) => a | b",            "Or.xml" },
        { TestLine(), "(a, b) => a != b",           "NotEqual.xml" },
        { TestLine(), "(a, b) => checked(a * b)",   "MultiplyChecked.xml" },
        { TestLine(), "(a, b) => a * b",            "Multiply.xml" },
        { TestLine(), "(a, b) => a % b",            "Modulo.xml" },
        { TestLine(), "(a, b) => a <= b",           "LessThanOrEqual.xml" },
        { TestLine(), "(a, b) => a < b",            "LessThan.xml" },
        { TestLine(), "(a, b) => a << b",           "LeftShift.xml" },
        { TestLine(), "(a, b) => a >= b",           "GreaterThanOrEqual.xml" },
        { TestLine(), "(a, b) => a > b",            "GreaterThan.xml" },
        { TestLine(), "(a, b) => a ^ b",            "ExclusiveOr.xml" },
        { TestLine(), "(a, b) => a == b",           "Equal.xml" },
        { TestLine(), "(a, b) => a / b",            "Divide.xml" },
        { TestLine(), "(a, b) => a ?? b",           "Coalesce.xml" },
        { TestLine(), "(a, i) => a[i]",             "ArrayIndex.xml" },
        { TestLine(), "(a, b) => a && b",           "AndAlso.xml" },
        { TestLine(), "(a, b) => a & b",            "And.xml" },
        { TestLine(), "(a, b) => (a + b) * 42",     "MultiplyAdd.xml" },
        { TestLine(), "(a, b) => a + b * 42",       "AddMultiply.xml" },
        { TestLine(), "(a, b) => checked(a + b)",   "AddChecked.xml" },
        { TestLine(), "(a, b) => a + (b + c)",      "AddParenAdd.xml" },
        { TestLine(), "(a, b) => a + b + c"  ,      "AddAdd.xml" },
        { TestLine(), "(a, b) => a + b"      ,      "Add.xml" },
    };

    public static Expression Substitute(string value) => _substitutes[value];

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["(a, b) => checked(a - b)"]            = (int a, int b) => checked(a - b),
        ["(a, b) => a - b"]                     = (int a, int b) => a - b,
        ["(a, b) => a >> b"]                    = (int a, int b) => a >> b,
        ["(a, b) => a ^ b"]                     = (int a, int b) => a ^ b,
        ["(a, b) => a || b"]                    = (bool a, bool b) => a || b,
        ["(a, b) => a | b"]                     = (int a, int b) => a | b,
        ["(a, b) => a != b"]                    = (int a, int b) => a != b,
        ["(a, b) => checked(a * b)"]            = (int a, int b) => checked(a * b),
        ["(a, b) => a * b"]                     = (int a, int b) => a * b,
        ["(a, b) => a % b"]                     = (int a, int b) => a % b,
        ["(a, b) => a <= b"]                    = (int a, int b) => a <= b,
        ["(a, b) => a < b"]                     = (int a, int b) => a < b,
        ["(a, b) => a << b"]                    = (int a, int b) => a << b,
        ["(a, b) => a >= b"]                    = (int a, int b) => a >= b,
        ["(a, b) => a > b"]                     = (int a, int b) => a > b,
        ["(a, b) => a ^ b"]                     = (int a, int b) => a ^ b,
        ["(a, b) => a == b"]                    = (int a, int b) => a == b,
        ["(a, b) => a / b"]                     = (int a, int b) => a / b,
        ["(a, b) => a ?? b"]                    = (int? a, int b) => a ?? b,
        ["(a, i) => a[i]"]                      = (int[] a, int i) => a[i],
        ["(a, b) => a && b"]                    = (bool a, bool b) => a && b,
        ["(a, b) => a & b"]                     = (int a, int b) => a & b,
        ["(a, b) => (a + b) * 42"]              = (int a, int b) => (a + b) * 42,
        ["(a, b) => a + b * 42"]                = (int a, int b) => a + b * 42,
        ["(a, b) => checked(a + b)"]            = (int a, int b) => checked(a + b),
        ["(a, b) => a + (b + c)"]               = (int a, int b, int c) => a + (b + c),
        ["(a, b) => a + b + c"]                 = (int a, int b, int c) => a + b + c,
        ["(a, b) => a + b"]                     = (int a, int b) => a + b,
    };
}