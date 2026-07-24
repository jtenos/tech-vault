```csharp
// <PackageReference Include="QRCoder" Version="1.6.0" />

using QRCoder;

QRCodeGenerator qrGenerator = new();

QRCodeData qrCodeData = qrGenerator.CreateQrCode(
    SOME_URL, QRCodeGenerator.ECCLevel.Q);

QRCode qrCode = new(qrCodeData);

// Adjust these until you find what you like
qrCode.GetGraphic(
    20, Color.FromArgb(0, 138, 196), Color.White,
    drawQuietZones: false, icon: (Bitmap)Image.FromFile("the-icon.png"),
    iconSizePercent: 20, iconBorderWidth: 25
).Save("output.png");
```