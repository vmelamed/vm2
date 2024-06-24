namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public class TransformLoadDocumentTest()
{
    static void ResetReloadSchemas(bool loadSchemas)
    {
        if (loadSchemas)
            XmlOptions.SetSchemasLocations(
                new Dictionary<string, string?> {
                    [XmlOptions.Ser] = Path.Combine(XmlTestsFixture.SchemasPath, "Microsoft.Serialization.xsd"),
                    [XmlOptions.Dcs] = Path.Combine(XmlTestsFixture.SchemasPath, "DataContract.xsd"),
                    [XmlOptions.Exs] = Path.Combine(XmlTestsFixture.SchemasPath, "Linq.Expressions.Serialization.xsd"),
                }, true);
        else
            XmlOptions.ResetSchemas();
    }

    public static readonly TheoryData<string, ValidateDocuments, string, bool, Type?> TransformLoadDocumentData = new()
    {
        { TestLine(), ValidateDocuments.Always, "__NullObjectInvalid.xml", false, typeof(InvalidOperationException) },
        { TestLine(), ValidateDocuments.Always, "__NullObjectInvalid.xml", true, typeof(SchemaValidationErrorsException) },
        { TestLine(), ValidateDocuments.Always, "NullObject.xml", false, typeof(InvalidOperationException) },
        { TestLine(), ValidateDocuments.Always, "NullObject.xml", true, null },
        { TestLine(), ValidateDocuments.Never, "__NullObjectInvalid.xml", false, null },
        { TestLine(), ValidateDocuments.Never, "__NullObjectInvalid.xml", true, null },
        { TestLine(), ValidateDocuments.Never, "NullObject.xml", false, null },
        { TestLine(), ValidateDocuments.Never, "NullObject.xml", true, null },
        { TestLine(), ValidateDocuments.IfSchemaPresent, "__NullObjectInvalid.xml", false, null },
        { TestLine(), ValidateDocuments.IfSchemaPresent, "__NullObjectInvalid.xml", true, typeof(SchemaValidationErrorsException) },
        { TestLine(), ValidateDocuments.IfSchemaPresent, "NullObject.xml", false, null },
        { TestLine(), ValidateDocuments.IfSchemaPresent, "NullObject.xml", true, null },
    };

    [Theory]
    [MemberData(nameof(TransformLoadDocumentData))]
    public void XmlFileShouldLoadTest(string _, ValidateDocuments validate, string fileName, bool loadSchemas, Type? exceptionType)
    {
        var options = new XmlOptions() { ValidateInputDocuments = validate };

        ResetReloadSchemas(loadSchemas);

        var transform = new ExpressionXmlTransform(options);
        using var stream = new FileStream(Path.Combine(XmlTestsFixture.TestLoadPath, fileName), FileMode.Open, FileAccess.Read);
        var deserialize = () => transform.Deserialize(stream);

        if (exceptionType is null)
            deserialize.Should().NotThrow().Which.Should().NotBeNull();
        else
        if (exceptionType == typeof(SchemaValidationErrorsException))
            deserialize.Should().Throw<SchemaValidationErrorsException>();
        else
        if (exceptionType == typeof(AggregateException))
            deserialize.Should().Throw<AggregateException>();
        else
        if (exceptionType == typeof(InvalidOperationException))
            deserialize.Should().Throw<InvalidOperationException>();
        else
            Assert.Fail("Unexpected exception.");
    }

    [Theory]
    [MemberData(nameof(TransformLoadDocumentData))]
    public async Task XmlFileShouldLoadTestAsync(string _, ValidateDocuments validate, string fileName, bool loadSchemas, Type? exceptionType)
    {
        var options = new XmlOptions() { ValidateInputDocuments = validate };

        ResetReloadSchemas(loadSchemas);

        var transform = new ExpressionXmlTransform(options);
        using var stream = new FileStream(Path.Combine(XmlTestsFixture.TestLoadPath, fileName), FileMode.Open, FileAccess.Read);
        var deserialize = async () => await transform.DeserializeAsync(stream);

        if (exceptionType is null)
            (await deserialize.Should().NotThrowAsync()).Which.Should().NotBeNull();
        else
        if (exceptionType == typeof(SchemaValidationErrorsException))
            await deserialize.Should().ThrowAsync<SchemaValidationErrorsException>();
        else
        if (exceptionType == typeof(AggregateException))
            await deserialize.Should().ThrowAsync<AggregateException>();
        else
        if (exceptionType == typeof(InvalidOperationException))
            await deserialize.Should().ThrowAsync<InvalidOperationException>();
        else
            Assert.Fail("Unexpected exception.");
    }
}
