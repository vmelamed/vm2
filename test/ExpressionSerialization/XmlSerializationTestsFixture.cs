namespace vm2.ExpressionSerialization.ExpressionSerializationTests;

public class XmlSerializationTestsFixture : IDisposable
{
    const string schemasPath = "../../../../../src/ExpressionSerialization/Schemas/";

    static readonly XmlSchemaSet _schemas = new();

    static readonly FileStreamOptions _fileStreamOptions = new() {
        Mode   = FileMode.Open,
        Access = FileAccess.Read,
        Share  = FileShare.Read,
    };

    static readonly Options _options = new()
    {
        ByteOrderMark = true,
        AddDocumentDeclaration = true,
        Indent = true,
        IndentSize = 4,
        AttributesOnNewLine = true,
        AddComments = true,
    };

    const LoadOptions loadOptions = LoadOptions.SetBaseUri | LoadOptions.SetLineInfo;

    public XmlSerializationTestsFixture()
    {
        var readerSettings = new XmlReaderSettings() {
            DtdProcessing = DtdProcessing.Parse
        };
        using (var stream = new FileStream($"{schemasPath}Microsoft.Serialization.xsd", _fileStreamOptions))
        using (var reader = XmlReader.Create(stream, readerSettings))
            _schemas.Add("http://schemas.microsoft.com/2003/10/Serialization/", reader);

        using (var stream = new FileStream($"{schemasPath}DataContract.xsd", _fileStreamOptions))
        using (var reader = XmlReader.Create(stream, readerSettings))
            _schemas.Add("http://schemas.datacontract.org/2004/07/System", reader);

        using (var stream = new FileStream($"{schemasPath}Expression.xsd", _fileStreamOptions))
        using (var reader = XmlReader.Create(stream, readerSettings))
            _schemas.Add("urn:schemas-vm-com:Aspects.Linq.Expressions.Serialization", reader);
    }

    public void Dispose() => GC.SuppressFinalize(this);

    public static bool Validate(XDocument doc, ITestOutputHelper? output = null)
    {
        var valid = true;

        doc.Validate(
                _schemas,
                (o, e) =>
                {
                    output?.WriteLine(
                        $"""
                        {e.Severity}: {e.Message}
                        {e.Exception}
                        """);
                    valid = false;
                });

        return valid;
    }

    public static async Task<(XDocument, string)> GetExpectedAsync(string pathName, ITestOutputHelper? output = null)
    {
        // ARRANGE - get the expected string and XDocument from a file:
        using var streamExpected = new FileStream(pathName, _fileStreamOptions);
        var length = (int)streamExpected.Length;
        Memory<byte> buf = new byte[length];
        var read = await streamExpected.ReadAsync(buf, CancellationToken.None);
        read.Should().Be(length, "Could not read the whole file.");
        var expectedStr = Encoding.UTF8.GetString(buf.Span);
        streamExpected.Seek(0, SeekOrigin.Begin);

        var expectedDoc = await XDocument.LoadAsync(streamExpected, loadOptions, CancellationToken.None);
        expectedDoc.Declaration = new XDeclaration(null, null, null);

        Validate(expectedDoc, output).Should().BeTrue($"Fix the test file (or the schema) - the XML file is not valid according to the schema.");

        return (expectedDoc, expectedStr);
    }

    public static void TestSerializeExpression(
        Expression expression,
        XDocument expectedDoc,
        string expectedStr,
        ITestOutputHelper? output = null)
    {
        // ACT - get the actual string and XDocument by transforming the expression:
        var transform = new ExpressionTransform(_options);
        var actualDoc = transform.TransformToDocument(expression);
        using var streamActual = new MemoryStream();
        transform.Serialize(expression, streamActual);
        var actualStr = Encoding.UTF8.GetString(streamActual.ToArray());

        output?.WriteLine("ACTUAL:\n{0}\n", actualStr);

        // ASSERT: both the strings and the XDocument-s are valid and equal
        Validate(actualDoc, output).Should().BeTrue("The actual document is not valid according to the schema.");

        actualStr.Should().Be(expectedStr, "The expected and the actual texts are different.");
        XNode.DeepEquals(actualDoc, expectedDoc).Should().BeTrue("The the expected and the actual XDocument-s are different.");
    }

    public static async Task TestSerializeExpressionAsync(
        Expression expression,
        XDocument expectedDoc,
        string expectedStr,
        ITestOutputHelper? output = null,
        CancellationToken cancellationToken = default)
    {

        // ACT - get the actual string and XDocument by transforming the expression:
        var transform = new ExpressionTransform(_options);
        var actualDoc = transform.TransformToDocument(expression);
        using var streamActual = new MemoryStream();
        await transform.SerializeAsync(expression, streamActual, cancellationToken);
        var actualStr = Encoding.UTF8.GetString(streamActual.ToArray());

        output?.WriteLine("ACTUAL:\n{0}\n", actualStr);

        // ASSERT: both the strings and the XDocument-s are valid and equal
        Validate(actualDoc, output).Should().BeTrue("The actual document is not valid according to the schema.");

        actualStr.Should().Be(expectedStr, "The expected and the actual texts are different.");
        XNode.DeepEquals(actualDoc, expectedDoc).Should().BeTrue("The the expected and the actual XDocument-s are different.");
    }
}
