﻿namespace vm2.ExpressionSerialization.Json;

#if JSON_SCHEMA
using global::Json.Schema;
#else
using global::Newtonsoft.Json.Linq;
using global::Newtonsoft.Json.Schema;
#endif

/// <summary>
/// Class JsonOptions holds options that control certain aspects of the transformations to/from LINQ expressions from/to
/// JSON documents. Consider caching this object.
/// </summary>
public partial class JsonOptions : DocumentOptions
{
    /// <summary>
    /// The expression transformation JSON schemaUri
    /// </summary>
    public const string Exs = "urn:schemas-vm-com:Linq.Expressions.Serialization.Json";

    static readonly JsonStringEnumConverter _jsonStringEnumConverter = new();

    ReaderWriterLockSlim _syncSchema = new(LockRecursionPolicy.SupportsRecursion);
    bool _allowTrailingCommas = true;
    JsonSerializerOptions? _jsonSerializerOptions;

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
    /// Determines whether the expressions schemaUri <see cref="JsonOptions.Exs"/> was added.
    /// </summary>
    /// <returns><c>true</c> if [has expressions schemaUri] [the specified options]; otherwise, <c>false</c>.</returns>
    internal override bool HasExpressionSchema
    {
        get
        {
            using (_syncSchema.ReaderLock())
                return _schema is not null;
        }
    }

    /// <summary>
    /// Get or sets a value that indicates whether an extra comma at the end of a list of JSON values in an object or
    /// array is allowed (and ignored) within the JSON payload being deserialized.
    /// </summary>
    /// <value>The allow trailing commas.</value>
    public bool AllowTrailingCommas
    {
        get => _allowTrailingCommas;
        set
        {
            if (Change(_allowTrailingCommas != value))
                _allowTrailingCommas = value;
        }
    }

    /// <summary>
    /// Creates the json serializer options object appropriate for the JsonTransform.
    /// </summary>
    /// <returns>System.Text.Json.JsonSerializerOptions.</returns>
    public JsonSerializerOptions JsonSerializerOptions
        => _jsonSerializerOptions is not null && !Changed
                ? _jsonSerializerOptions
                : (_jsonSerializerOptions = new() {
                    AllowTrailingCommas = AllowTrailingCommas,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    Converters = { _jsonStringEnumConverter },
                    MaxDepth = 1000,
                    NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString,
                    PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    ReferenceHandler = ReferenceHandler.Preserve,
                    UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode,
                    UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
                    TypeInfoResolver = null,
                    WriteIndented = Indent,
                });

    /// <summary>
    /// Gets the json node options.
    /// </summary>
    public static JsonNodeOptions JsonNodeOptions { get; } = new() {
        PropertyNameCaseInsensitive = false,
    };

    /// <summary>
    /// Gets the json writer options based on these options
    /// </summary>
    /// <value>The json writer options.</value>
    public JsonWriterOptions JsonWriterOptions => new() {
        Indented = Indent,
        SkipValidation = false,
    };

    /// <summary>
    /// Gets the json document options.
    /// </summary>
    public static JsonDocumentOptions JsonDocumentOptions { get; } = new() {
        AllowTrailingCommas = true,
        CommentHandling = JsonCommentHandling.Skip,
        MaxDepth = 1000
    };

#if JSON_SCHEMA
    JsonSchema? _schema;

    static readonly EvaluationOptions _evaluationOptions = new() {
        OutputFormat            = OutputFormat.Hierarchical,
        RequireFormatValidation = true,
    };

    /// <summary>
    /// Loads the schema from the specified URL.
    /// </summary>
    /// <param name="schemaFilePath">The location of the schema file.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>Load
    public void LoadSchema(string schemaFilePath)
    {
        using (_syncSchema.WriterLock())
            _schema = JsonSchema.FromFile(schemaFilePath);
    }

    /// <summary>
    /// Evaluates the specified jsonNode against the schema (by default the expressions schema).
    /// </summary>
    /// <param name="json">The JSON text to validate.</param>
    /// <returns>Json.Schema.EvaluationResults.</returns>
    public void Validate(string json)
        => Validate(JsonNode.Parse(json) ?? throw new SchemaValidationErrorsException("Invalid JSON text."));

    /// <summary>
    /// Evaluates the specified jsonNode against the schema (by default the expressions schema).
    /// </summary>
    /// <param name="jsonNode">The jsonNode.</param>
    /// <returns>Json.Schema.EvaluationResults.</returns>
    public void Validate(JsonNode jsonNode)
    {
        if (!MustValidate)
            return;

        EvaluationResults results;

        using (_syncSchema.ReaderLock())
            results = _schema is not null
                            ? _schema.Evaluate(jsonNode, _evaluationOptions)
                            : throw new InvalidOperationException("The schema is not loaded. Use JsonOptions.LoadSchema.");

        if (results.IsValid)
            return;

        var writer = new StringWriter();

        writer.WriteLine($"The validation of the JSON/YAML against the schema \"{Exs}\" failed:\n");
        WriteResults(writer, results, 1);
        writer.Flush();

        throw new SchemaValidationErrorsException(writer.ToString());
    }

    static void WriteResults(TextWriter writer, EvaluationResults results, int indent)
    {
        if (results.HasErrors && results.Errors is not null)
            foreach (var (k, v) in results.Errors)
                writer.WriteLine($"{new string(' ', indent * 2)}{k}: {v} ({results.InstanceLocation})");

        if (results.HasDetails && results.Details is not null)
            foreach (var nestedResults in results.Details)
                WriteResults(writer, nestedResults, indent + 1);
    }
#else
    JSchema? _schema;

    /// <summary>
    /// Loads the schema from the specified URL.
    /// </summary>
    /// <param name="schemaFilePath">The location of the schema file.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>Load
    public void LoadSchema(string schemaFilePath)
    {
        string schemaText;

        using (var file = File.OpenText(schemaFilePath))
            schemaText = file.ReadToEnd();

        using (_syncSchema.WriterLock())
            _schema = JSchema.Parse(schemaText);

    }

    /// <summary>
    /// Evaluates the specified jsonNode against the schema (by default the expressions schema).
    /// </summary>
    /// <param name="node">The JSON node.</param>
    /// <returns>Json.Schema.EvaluationResults.</returns>
    public void Validate(JsonNode node)
    {
        if (!MustValidate)
            return;

        string json;
        using (var stream = new MemoryStream())
        using (var writer = new Utf8JsonWriter(
                                    stream,
                                    new JsonWriterOptions() {
                                        Indented = Indent,
                                        SkipValidation = false,
                                    }))
        {
            node.WriteTo(writer);
            writer.Flush();

            json = Encoding.UTF8.GetString(new ArraySegment<byte>(stream.GetBuffer(), 0, (int)stream.Position));
        }

        Validate(json);
    }

    /// <summary>
    /// Evaluates the specified jsonNode against the schema (by default the expressions schema).
    /// </summary>
    /// <param name="json">The JSON string.</param>
    /// <returns>Json.Schema.EvaluationResults.</returns>
    public void Validate(string json)
    {
        if (!MustValidate)
            return;

        using (_syncSchema.WriterLock())
        {
            Debug.Assert(_schema is not null);

            JObject.Parse(json).IsValid(_schema, out IList<string>? messages);
            if (messages.Count == 0)
                return;

            throw new SchemaValidationErrorsException($"The validation of the JSON/YAML against the schema \"{Exs}\" failed:\n  {string.Join("\n  ", messages)}\n");
        }
    }
#endif

    /// <summary>
    /// Builds a JSON comment object with the specified comment text if comments are enabled.
    /// </summary>
    /// <param name="comment">The comment.</param>
    /// <returns>The comment object System.Nullable&lt;XComment&gt;.</returns>
    internal JElement? Comment(string comment)
        => AddComments ? new JElement(Shared.Conventions.Vocabulary.Comment, comment) : null;

    /// <summary>
    /// Builds an JSON comment object with the text of the expression if comments are enabled.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>System.Nullable&lt;XComment&gt;.</returns>
    internal JElement? Comment(Expression expression)
        => AddComments ? Comment($" {expression} ") : null;

    /// <summary>
    /// Adds the expression comment to the specified JSON container if comments are enabled.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="expression">The expression.</param>
    internal void AddComment(JsonObject parent, Expression expression)
        => parent.Add(AddComments ? new JElement(Shared.Conventions.Vocabulary.Comment, $" {expression} ") : null);

    /// <summary>
    /// Adds the comment to the specified JSON container if comments are enabled.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="comment">The comment.</param>
    internal void AddComment(JElement parent, string comment)
        => parent.Add(Comment($" {comment} "));

    /// <summary>
    /// Adds the expression comment to the specified JSON container if comments are enabled.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="expression">The expression.</param>
    internal void AddComment(JElement parent, Expression expression)
        => parent.Add(AddComments ? Comment($" {expression} ") : null);

    /// <summary>
    /// Creates an <see cref="XComment" /> with the human readable name of the <paramref name="type" /> if comments are enabled.
    /// The type must be non-basic type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The comment as System.Nullable&lt;XComment&gt;.</returns>
    internal JElement? TypeComment(Type type)
    => AddComments &&
       TypeNames != TypeNameConventions.AssemblyQualifiedName &&
       (!type.IsBasicType() && type != typeof(object) || type.IsEnum)
                ? Comment($" {Transform.TypeName(type, TypeNames)} ")
                : null;
}