namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

using vm2.ExpressionSerialization.JsonTransform;

public class JsonTestsFixture : IDisposable
{
    internal const string TestFilesPath = "../../../TestData";
    internal const string TestLoadPath = "../../../LoadTestData";

    internal const string SchemasPath = "../../../../../../src/ExpressionSerialization/Schemas";

    public FileStreamOptions FileStreamOptions { get; } = new() {
        Mode = FileMode.Open,
        Access = FileAccess.Read,
        Share = FileShare.Read,
    };

    public JsonOptions Options { get; set; } = new(Path.Combine(SchemasPath, "Linq.Expressions.Serialization.json")) {
        Indent = true,
        IndentSize = 4,
        AddComments = true,
        AllowTrailingCommas = true,
        ValidateInputDocuments = ValidateDocuments.Always,
    };

    public void Dispose() => GC.SuppressFinalize(this);

    public void Validate(JsonNode doc) => Options.Validate(doc);

    public async Task<(JsonNode?, string)> GetJsonDocumentAsync(
        string testFileLine,
        string pathName,
        string expectedOrInput,
        ITestOutputHelper? output = null,
        bool throwIo = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // ARRANGE - get the expected string and validated JsonNode from a file:

            pathName = Path.GetFullPath(pathName);
            using var streamExpected = new FileStream(pathName, FileStreamOptions);
            var length = (int)streamExpected.Length;

            // get the text to display
            Memory<byte> buf = new byte[length];
            var read = await streamExpected.ReadAsync(buf, cancellationToken);
            read.Should().Be(length, "should be able to read the whole file");
            var expectedStr = Encoding.UTF8.GetString(buf.Span);
            output?.WriteLine($"{expectedOrInput}:\n{expectedStr}\n");

            // and parse
            streamExpected.Seek(0, SeekOrigin.Begin);

            var parse = async () => await JsonNode.ParseAsync(streamExpected, JsonOptions.JsonNodeOptions, JsonOptions.JsonDocumentOptions, cancellationToken);
            var expectedDoc = (await parse.Should().NotThrowAsync()).Which;

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

    public void TestExpressionToJson(
        string testFileLine,
        Expression expression,
        JsonNode? expectedDoc,
        string expectedStr,
        string? fileName,
        ITestOutputHelper? output = null)
    {
        // ACT - get the actual string and XDocument by transforming the expression:

        var transform = new ExpressionJsonTransform(Options);

        var actualDoc = transform.Transform(expression);
        using var streamActual = new MemoryStream();
        transform.Serialize(expression, streamActual);
        var actualStr = Encoding.UTF8.GetString(streamActual.ToArray());

        // ASSERT: both the strings and the XDocument-s are valid and equal
        AssertJsonAsExpectedOrSave(testFileLine, expectedDoc, expectedStr, actualDoc, actualStr, fileName, false, output);
    }

    public async Task TestExpressionToJsonAsync(
        string testFileLine,
        Expression expression,
        JsonNode? expectedDoc,
        string expectedStr,
        string? fileName,
        ITestOutputHelper? output = null,
        CancellationToken cancellationToken = default)
    {
        // async ACT - get the actual string and XDocument by transforming the expression:

        var transform = new ExpressionJsonTransform(Options);

        var actualDoc = transform.Transform(expression);
        using var streamActual = new MemoryStream();
        await transform.SerializeAsync(expression, streamActual, cancellationToken);
        var actualStr = Encoding.UTF8.GetString(streamActual.ToArray());

        // ASSERT: both the strings and the XDocument-s are valid and equal
        AssertJsonAsExpectedOrSave(testFileLine, expectedDoc, expectedStr, actualDoc, actualStr, fileName, true, output);
    }

    public void TestJsonToExpression(
        string testFileLine,
        JsonNode? inputDoc,
        Expression expectedExpression)
    {
        inputDoc.Should().NotBeNull("The JSON document (JsonNode?) to transform is null");
        Debug.Assert(inputDoc is not null);
        inputDoc.GetValueKind().Should().Be(JsonValueKind.Object, "The input JSON document (JsonNode?) is not JsonObject.");

        // ACT - get the actual string and XDocument by transforming the expression:
        var transform = new ExpressionJsonTransform(Options);
        var actualExpression = transform.Transform(inputDoc.AsObject());

        expectedExpression
            .DeepEquals(actualExpression, out var difference)
            .Should()
            .BeTrue($"the expression at {testFileLine} should be \"DeepEqual\" to `{expectedExpression}`\n({difference})");
    }

    void AssertJsonAsExpectedOrSave(
        string testFileLine,
        JsonNode? expectedDoc,
        string expectedStr,
        JsonObject actualDoc,
        string actualStr,
        string? fileName,
        bool async,
        ITestOutputHelper? output)
    {
        output?.WriteLine($"ACTUAL ({(async ? "async" : "sync")}):\n{actualStr}\n");

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
