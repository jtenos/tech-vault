```csharp
// <PackageReference Include="Azure.Storage.Blobs" Version="12.24.0" />

BlobContainerClient container = new(azureConnectionString, containerName);
FileInfo inputFile = new(inputFileName);
BlobClient blob = container.GetBlobClient($"/{inputFile.Name}");

await blob.UploadAsync(inputFile.FullName, new BlobUploadOptions
    {
        ProgressHandler = (long value) =>
        {
            logger.LogInformation("{FileName}: Bytes uploaded: {value} ({percent}%)",
                inputFileName, value.ToString(":#,##0"), (value * 100.0 / inputFile.Length).ToString("0"));
        }
    }, cancellationToken);
	```