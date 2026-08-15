```csharp
using ImageMagick;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

uint width = config.GetValue<uint>("Width");
uint quality = config.GetValue<uint>("Quality");

foreach (string arg in args)
{
    if (arg.EndsWith(".heic", StringComparison.OrdinalIgnoreCase))
    {
        FileInfo inputFile = new(arg);
        if (inputFile.Exists)
        {
            ConvertHeicToJpg(inputFile, width, quality);
            Console.WriteLine($"Converted {inputFile.Name} to JPG format with a width of {width} pixels at {quality} quality.");
        }
        else
        {
            Console.WriteLine($"File not found: {inputFile.FullName}");
        }
    }
    else
    {
        Console.WriteLine($"Unsupported file format: {arg}");
    }
}

static void ConvertHeicToJpg(FileInfo inputFile, uint width, uint quality)
{
    using MagickImage image = new(inputFile);
    image.Format = MagickFormat.Jpeg;
    image.Resize(new MagickGeometry(width, 0));
    image.Quality = quality;
    image.Write(Path.ChangeExtension(inputFile.FullName, ".jpg"));
}


/*
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>net9.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="Magick.NET-Q16-AnyCPU" Version="14.6.0" />
        <PackageReference Include="Magick.NET.Core" Version="14.6.0" />
        <PackageReference Include="Microsoft.Extensions.Configuration.FileExtensions" Version="9.0.5" />
        <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.5" />
        <PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="9.0.5" />
    </ItemGroup>

    <ItemGroup>
        <None Update="appsettings.json">
            <CopyToOutputDirectory>Always</CopyToOutputDirectory>
        </None>
    </ItemGroup>

</Project>
*/

/*
{
    "Quality": 50,
    "Width": 400
}
*/
```