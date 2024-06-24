namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

using System.Linq.Expressions;

using vm2.ExpressionSerialization.Exceptions;

[CollectionDefinition("JSON")]
public class TransformLoadDocumentTest(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestLoadPath);

    protected override Expression Substitute(string id) => throw new NotImplementedException();

    void ResetReloadSchemas(bool loadSchemas, ValidateDocuments validate)
        => _fixture.Options = loadSchemas
                                ? new(Path.Combine(JsonTestsFixture.SchemasPath, "Linq.Expressions.Serialization.Json")) {
                                    Indent = true,
                                    IndentSize = 4,
                                    AddComments = true,
                                    AllowTrailingCommas = true,
                                    ValidateInputDocuments = validate,
                                }
                                : new() {
                                    Indent = true,
                                    IndentSize = 4,
                                    AddComments = true,
                                    AllowTrailingCommas = true,
                                    ValidateInputDocuments = validate,
                                };

    public static readonly TheoryData<string, ValidateDocuments, string, bool, Type?> TransformLoadDocumentData = new()
    {
        { TestLine(), ValidateDocuments.Always, "__NullObjectInvalid.json", false, typeof(InvalidOperationException) },
        { TestLine(), ValidateDocuments.Always, "__NullObjectInvalid.json", true, typeof(SchemaValidationErrorsException) },
        { TestLine(), ValidateDocuments.Always, "NullObject.json", false, typeof(InvalidOperationException) },
        { TestLine(), ValidateDocuments.Always, "NullObject.json", true, null },
        { TestLine(), ValidateDocuments.Never, "__NullObjectInvalid.json", false, null },
        { TestLine(), ValidateDocuments.Never, "__NullObjectInvalid.json", true, null },
        { TestLine(), ValidateDocuments.Never, "NullObject.json", false, null },
        { TestLine(), ValidateDocuments.Never, "NullObject.json", true, null },
        { TestLine(), ValidateDocuments.IfSchemaPresent, "__NullObjectInvalid.json", false, null },
        { TestLine(), ValidateDocuments.IfSchemaPresent, "__NullObjectInvalid.json", true, typeof(SchemaValidationErrorsException) },
        { TestLine(), ValidateDocuments.IfSchemaPresent, "NullObject.json", false, null },
        { TestLine(), ValidateDocuments.IfSchemaPresent, "NullObject.json", true, null },
    };

    [Theory]
    [MemberData(nameof(TransformLoadDocumentData))]
    public void JsonFileShouldLoadTest(string _, ValidateDocuments validate, string fileName, bool reloadSchema, Type? exceptionType)
    {
        ResetReloadSchemas(reloadSchema, validate);

        var transform = new ExpressionJsonTransform(_fixture.Options);
        using var stream = new FileStream(Path.Combine(JsonTestFilesPath, fileName), FileMode.Open, FileAccess.Read);
        var deserialize = () => transform.Deserialize(stream);

        if (exceptionType is null)
            deserialize.Should().NotThrow().Which.Should().NotBeNull();
        else
        if (exceptionType == typeof(SerializationException))
            deserialize.Should().Throw<SerializationException>();
        else
        if (exceptionType == typeof(AggregateException))
            deserialize.Should().Throw<AggregateException>();
        else
        if (exceptionType == typeof(InvalidOperationException))
            deserialize.Should().Throw<InvalidOperationException>();
        else
        if (exceptionType == typeof(SchemaValidationErrorsException))
            deserialize.Should().Throw<SchemaValidationErrorsException>();
        else
            Assert.Fail("Unexpected exception.");
    }
}
