namespace vm2.Linq.ExpressionSerialization.Json.Tests;

public class JsonTestsFixture : BaseTestsFixture<JsonTestsFixture>
{
    public string TestFilesPath { get; init; }

    public string TestLoadPath { get; init; }

    public string SchemasPath { get; init; }

    public FileStreamOptions FileStreamOptions { get; } = new() {
        Mode = FileMode.Open,
        Access = FileAccess.Read,
        Share = FileShare.Read,
    };

    public JsonOptions Options { get; set; }

    public JsonTestsFixture() : base()
    {
        var gitWorkSpace = Path.GetFullPath(
                                    Path.Combine(
                                        Repository.Discover(".") ?? throw new FileNotFoundException("Could not find the GIT repository of the test."),
                                        ".."));

        TestFilesPath = Path.Combine(gitWorkSpace, "test/ExpressionSerialization.Tests/ExpressionSerialization.Shared.Tests/TestData/Json");
        TestLoadPath = Path.Combine(TestFilesPath, "LoadTestData");
        SchemasPath = Path.Combine(gitWorkSpace, "src/ExpressionSerialization.Json/Schema");

        Options = new(Path.Combine(SchemasPath, "Linq.Expressions.Serialization.json")) {
            Indent = true,
            IndentSize = 4,
            AddComments = true,
            AllowTrailingCommas = true,
            ValidateInputDocuments = ValidateExpressionDocuments.Always,
        };
    }

    public void Validate(JsonNode doc) => Options.Validate(doc);

    public void Validate(string json) => Options.Validate(json);

    /// <summary>
    /// Get json document as an asynchronous operation.
    /// </summary>
    /// <param name="testFileLine">The file and line of the actual test data.</param>
    /// <param name="pathName">The pathname of the file to get.</param>
    /// <param name="expectedOrInput">The string designation of the input file as EXPECTED or INPUT for test output purposes.</param>
    /// <param name="validate">if set to <c>true</c> validates the read file against the schema.</param>
    /// <param name="output">The test output.</param>
    /// <param name="throwIo">if set to <c>true</c> throws exception on I/O operation failure.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
    public async Task<(JsonNode?, string)> GetJsonDocumentAsync(
        string testFileLine,
        string pathName,
        string expectedOrInput,
        ITestOutputHelper? output = null,
        bool validate = false,
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

            if (validate)
            {
                var isValid = () => Validate(expectedStr);

                isValid.Should().NotThrow($"the {expectedOrInput} document from {testFileLine} should be valid according to the schema");
            }

            return (expectedDoc, expectedStr);
        }
        catch (IOException x)
        {
            if (throwIo)
                throw;
            output?.WriteLine($"WARNING: error getting the {expectedOrInput} document from `{pathName}`:\n{x}\nProceeding with creating the file from the actual document...");
        }
        catch (Exception x)
        {
            output?.WriteLine($"ERROR: Error getting the {expectedOrInput} document from `{pathName}`:\n{x}");
        }
        return (null, "");
    }

    /// <summary>
    /// Tests the expression to json.
    /// </summary>
    /// <param name="testFileLine">The file and line of the actual test data.</param>
    /// <param name="expression">The expression to test.</param>
    /// <param name="expectedDoc">The expected document.</param>
    /// <param name="expectedStr">The expected string.</param>
    /// <param name="fileName">Name of the expected document's file.</param>
    /// <param name="output">The test output writer.</param>
    /// <param name="validate">if set to <c>true</c> - validate the produced actual JSON document.</param>
    public void TestExpressionToJson(
        string testFileLine,
        Expression expression,
        JsonNode? expectedDoc,
        string expectedStr,
        string? fileName,
        ITestOutputHelper? output = null,
        bool validate = true)
    {
        // ACT - get the actual string and XDocument by transforming the expression:

        var transform = new ExpressionJsonTransform(Options);

        var actualDoc = transform.Transform(expression);
        using var streamActual = new MemoryStream();
        transform.Serialize(expression, streamActual);
        var actualStr = Encoding.UTF8.GetString(streamActual.ToArray());

        // ASSERT: both the strings and the XDocument-s are valid and equal
        AssertJsonAsExpectedOrSave(testFileLine, expectedDoc, expectedStr, actualDoc, actualStr, fileName, false, output, validate);
    }

    /// <summary>
    /// Test expression to json as an asynchronous operation.
    /// </summary>
    /// <param name="testFileLine">The file and line of the actual test data.</param>
    /// <param name="expression">The expression to test.</param>
    /// <param name="expectedDoc">The expected document.</param>
    /// <param name="expectedStr">The expected string.</param>
    /// <param name="fileName">Name of the expected document's file.</param>
    /// <param name="output">The test output writer.</param>
    /// <param name="validate">if set to <c>true</c> - validate the produced actual JSON document.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task TestExpressionToJsonAsync(
        string testFileLine,
        Expression expression,
        JsonNode? expectedDoc,
        string expectedStr,
        string? fileName,
        ITestOutputHelper? output = null,
        bool validate = false,                               // by default don't validate the async transform - the sync transform already validated it
        CancellationToken cancellationToken = default)
    {
        // async ACT - get the actual string and XDocument by transforming the expression:

        var transform = new ExpressionJsonTransform(Options);

        var actualDoc = transform.Transform(expression);
        using var streamActual = new MemoryStream();
        await transform.SerializeAsync(expression, streamActual, cancellationToken);
        var actualStr = Encoding.UTF8.GetString(streamActual.ToArray());

        // ASSERT: both the strings and the XDocument-s are valid and equal
        AssertJsonAsExpectedOrSave(testFileLine, expectedDoc, expectedStr, actualDoc, actualStr, fileName, true, output, validate);
    }

    /// <summary>
    /// Asserts that the produced JSON is as expected or if expected is not present - save it in a file for the next tests to use as expected.
    /// </summary>
    /// <param name="testFileLine">The file and line of the actual test data.</param>
    /// <param name="expectedDoc">The expected document.</param>
    /// <param name="expectedStr">The expected string.</param>
    /// <param name="actualDoc">The actual document.</param>
    /// <param name="actualStr">The actual string.</param>
    /// <param name="fileName">Name of the file where to store the produced JSON document.</param>
    /// <param name="async">if set to <c>true</c> it is an asynchronous conversion, otherwise synchronous.</param>
    /// <param name="validate">if set to <c>true</c> - validate the produced actual JSON document.</param>
    /// <param name="output">The test output writer.</param>
    void AssertJsonAsExpectedOrSave(
        string testFileLine,
        JsonNode? expectedDoc,
        string expectedStr,
        JsonObject actualDoc,
        string actualStr,
        string? fileName,
        bool async,
        ITestOutputHelper? output,
        bool validate)
    {
        output?.WriteLine($"ACTUAL ({(async ? "async" : "sync")}):\n{actualStr}\n");

        if (validate)
        {
            // ASSERT: both the strings and the JsonObject-s are valid and equal
            var isValid = () => Validate(actualStr);

            isValid.Should().NotThrow($"the ACTUAL document from {testFileLine} should be valid according to the schema `{JsonOptions.Exs}`.");
        }

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

    /// <summary>
    /// Tests the json to expression transformation.
    /// </summary>
    /// <param name="testFileLine">The file and line of the actual test data.</param>
    /// <param name="inputDoc">The input document.</param>
    /// <param name="expectedExpression">The expected expression.</param>
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
}
