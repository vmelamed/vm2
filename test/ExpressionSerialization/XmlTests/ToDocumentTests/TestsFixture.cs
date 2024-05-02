namespace vm2.ExpressionSerialization.XmlTests.ToDocumentTests;

public class TestsFixture : IDisposable
{
    internal const string TestFilesPath = "../../../TestData";

    internal const string SchemasPath = "../../../../../../src/ExpressionSerialization/Schemas";

    internal static FileStreamOptions FileStreamOptions => new() {
        Mode = FileMode.Open,
        Access = FileAccess.Read,
        Share = FileShare.Read,
    };

    internal static Options Options => new() {
        ByteOrderMark = true,
        AddDocumentDeclaration = true,
        OmitDuplicateNamespaces = false, // otherwise DeepEquals will fail
        Indent = true,
        IndentSize = 4,
        AttributesOnNewLine = true,
        AddComments = true,
    };

    internal const LoadOptions XmlLoadOptions = LoadOptions.SetLineInfo; // LoadOptions.SetBaseUri | LoadOptions.None;

    public TestsFixture()
    {
        Options.SetSchemaLocation(Options.Ser, Path.Combine(SchemasPath, "Microsoft.Serialization.xsd"));
        Options.SetSchemaLocation(Options.Dcs, Path.Combine(SchemasPath, "DataContract.xsd"));
        Options.SetSchemaLocation(Options.Exs, Path.Combine(SchemasPath, "Expression.xsd"));
    }

    public void Dispose() => GC.SuppressFinalize(this);

    public static void Validate(XDocument doc)
    {
        List<XmlSchemaException> exceptions = [];

        doc.Validate(Options.Schemas, (_, e) => exceptions.Add(e.Exception));

        if (exceptions.Count is not 0)
            throw new AggregateException(
                        $"Error(s) validating the XML document against the {Options.Exs}:\n  " +
                        string.Join("\n  ", exceptions.Select(x => $"({x.LineNumber},{x.LinePosition}) : {x.Message}")),
                        exceptions);
    }

    public static async Task<(XDocument?, string)> GetExpectedAsync(string pathName, ITestOutputHelper? output = null)
    {
        try
        {
            // ARRANGE - get the expected string and XDocument from a file:
            pathName = Path.GetFullPath(pathName);
            using var streamExpected = new FileStream(pathName, FileStreamOptions);
            var length = (int)streamExpected.Length;
            Memory<byte> buf = new byte[length];
            var read = await streamExpected.ReadAsync(buf, CancellationToken.None);
            read.Should().Be(length, "should be able to read the whole file");
            var expectedStr = Encoding.UTF8.GetString(buf.Span);

            output?.WriteLine("EXPECTED:\n{0}\n", expectedStr);

            streamExpected.Seek(0, SeekOrigin.Begin);

            var expectedDoc = await XDocument.LoadAsync(streamExpected, XmlLoadOptions, CancellationToken.None);
            var validate = () => Validate(expectedDoc);

            validate.Should().NotThrow("the EXPECTED document should be valid according to the schema");
            return (expectedDoc, expectedStr);
        }
        catch (IOException x)
        {
            output?.WriteLine($"Error getting the expected document from `{pathName}`:\n{x}\nProceeding with creating the file from the actual document...");
        }
        catch (Exception x)
        {
            Xunit.Assert.Fail($"Error getting the expected document from `{pathName}`:\n{x}");
        }
        return (null, "");
    }

    public static void TestSerializeExpression(
        Expression expression,
        XDocument? expectedDoc,
        string expectedStr,
        string? fileName,
        ITestOutputHelper? output = null)
    {
        // ACT - get the actual string and XDocument by transforming the expression:
        var transform = new ExpressionTransform(Options);
        var actualDoc = transform.Transform(expression);
        using var streamActual = new MemoryStream();
        transform.Serialize(expression, streamActual);
        var actualStr = Encoding.UTF8.GetString(streamActual.ToArray());

        // ASSERT: both the strings and the XDocument-s are valid and equal
        Assert(expectedDoc, expectedStr, actualDoc, actualStr, fileName, output);
    }

    public static async Task TestSerializeExpressionAsync(
        Expression expression,
        XDocument? expectedDoc,
        string expectedStr,
        string? fileName,
        ITestOutputHelper? output = null,
        CancellationToken cancellationToken = default)
    {
        // ACT - get the actual string and XDocument by transforming the expression:
        var transform = new ExpressionTransform(Options);
        var actualDoc = transform.Transform(expression);
        using var streamActual = new MemoryStream();
        await transform.SerializeAsync(expression, streamActual, cancellationToken);
        var actualStr = Encoding.UTF8.GetString(streamActual.ToArray());

        // ASSERT: both the strings and the XDocument-s are valid and equal
        Assert(expectedDoc, expectedStr, actualDoc, actualStr, fileName, output);
    }

    static void Assert(
        XDocument? expectedDoc,
        string expectedStr,
        XDocument actualDoc,
        string actualStr,
        string? fileName,
        ITestOutputHelper? output = null)
    {
        output?.WriteLine("ACTUAL:\n{0}\n", actualStr);

        // ASSERT: both the strings and the XDocument-s are valid and equal
        var validate = () => Validate(actualDoc);

        validate.Should().NotThrow("the ACTUAL document should be valid according to the schema");

        if (expectedDoc is null)
        {
            fileName = string.IsNullOrEmpty(fileName)
                            ? Path.GetFullPath(Path.Combine(TestFilesPath, DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss.fff") + ".xml"))
                            : Path.GetFullPath(fileName);

            var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            var settings = new XmlWriterSettings() {
                Encoding = Options.GetEncoding(),
                Indent = Options.Indent,
                IndentChars = new(' ', Options.IndentSize),
                NamespaceHandling = Options.OmitDuplicateNamespaces ? NamespaceHandling.OmitDuplicates : NamespaceHandling.Default,
                NewLineOnAttributes = Options.AttributesOnNewLine,
                OmitXmlDeclaration = !Options.AddDocumentDeclaration,
                WriteEndDocumentOnClose = true,
            };
            using var writer = new StreamWriter(stream, Options.GetEncoding());
            using var xmlWriter = XmlWriter.Create(writer, settings);

            actualDoc.WriteTo(xmlWriter);

            Xunit.Assert.Fail($"The expected XML does not appear to exist. Saved the actual XML in the file `{fileName}`.");
        }

        actualStr.Should().Be(expectedStr, "the expected and the actual XML texts should be the same");

        var comparer = new XNodeDeepEquals();
        var myEquals = comparer.AreEqual(actualDoc, expectedDoc);

        if (!myEquals)
            output?.WriteLine(comparer.LastResult);

        var deepEquals = XNode.DeepEquals(actualDoc, expectedDoc);

        if (!deepEquals)
            output?.WriteLine("XNode.DeepEquals returned false!");

        (myEquals || deepEquals).Should().BeTrue("the expected and the actual XDocument objects should be deep-equal");
    }
}
