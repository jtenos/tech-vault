```csharp
// <PackageReference Include="Azure.Storage.Blobs" Version="12.24.0" />

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

const string CS = @"PASTE FROM AZURE PORTAL";

BlobContainerClient containerClient = new BlobContainerClient(CS, CONTAINER_NAME);

await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(prefix: "SomePath/"))
{
    BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
    await blobClient.SetAccessTierAsync(AccessTier.Hot);
    Console.WriteLine($"Converted {blobItem.Name} to Hot tier.");
}
```