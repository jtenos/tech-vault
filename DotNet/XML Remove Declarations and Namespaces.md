```csharp
// Default: Lots of XML junk
var foo = new Foo { Name = "John Doe", Age = 34 };
using (var writer = XmlWriter.Create(Console.Out)) {
    new XmlSerializer(typeof (Foo)).Serialize(writer, foo);
}

// Better looking output
var ns = new XmlSerializerNamespaces(); ns.Add("", "");
var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
using (var writer = XmlWriter.Create(Console.Out, settings)) {
    new XmlSerializer(typeof (Foo)).Serialize(writer, foo, ns); 
}
/*
<Foo>
  <Name>John Doe</Name>
  <Age>34</Age>
</Foo>
*/
```