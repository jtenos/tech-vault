# Get
```csharp
// <package id="MetadataExtractor" version="2.0.0" targetFramework="net40" />

static DateTime GetImageDate(string file)
{
    var directories = ImageMetadataReader.ReadMetadata(file);
    ExifSubIfdDirectory subIfdDirectory = null;
    try
    {
        subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
    }
    catch (Exception ex)
    {
    }
    DateTime? dateTime;
    try
    {
        dateTime = subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTime);
        return dateTime.Value;
    }
    catch (Exception ex)
    {
        try
        {
            dateTime = subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);
            return dateTime.Value;
        }
        catch (Exception ex1)
        {
            try
            {
                dateTime = subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTimeDigitized);
                return dateTime.Value;
            }
            catch (Exception ex2)
            {
                dateTime = new[] {
                    System.IO.File.GetCreationTime(file),
                    System.IO.File.GetLastWriteTime(file)
                }.Min();
                return dateTime.Value;
            }
        }
    }
}
```

# Set
```csharp
// If you have a photo that you need to add a timestamp to the EXIF information,
// you can use the following .NET code. Apparently there are a ridiculous number of
// formats and standards which are incompatible with each other and conflicting,
// but from what I’ve found, this should cover regular use cases.

// I’m sure there’s a better solution, but this requires that you have some
// unrelated image file that has the appropriate tags in it, in case your photo
// is missing the properties entirely. Not exactly a great solution, but it does seem to work.

static void Go(string oldFile, string newFile) {
    DateTime theDate = new DateTime(2009, 1, 15);
 
    Func<int, PropertyItem> getPropItem = id => {
        using (var image = Image.FromFile(@"c:\temp\IMG_6139.jpg")) { // Some unrelated image file that already has EXIF data
            return image.GetPropertyItem(id);
        }
    };
 
    using (var image = Image.FromFile(oldFile)) {
        PropertyItem propItem;
        int ID = 0x9004;
        try {
            propItem = image.GetPropertyItem(ID);
        } catch {
            propItem = getPropItem();
        }
        propItem.Value = Encoding.UTF8.GetBytes(theDate.ToString("yyyy\\:MM\\:dd HH\\:mm\\:ss") + "\0");
        image.SetPropertyItem(propItem);
 
        ID = 0x0132;
        try {
            propItem = image.GetPropertyItem(ID);
        } catch {
            propItem = getPropItem();
        }
        propItem.Value = Encoding.UTF8.GetBytes(theDate.ToString("yyyy\\:MM\\:dd HH\\:mm\\:ss") + "\0");
        image.SetPropertyItem(propItem);
 
        image.Save(newFile, ImageFormat.Jpeg);
    }
}



string oldFile = args[0];
string newFile = args[1];
DateTime theDate = DateTime.Parse(args[2]);

using var image = Image.FromFile(oldFile);
foreach (int propertyID in new[] { 0x9003, 0x9004, 0x0132 })
{
    if (image.PropertyIdList.Contains(propertyID))
    {
        image.RemovePropertyItem(propertyID);
    }
    PropertyItem propertyItem;

    // No public constructor, but it's a simple data type, so we'll cheat and use reflection
    propertyItem = (PropertyItem)typeof(PropertyItem).Assembly.CreateInstance(typeof(PropertyItem).FullName,
        ignoreCase: false,
        bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic,
        binder: null,
        args: null,
        culture: null,
        activationAttributes: null);

    propertyItem.Id = propertyID;
    propertyItem.Len = 20;
    propertyItem.Type = 2;
    Console.WriteLine("Creating new property");
    // }

    propertyItem.Value = Encoding.UTF8.GetBytes(theDate.ToString("yyyy\\:MM\\:dd HH\\:mm\\:ss") + "\0");
    image.SetPropertyItem(propertyItem);
}

image.Save(newFile, ImageFormat.Jpeg);
```