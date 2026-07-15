```csharp
var process = new Process
{
    StartInfo = new ProcessStartInfo
    {
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardInput = true,
        UseShellExecute = false,
        FileName = @"C:\temp\Hello.exe",
        Arguments = "world"
    },
    EnableRaisingEvents = true
};
process.OutputDataReceived += (sender, e) =>
{
    Debug.WriteLine(e.Data);
};
process.Start();
process.BeginOutputReadLine();
process.WaitForExit();
process.CancelOutputRead();
```
