namespace vm2.ExpressionSerialization.XmlTests.ToExpressionTests;

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

    [Theory]
    [InlineData(ValidateDocuments.Always, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.Always, "NullObject.xml", false, typeof(InvalidOperationException))]
    [InlineData(ValidateDocuments.Always, "__NullObjectInvalid.xml", true, typeof(AggregateException))]
    [InlineData(ValidateDocuments.Always, "__NullObjectInvalid.xml", true, typeof(XmlSchemaValidationException))]
    [InlineData(ValidateDocuments.Never, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.Never, "__NullObjectInvalid.xml", true, null)]
    [InlineData(ValidateDocuments.Never, "NullObject.xml", false, null)]
    [InlineData(ValidateDocuments.Never, "__NullObjectInvalid.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObject.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "__NullObjectInvalid.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "__NullObjectInvalid.xml", true, typeof(XmlSchemaValidationException))]
    public void XmlFileShouldLoadTest(ValidateDocuments validate, string fileName, bool loadSchemas, Type? exceptionType)
    {
        var options = new XmlOptions() { ValidateInputDocuments = validate };

        ResetReloadSchemas(loadSchemas);

        var transform = new ExpressionXmlTransform(options);
        using var stream = new FileStream(Path.Combine(XmlTestsFixture.TestFilesPath, "Constants", fileName), FileMode.Open, FileAccess.Read);
        var deserialize = () => transform.Deserialize(stream);

        if (exceptionType is null)
            deserialize.Should().NotThrow().Which.Should().NotBeNull();
        else
        if (exceptionType == typeof(XmlSchemaValidationException))
            deserialize.Should().Throw<XmlSchemaValidationException>();
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
    [InlineData(ValidateDocuments.Always, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.Always, "NullObject.xml", false, typeof(InvalidOperationException))]
    [InlineData(ValidateDocuments.Always, "__NullObjectInvalid.xml", true, typeof(AggregateException))]
    [InlineData(ValidateDocuments.Always, "__NullObjectInvalid.xml", true, typeof(XmlSchemaValidationException))]
    [InlineData(ValidateDocuments.Never, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.Never, "__NullObjectInvalid.xml", true, null)]
    [InlineData(ValidateDocuments.Never, "NullObject.xml", false, null)]
    [InlineData(ValidateDocuments.Never, "__NullObjectInvalid.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObject.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "__NullObjectInvalid.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "__NullObjectInvalid.xml", true, typeof(XmlSchemaValidationException))]
    public async Task XmlFileShouldLoadTestAsync(ValidateDocuments validate, string fileName, bool loadSchemas, Type? exceptionType)
    {
        var options = new XmlOptions() { ValidateInputDocuments = validate };

        ResetReloadSchemas(loadSchemas);

        var transform = new ExpressionXmlTransform(options);
        using var stream = new FileStream(Path.Combine(XmlTestsFixture.TestFilesPath, "Constants", fileName), FileMode.Open, FileAccess.Read);
        var deserialize = async () => await transform.DeserializeAsync(stream);

        if (exceptionType is null)
            (await deserialize.Should().NotThrowAsync()).Which.Should().NotBeNull();
        else
        if (exceptionType == typeof(XmlSchemaValidationException))
            await deserialize.Should().ThrowAsync<XmlSchemaValidationException>();
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
