namespace vm2.ExpressionSerialization.JsonTransform;

using System;

using Json.Schema;

/// <summary>
/// Class XmlOptions holds options that control certain aspects of the transformations to/from LINQ expressions from/to 
/// JSON documents. Consider caching this object.
/// </summary>
public partial class JsonOptions : DocumentOptions
{
    /// <summary>
    /// The expression transformation JSON schemaUri
    /// </summary>
    public const string Exs = "urn:schemas-vm-com:Linq.Expressions.Serialization.Json";

    JsonSchema? _schema;

    /// <summary>
    /// Gets the loaded expression serialization schema.
    /// </summary>
    /// <value>The schemas.</value>
    public JsonSchema Schema => _schema ?? throw new InvalidOperationException("The schema must be loaded with LoadSchemaAsync.");

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonOptions"/> class. The schema must be subsequently loaded with
    /// <see cref="LoadSchema(string)"/>.
    /// </summary>
    public JsonOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonOptions"/> class.
    /// </summary>
    public JsonOptions(string filePath)
        => LoadSchema(filePath);

    /// <summary>
    /// Loads the schema from the specified URL.
    /// </summary>
    /// <param name="schemaFilePath">The location of the schema file.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public JsonSchema LoadSchema(string schemaFilePath)
    {
        return _schema = JsonSchema.FromFile(schemaFilePath);
    }

    /// <summary>
    /// Determines whether the expressions schemaUri <see cref="JsonOptions.Exs"/> was added.
    /// </summary>
    /// <returns><c>true</c> if [has expressions schemaUri] [the specified options]; otherwise, <c>false</c>.</returns>
    internal override bool HasExpressionsSchema => Schema is not null;

    /// <summary>
    /// Get or sets a value that indicates whether an extra comma at the end of a list of JSON values in an object or 
    /// array is allowed (and ignored) within the JSON payload being deserialized.
    /// </summary>
    /// <value>The allow trailing commas.</value>
    public bool AllowTrailingCommas { get; set; } = true;

    /// <summary>
    /// Creates the json serializer options object appropriate for the JsonTransform.
    /// </summary>
    /// <returns>System.Text.Json.JsonSerializerOptions.</returns>
    public JsonSerializerOptions JsonSerializerOptions
    {
        get
        {
            var options = new JsonSerializerOptions()
            {
                AllowTrailingCommas             = AllowTrailingCommas,
                Converters                      =
                {
                    new JsonStringEnumConverter(),
                },
                Encoder                         = JavaScriptEncoder.Default,
                NumberHandling                  = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
                PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
                ReadCommentHandling             = JsonCommentHandling.Skip,
                ReferenceHandler                = ReferenceHandler.Preserve,
                UnknownTypeHandling             = JsonUnknownTypeHandling.JsonNode,
                UnmappedMemberHandling          = JsonUnmappedMemberHandling.Skip,
                // TypeInfoResolver
                WriteIndented                   = Indent,
            };

            return options;
        }
    }

    /// <summary>
    /// Gets the json writer options based on these options
    /// </summary>
    /// <value>The json writer options.</value>
    public JsonWriterOptions JsonWriterOptions => new() {
        Indented = Indent,
        SkipValidation = false,
    };

    /// <summary>
    /// Evaluates the specified jsonNode against the schema (by default the expressions schema).
    /// </summary>
    /// <param name="jsonNode">The jsonNode.</param>
    /// <returns>Json.Schema.EvaluationResults.</returns>
    public void Validate(JsonNode jsonNode)
    {
        var result = Schema.Evaluate(
                                jsonNode,
                                new EvaluationOptions
                                {
                                    OutputFormat            = OutputFormat.Flag,
                                    RequireFormatValidation = true,
                                });

        if (result.IsValid)
            return;

        var writer = new StringWriter();

        writer.WriteLine($"The validation of the JSON/YAML against the schema \"{Exs}\" failed:");

        static void writeResults(EvaluationResults results, TextWriter writer, int indent)
        {
            if (results.HasErrors && results.Errors is not null)
                foreach (var (k, v) in results.Errors)
                    writer.WriteLine($"{new string(' ', indent * 2)}{k}: {v}");

            if (results.HasDetails && results.Details is not null)
                foreach (var nestedResults in results.Details)
                    writeResults(nestedResults, writer, indent + 1);
        }

        writeResults(result, writer, 1);
        writer.Flush();

        throw new SerializationException(writer.ToString());
    }

    /// <summary>
    /// Builds an XML comment object with the specified comment text if comments are enabled.
    /// </summary>
    /// <param name="comment">The comment.</param>
    /// <returns>The comment object System.Nullable&lt;XComment&gt;.</returns>
    internal JElement? Comment(string comment)
        => AddComments ? new JElement(Conventions.Vocabulary.Comment, comment) : null;

    /// <summary>
    /// Builds an XML comment object with the text of the expression if comments are enabled.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>System.Nullable&lt;XComment&gt;.</returns>
    internal JElement? Comment(Expression expression)
        => AddComments ? Comment($" {expression} ") : null;

    /// <summary>
    /// Adds the expression comment to the specified XML container if comments are enabled.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="expression">The expression.</param>
    internal void AddComment(JsonObject parent, Expression expression)
    {
        if (AddComments)
            parent.Add(Conventions.Vocabulary.Comment, $" {expression} ");
    }

    /// <summary>
    /// Adds the comment to the specified XML container if comments are enabled.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="comment">The comment.</param>
    internal void AddComment(JElement parent, string comment)
    {
        if (AddComments)
            parent.Add(Comment($" {comment} ")!);
    }

    /// <summary>
    /// Adds the expression comment to the specified XML container if comments are enabled.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="expression">The expression.</param>
    internal void AddComment(JElement parent, Expression expression)
    {
        if (AddComments)
            parent.Add(Comment($" {expression} ")!);
    }

    /// <summary>
    /// Creates an <see cref="XComment" /> with the human readable name of the <paramref name="type" /> if comments are enabled.
    /// The type must be non-basic type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The comment as System.Nullable&lt;XComment&gt;.</returns>
    internal JElement? TypeComment(Type type)
        => TypeNames != TypeNameConventions.AssemblyQualifiedName &&
           (!type.IsBasicType() && type != typeof(object) || type.IsEnum)
                    ? Comment($" {Transform.TypeName(type, TypeNames)} ")
                    : null;
}