```csharp
Process process = new()
{
	StartInfo = new()
	{
		CreateNoWindow = true,
		RedirectStandardOutput = true,
		RedirectStandardInput = true,
		UseShellExecute = false,
		FileName = @"C:\wkhtmltopdf\bin\wkhtmltopdf.exe",
		Arguments = @"c:\temp\1.htm c:\temp\1.pdf"
	},
	EnableRaisingEvents = true
};
process.OutputDataReceived += (sender, e) =>
{
	//Debug.Write(e.Data);
};
process.Start();
process.BeginOutputReadLine();
process.WaitForExit();
process.CancelOutputRead();
```