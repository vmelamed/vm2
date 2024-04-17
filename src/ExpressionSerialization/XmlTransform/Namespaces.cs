namespace vm2.ExpressionSerialization.XmlTransform;

static class Namespaces
{
    /// <summary>
    /// The XML namespace of the W3C schema definition - http://www.w3.org/2001/XMLSchema
    /// </summary>
    public static readonly XNamespace Xsd  = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

    /// <summary>
    /// The XML namespace of the W3C instance schema definition - http://www.w3.org/2001/XMLSchema-instance
    /// </summary>
    public static readonly XNamespace Xsi  = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

    /// <summary>
    /// The XML namespace of the Microsoft serialization schema definition - http://schemas.microsoft.com/2003/10/Serialization/
    /// </summary>
    public static readonly XNamespace Ser  = XNamespace.Get("http://schemas.microsoft.com/2003/10/Serialization/");

    /// <summary>
    /// The XML namespace object representing the namespace of the data contracts - http://schemas.datacontract.org/2004/07/System
    /// </summary>
    public static readonly XNamespace Dcs = XNamespace.Get("http://schemas.datacontract.org/2004/07/System");

    /// <summary>
    /// The XML namespace object representing the namespace of the expression serialization - urn:schemas-vm-com:Linq.Expressions.Serialization
    /// </summary>
    public static readonly XNamespace Exs = XNamespace.Get("urn:schemas-vm-com:Linq.Expressions.Serialization");
}
