```csharp
public static class Serialization {
    private static readonly Encoding _defaultEncoding = Encoding.UTF8;
 
    public static T DeserializeXmlFile<T>(string fileName) {
        if (!File.Exists(fileName))
            throw new FileNotFoundException();
 
        var serializer = new XmlSerializer(typeof(T));
        using (var xmlReader = new XmlTextReader(fileName)) {
            return (T)serializer.Deserialize(xmlReader);
        }
    }
 
    public static T Deserialize<T>(TextReader textReader) {
        if (textReader == null)
            throw new ArgumentNullException("textReader");
 
        var serializer = new XmlSerializer(typeof(T));
        return (T)(serializer.Deserialize(textReader));
    }
 
    public static T Deserialize<T>(string serializedObject) {
        if (string.IsNullOrEmpty(serializedObject))
            return default(T);
 
        var serializer = new XmlSerializer(typeof(T));
        using (var objectReader = new StringReader(serializedObject)) {
            return (T)serializer.Deserialize(objectReader);
        }
    }
 
    public static void SerializeToXmlFile<T>(T objectToSerialize, string fileName) {
        SerializeToXmlFile(objectToSerialize, fileName, _defaultEncoding);
    }
 
    public static void SerializeToXmlFile<T>(T objectToSerialize, string fileName, Encoding encoding) {
        var serializer = new XmlSerializer(typeof(T));
        using (var xmlWriter = new XmlTextWriter(fileName, encoding) { Formatting = Formatting.Indented, IndentChar = '\t', Indentation = 1 }) {
            serializer.Serialize(xmlWriter, objectToSerialize);
        }
    }
 
    public static string Serialize<T>(T objectToSerialize, bool includeXmlDeclaration, bool indent) {
        var serializer = new XmlSerializer(typeof(T));
        var sb = new StringBuilder(0x1000);
 
        XmlTextWriter xmlWriter =
            includeXmlDeclaration
            ? new XmlTextWriter(new StringWriter(sb))
            : new XmlTextWriterWithoutDeclaration(new StringWriter(sb));
 
        if (indent) {
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.IndentChar = '\t';
            xmlWriter.Indentation = 1;
        }
 
        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add(string.Empty, string.Empty);
        serializer.Serialize(xmlWriter, objectToSerialize, namespaces);
        return sb.ToString();
    }
 
    private class XmlTextWriterWithoutDeclaration
        : XmlTextWriter
    {
        public XmlTextWriterWithoutDeclaration(TextWriter w)
            : base(w)
        { }
 
        public override void WriteStartDocument() { }
 
        public override void WriteStartDocument(bool standalone) { }
    }
}
```