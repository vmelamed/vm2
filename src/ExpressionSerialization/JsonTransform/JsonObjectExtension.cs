namespace vm2.ExpressionSerialization.JsonTransform;
static class JsonObjectExtension
{
    public static JsonObject Add(this JsonObject jObj, JElement? element)
    {
        if (element.HasValue)
            jObj.Add(element.Value.Key, element.Value.Value);
        return jObj;
    }
}
