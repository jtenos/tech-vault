```csharp
// <PackageReference Include="Otp.NET" Version="1.4.0" />
// <PackageReference Include="QRCoder" Version="1.6.0" />

using OtpNet;
using QRCoder;
using System;
using System.Drawing;

class Program
{
	static string _code;

	static void GenerateCode()
	{
		byte[] key = KeyGeneration.GenerateRandomKey(20);
		_code = Base32Encoding.ToString(key);

		string uriString = new OtpUri(
			schema: OtpType.Totp,
			secret: _code,
			user: "alice@google.com",
			issuer: "ACME Co"
		).ToString();

		Console.WriteLine(uriString);

		const string qrCodeFile = @"C:\Temp\qrcode.png";

		QRCodeGenerator qrGenerator = new QRCodeGenerator();
		QRCodeData qrCodeData = qrGenerator.CreateQrCode(uriString, QRCodeGenerator.ECCLevel.Q);

		var qrCode = new QRCode(qrCodeData);
		qrCode.GetGraphic(
			5,
			Color.Black,
			Color.White,
			drawQuietZones: true
		).Save(qrCodeFile);

		Console.WriteLine(qrCodeFile);
	}

	static void Main()
	{
		Console.WriteLine("Generating Code...");
		GenerateCode();

		while (true)
		{
			Console.Write("Enter Code: ");
			string enteredCode = Console.ReadLine();
			var totp = new Totp(Base32Encoding.ToBytes(_code));
			bool isValid = totp.VerifyTotp(enteredCode, out long timeWindowUsed, VerificationWindow.RfcSpecifiedNetworkDelay);
			Console.WriteLine($"Valid: {isValid}");
		}
	}
}
```