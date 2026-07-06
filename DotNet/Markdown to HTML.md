## csproj
```xml
<PackageReference Include="Markdig" Version="1.3.2" />
```
### cs
```csharp
using Markdig;

// This can go in DI
MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
	.UseAdvancedExtensions()
	.Build();

string markdown = """
	# Header 1
	## Header 2
	- List Item 1
	- LIst Item 2
	""";

string html = Markdown.ToHtml(markdown, pipeline);
```
