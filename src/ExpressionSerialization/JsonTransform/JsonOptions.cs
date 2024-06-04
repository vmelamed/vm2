namespace vm2.ExpressionSerialization.JsonTransform;

using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

using Json.Schema;

/// <summary>
/// Class XmlOptions holds options that control certain aspects of the transformations to/from LINQ expressions from/to 
/// JSON documents.
/// </summary>
public partial class JsonOptions : DocumentOptions
{
    static object _schemaSync = new();

    /// <summary>
    /// The expression transformation JSON schemaUri
    /// </summary>
    public const string Exs = "urn:schemas-vm-com:Linq.Expressions.Serialization.Json";

    /// <summary>
    /// Gets the loaded schemas.
    /// </summary>
    /// <value>The schemas.</value>
    public static Dictionary<string, JsonSchema> Schemas { get; private set; } = [];

    /// <summary>
    /// Sets the schemaUri url.
    /// </summary>
    /// <param name="schemaUri">The schema identifier (most likely <see cref="Exs"/> which is not URL).</param>
    /// <param name="url">The location of the schema file.</param>
    public static void SetSchemaLocation(string schemaUri, string? url)
    {
        if (Schemas.ContainsKey(url ?? schemaUri))
            return;

        lock (_schemaSync)
        {
            if (Schemas.ContainsKey(url ?? schemaUri))
                return;

            using HttpClient client = new();
            Schemas[schemaUri] = JsonSchema.FromText(
                                                client
                                                    .GetStringAsync(url ?? schemaUri)
                                                    .ConfigureAwait(false)
                                                    .GetAwaiter()
                                                    .GetResult()
                                            );
        }
    }

    /// <summary>
    /// Determines whether the expressions schemaUri <see cref="JsonOptions.Exs"/> was added.
    /// </summary>
    /// <returns><c>true</c> if [has expressions schemaUri] [the specified options]; otherwise, <c>false</c>.</returns>
    internal override bool HasExpressionsSchema => Schemas.ContainsKey(Exs);

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
    public JsonSerializerOptions CreateJsonSerializerOptions()
            => new() {
                AllowTrailingCommas = AllowTrailingCommas,
                Encoder = JavaScriptEncoder.Default,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
                PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
                ReadCommentHandling = JsonCommentHandling.Skip,
                ReferenceHandler = ReferenceHandler.Preserve,
                UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
                // TypeInfoResolver
                WriteIndented = Indent,
            };
}
