```csharp
using System.Diagnostics;
using System.Globalization;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

DirectoryInfo inputDir = new(config["inputDir"]!);
DirectoryInfo backupDir = new(config["backupDir"]!);
DirectoryInfo outputDir = new(config["outputDir"]!);

if (!inputDir.Exists) { throw new ApplicationException($"Input Dir [{inputDir}] does not exist"); }
if (!backupDir.Exists) { throw new ApplicationException($"Backup Dir [{backupDir}] does not exit"); }
if (!outputDir.Exists) { throw new ApplicationException($"Output Dir [{outputDir}] does not exit"); }

Console.WriteLine($"Input Dir: {inputDir.FullName}");
Console.WriteLine($"Backup Dir: {backupDir.FullName}");
Console.WriteLine($"Output Dir: {outputDir.FullName}");

List<FileInfo> inputFiles = inputDir.GetFiles("*.jpg")
    .Union(inputDir.GetFiles("*.jpeg"))
    .Union(inputDir.GetFiles("*.heic"))
    .ToList();

foreach (FileInfo fi in inputFiles)
{
    DateTime? imageDate = GetImageDate(fi);
    if (imageDate is null) { continue; }

    string backupFileName = Path.Combine(backupDir.FullName, $"backup_{DateTime.Now:yyyyMMddHHmmss}_{fi.Name}");
    Console.WriteLine($"Backing up {fi.FullName} to {backupFileName}");
    fi.CopyTo(backupFileName);

    string formattedDate = $"{imageDate:ddd, MMM dd, yyyy} at {imageDate:hh:mm}{imageDate.Value.ToString("tt").ToLower()}";

    string outputFullFileName = Path.Combine(outputDir.FullName, fi.Name);
    if (!outputFullFileName.EndsWith(".jpg", ignoreCase: true, CultureInfo.CurrentCulture)
        && !outputFullFileName.EndsWith(".jpeg", ignoreCase: true, CultureInfo.CurrentCulture))
    {
        outputFullFileName = Path.Combine(outputDir.FullName, Path.GetFileNameWithoutExtension(fi.Name) + ".jpg");
    }

    string[] arguments = {
        "convert",
        "-verbose",
        $"\"{fi.FullName}\"",
        "-gravity SouthEast",
        "-font Arial",
        "-pointsize 81",
        "-fill gold",
        "-stroke black",
        "-annotate +81+81",
        $"\"{formattedDate}\"",
        $"\"{outputFullFileName}\""
    };

    Process.Start("magick", string.Join(" ", arguments)).WaitForExit();

    fi.Delete();
}

Process.Start("explorer", outputDir.FullName);

static DateTime? GetImageDate(FileInfo file)
{
    using FileStream stream = file.OpenRead();
    var directories = ImageMetadataReader.ReadMetadata(stream);
    ExifSubIfdDirectory? subIfdDirectory = null;
    try
    {
        subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
    }
    catch { }

    if (subIfdDirectory is null)
    {
        return null;
    }

    int[] tagTypes = {
        ExifDirectoryBase.TagDateTime,
        ExifDirectoryBase.TagDateTimeOriginal,
        ExifDirectoryBase.TagDateTimeDigitized
    };
    foreach (int tagType in tagTypes)
    {
        try
        {
            DateTime? dt = subIfdDirectory.GetDateTime(tagType);
            if (dt is not null) { return dt; }
        }
        catch { }
    }

    return null;
}
```