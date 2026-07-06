```csharp
string fullFileName = @"C:\images\myimage.png";
byte[] bytes = File.ReadAllBytes(fullFileName);
string ext = Path.GetExtension(fullFileName).Replace(".", "");
string base64 = Convert.ToBase64String(bytes);

string html = $"<img src=\"data:image/{ext};base64,{base64}\">";
```