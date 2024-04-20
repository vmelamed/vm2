namespace vm2.ExpressionSerialization.XmlTests;

public partial class LambdaExpressionTests
{
    public static readonly TheoryData<string, string, string> LambdaExpressionData = new ()
    {
        { TestLine(), "i => true",     "Param2BoolConstant.xml" },
        { TestLine(), "(s,d) => true", "2ParamsToConstant.xml" },
    };

    public static Expression Substitute(string value) => _substitutes[value]();

    static Dictionary<string, Func<Expression>> _substitutes = new()
    {
        ["(s,d) => true"] = () => { Expression<Func<string, DateTime, bool>> x = (s, d) => true; return x; },
        ["i => true"]     = () => { Expression<Func<int, bool>>              x = i => true;      return x; },
    };
}