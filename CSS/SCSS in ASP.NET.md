## csproj
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <Target Name="ScssToCss" BeforeTargets="Build">
    <Exec Command="mkdir $(ProjectDir)wwwroot\css" Condition="!Exists('$(ProjectDir)wwwroot\css')" />
    <Exec Command="sass $(ProjectDir)Styles\site.scss $(ProjectDir)wwwroot\css\site.css" />
  </Target>

</Project>
```

## ~/Styles/\_colors.scss
```scss
$my-color: green;
```

## ~/Styles/site.scss
```scss
@import "colors";

.my-class {
	color: $my-color;
}
```

## ~/Pages/Shared/\_Layout.cshtml
```html
...
<head>
	<!--
	    Reference the .css file that the compilation step built.
	    The .css file is not in source control since it's built on the fly.	
     -->
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
</head>
<body>
	<div class="my-class">This is green text</div>
	...
</body>
</html>
```