namespace vm2.ExpressionSerialization.XmlTests;
public class TransformDocumentTest()
{
    [Theory]
    [InlineData(ValidateDocuments.Always, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.Always, "NullObject.xml", false, typeof(InvalidOperationException))]
    [InlineData(ValidateDocuments.Always, "NullObjectInvalid.xml", true, typeof(AggregateException))]
    [InlineData(ValidateDocuments.Always, "NullObjectInvalid.xml", true, typeof(XmlSchemaValidationException))]
    [InlineData(ValidateDocuments.Never, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.Never, "NullObjectInvalid.xml", true, null)]
    [InlineData(ValidateDocuments.Never, "NullObject.xml", false, null)]
    [InlineData(ValidateDocuments.Never, "NullObjectInvalid.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObject.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObjectInvalid.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObjectInvalid.xml", true, typeof(XmlSchemaValidationException))]
    public void XmlFileShouldLoadTest(ValidateDocuments validate, string fileName, bool loadSchemas, Type? exceptionType)
    {
        var options = new Options() { ValidateInputDocuments = validate };

        Options.ResetSchemas();
        if (loadSchemas)
        {
            Options.SetSchemaLocation(Options.Ser, Path.Combine(TestsFixture.SchemasPath, "Microsoft.Serialization.xsd"));
            Options.SetSchemaLocation(Options.Dcs, Path.Combine(TestsFixture.SchemasPath, "DataContract.xsd"));
            Options.SetSchemaLocation(Options.Exs, Path.Combine(TestsFixture.SchemasPath, "Expression.xsd"));
        }

        var transform = new ExpressionTransform(options);
        using var stream = new FileStream(Path.Combine(TestsFixture.TestFilesPath, fileName), FileMode.Open, FileAccess.Read);
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
    [InlineData(ValidateDocuments.Always, "NullObjectInvalid.xml", true, typeof(AggregateException))]
    [InlineData(ValidateDocuments.Always, "NullObjectInvalid.xml", true, typeof(XmlSchemaValidationException))]
    [InlineData(ValidateDocuments.Never, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.Never, "NullObjectInvalid.xml", true, null)]
    [InlineData(ValidateDocuments.Never, "NullObject.xml", false, null)]
    [InlineData(ValidateDocuments.Never, "NullObjectInvalid.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObject.xml", true, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObject.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObjectInvalid.xml", false, null)]
    [InlineData(ValidateDocuments.IfSchemaPresent, "NullObjectInvalid.xml", true, typeof(XmlSchemaValidationException))]
    public async Task XmlFileShouldLoadTestAsync(ValidateDocuments validate, string fileName, bool loadSchemas, Type? exceptionType)
    {
        var options = new Options() { ValidateInputDocuments = validate };

        Options.ResetSchemas();
        if (loadSchemas)
        {
            Options.SetSchemaLocation(Options.Ser, Path.Combine(TestsFixture.SchemasPath, "Microsoft.Serialization.xsd"));
            Options.SetSchemaLocation(Options.Dcs, Path.Combine(TestsFixture.SchemasPath, "DataContract.xsd"));
            Options.SetSchemaLocation(Options.Exs, Path.Combine(TestsFixture.SchemasPath, "Expression.xsd"));
        }

        var transform = new ExpressionTransform(options);
        using var stream = new FileStream(Path.Combine(TestsFixture.TestFilesPath, fileName), FileMode.Open, FileAccess.Read);
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
