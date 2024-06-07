namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

using vm2.ExpressionSerialization.JsonTransform;

public class TestsFixture : IDisposable
{
    internal const string TestFilesPath = "../../../TestData";

    internal const string SchemasPath = "../../../../../../src/ExpressionSerialization/Schemas";

    internal static FileStreamOptions FileStreamOptions => new() {
        Mode = FileMode.Open,
        Access = FileAccess.Read,
        Share = FileShare.Read,
    };

    internal static JsonOptions Options => new() {
        Indent = true,
        IndentSize = 4,
        AddComments = true,
        AllowTrailingCommas = true,
        ValidateInputDocuments = ValidateDocuments.Always,
    };

    internal const LoadOptions JsonLoadOptions = LoadOptions.SetLineInfo; // LoadOptions.SetBaseUri | LoadOptions.None;

    public TestsFixture()
        => Options.LoadSchema(Path.Combine(SchemasPath, "delinea.deployment.schema.json"/*"Linq.Expressions.Serialization.json"*/));

    public void Dispose() => GC.SuppressFinalize(this);

    public static void Validate(JsonNode doc)
        => Options.Validate(doc);

    public static async Task<(JsonNode?, string)> GetJsonDocumentAsync(
        string testFileLine,
        string pathName,
        string expectedOrInput,
        ITestOutputHelper? output = null,
        bool throwIo = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // ARRANGE - get the expected string and JsonNode from a file:
            pathName = Path.GetFullPath(pathName);
            using var streamExpected = new FileStream(pathName, FileStreamOptions);
            var length = (int)streamExpected.Length;
            Memory<byte> buf = new byte[length];
            var read = await streamExpected.ReadAsync(buf, cancellationToken);
            read.Should().Be(length, "should be able to read the whole file");
            var expectedStr = Encoding.UTF8.GetString(buf.Span);

            output?.WriteLine($"{expectedOrInput}:\n{0}\n", expectedStr);

            streamExpected.Seek(0, SeekOrigin.Begin);

            var expectedDoc = JsonNode.Parse(expectedStr);

            expectedDoc.Should().NotBeNull();
            Debug.Assert(expectedDoc is not null);

            var validate = () => Validate(expectedDoc);

            validate.Should().NotThrow($"the {expectedOrInput} document from {testFileLine} should be valid according to the schema");
            return (expectedDoc, expectedStr);
        }
        catch (IOException x)
        {
            if (throwIo)
                throw;
            output?.WriteLine($"Error getting the {expectedOrInput} document from `{pathName}`:\n{x}\nProceeding with creating the file from the actual document...");
        }
        catch (Exception x)
        {
            Assert.Fail($"Error getting the {expectedOrInput} document from `{pathName}`:\n{x}");
        }
        return (null, "");
    }

    public static void TestExpressionToJson(
        string testFileLine,
        Expression expression,
        JsonNode? expectedDoc,
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
        AssertJsonAsExpectedOrSave(testFileLine, expectedDoc, expectedStr, actualDoc, actualStr, fileName, output);
    }

    public static async Task TestExpressionToJsonAsync(
        string testFileLine,
        Expression expression,
        JsonNode? expectedDoc,
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
        AssertJsonAsExpectedOrSave(testFileLine, expectedDoc, expectedStr, actualDoc, actualStr, fileName, output);
    }

    public static void TestJsonToExpression(
        string testFileLine,
        JsonNode? inputDoc,
        Expression expectedExpression)
    {
        inputDoc.Should().NotBeNull("The JSON document (JsonNode?) to transform is null");
        Debug.Assert(inputDoc is not null);
        inputDoc.GetValueKind().Should().Be(JsonValueKind.Object, "The input JSON document (JsonNode?) is not JsonObject.");

        // ACT - get the actual string and XDocument by transforming the expression:
        var transform = new ExpressionTransform(Options);
        var actualExpression = transform.Transform(inputDoc.AsObject());

        expectedExpression
            .DeepEquals(actualExpression, out var difference)
            .Should()
            .BeTrue($"the expression at {testFileLine} should be \"DeepEqual\" to `{expectedExpression}`\n({difference})");
    }

    static void AssertJsonAsExpectedOrSave(
        string testFileLine,
        JsonNode? expectedDoc,
        string expectedStr,
        JsonObject actualDoc,
        string actualStr,
        string? fileName,
        ITestOutputHelper? output = null)
    {
        output?.WriteLine("ACTUAL:\n{0}\n", actualStr);

        // ASSERT: both the strings and the JsonObject-s are valid and equal
        var validate = () => Validate(actualDoc);

        validate.Should().NotThrow($"the ACTUAL document from {testFileLine} should be valid according to the schema `{JsonOptions.Exs}`.");

        if (expectedDoc is null)
        {
            // create a new file with contents - the actual JSON
            fileName = string.IsNullOrEmpty(fileName)
                            ? Path.GetFullPath(Path.Combine(TestFilesPath, DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss.fff") + ".json"))
                            : Path.GetFullPath(fileName);

            var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            using var writer = new Utf8JsonWriter(stream, Options.JsonWriterOptions);

            actualDoc.WriteTo(writer);

            Assert.Fail($"The expected JSON does not appear to exist. Saved the actual JSON in the file `{fileName}`.");
        }

        expectedDoc.GetValueKind().Should().Be(JsonValueKind.Object, "The expected JSON document (JsonNode?) is not JsonObject.");

        actualStr.Should().Be(expectedStr, "the expected and the actual JSON texts should be the same");

        var equal = JsonNode
                        .DeepEquals(actualDoc, expectedDoc)
                        .Should()
                        .BeTrue($"the expected and the actual top-level JsonObject objects (documents) from {testFileLine} should be deep-equal.")
                        ;
    }
}
