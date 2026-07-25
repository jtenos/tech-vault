```csharp
// .NET Framework, I don't think this works with current .NET

private void Go()
{
    WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
    bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
 
    if (!hasAdministrativeRight)
    {
        RunElevated(Application.ExecutablePath);
        this.Close();
        Application.Exit();
    }
     
    // Do your administrator stuff
}
 
private static bool RunElevated(string fileName)
{
    ProcessStartInfo processInfo = new ProcessStartInfo();
    processInfo.Verb = "runas";
    processInfo.FileName = fileName;
    try
    {
        Process.Start(processInfo);
        return true;
    }
    catch (Win32Exception)
    {
        //Do nothing. Probably the user canceled the UAC window
    }
    return false;
}
```