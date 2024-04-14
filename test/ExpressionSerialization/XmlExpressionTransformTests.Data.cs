namespace ExpressionSerializationTests;

public partial class XmlExpressionTransformTests
{
    public static TheoryData<string, Expression, string> ExpressionsData = new ()
    {
        { TestLine(), Expression.Constant(5), "Int.xml" },
    };
}
