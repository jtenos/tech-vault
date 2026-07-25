```csharp
static string XmlWriterToString(Action<XmlWriter> action)
{
	var sb = new StringBuilder();
	using (var strWriter = new StringWriter(sb))
	{
		using (var xmlWriter = new XmlTextWriter(strWriter))
		{
			action(xmlWriter);
		}
	}
	return sb.ToString();
}


string s = XmlWriterToString(xmlWriter => {
	xmlWriter.WriteStartElement("start");
	xmlWriter.WriteAttributeString("type", "someval");
	xmlWriter.WriteStartElement("notes");

	string cdata = “some cdata stuff”;
	xmlWriter.WriteCData(cdata);
	xmlWriter.WriteEndElement(); // </notes>
	xmlWriter.WriteEndElement(); // </start>
});
```