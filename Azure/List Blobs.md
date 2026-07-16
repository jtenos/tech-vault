# C# project
## csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <RootNamespace>AzureList</RootNamespace>
    <PublishAot>True</PublishAot>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Azure.Storage.Blobs" Version="12.22.2" />
  </ItemGroup>

</Project>
```

## pubxml
```xml
<?xml version="1.0" encoding="utf-8"?>
<!-- https://go.microsoft.com/fwlink/?LinkID=208121. -->
<Project>
  <PropertyGroup>
    <Configuration>Release</Configuration>
    <Platform>Any CPU</Platform>
    <PublishDir>bin\Release\net10.0\publish\win-x64\</PublishDir>
    <PublishProtocol>FileSystem</PublishProtocol>
    <_TargetId>Folder</_TargetId>
    <TargetFramework>net10.0</TargetFramework>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <SelfContained>true</SelfContained>
    <PublishSingleFile>true</PublishSingleFile>
    <PublishReadyToRun>true</PublishReadyToRun>
  </PropertyGroup>
</Project>
```

## Program.cs
```csharp
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

const string ACCOUNT_NAME = "xx";
const string ACCOUNT_KEY = "xx";

StorageSharedKeyCredential credential = new(ACCOUNT_NAME, ACCOUNT_KEY);
Uri serviceUri = new($"https://{ACCOUNT_NAME}.blob.core.windows.net/");
BlobServiceClient serviceClient = new(serviceUri, credential);

long numBlobs = 0;

await foreach (BlobContainerItem container in serviceClient.GetBlobContainersAsync())
{
	Console.WriteLine($"\nContainer: {container.Name}");

	BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(container.Name);

	await foreach (BlobItem blob in containerClient.GetBlobsAsync())
	{
		long size = blob.Properties.ContentLength ?? 0;
		Console.WriteLine($"  {blob.Name} ({size:#,##0} bytes)");
		++numBlobs;
	}
}

Console.WriteLine($"DONE. {numBlobs:#,##0} blob(s) across all containers.");
Console.ReadLine();
```