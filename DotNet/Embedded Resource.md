You can compile binary or text files into the application assembly – here’s the basic idea.

Add the file to the project in Visual Studio, and in the Properties, mark it as “Embedded Resource”. The name of the resource is the default namespace for the project + relative namespace (folder structure) + filename. This is not the same as a resx file.

In the code, you can grab the file contents as follows:

```csharp
var stream = Assembly.GetExecutingAssembly()
    .GetManifestResourceStream(
        "WebApplication2.Resources.SomeExcelFile.xlsx"
    )
);
// Do whatever you want to with this stream
// copy to byte[], write to output, etc.

```