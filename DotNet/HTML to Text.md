## csproj
```xml
<ItemGroup>
    <PackageReference Include="NUglify" Version="1.13.2" />
  </ItemGroup>
```

## cs
```csharp
public static class Extensions
{
	public static IHtmlContent HtmlToText<T>(
		this IHtmlHelper<T> html,
		string htmlText)
	{
		return new StringHtmlContent(
			Uglify.HtmlToText($"<div>{htmlText}</div>").ToString()
		);
	}
}
```

## cshtml
```csharp
@page
@model IndexModel

@{
    const string THE_TEXT = @"
        This is <b>some text</b> 
        <div>more text</div>
        <script>alert('danger!');</script> 
    ";
}

@* Will safely be HTML-encoded *@
@THE_TEXT

@* Will display the raw HTML including the dangerous stuff *@
@Html.Raw(THE_TEXT)

@* Only renders the plain text *@
@Html.HtmlToText(THE_TEXT)
```