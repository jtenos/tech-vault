```powershell
$Dir = "C:\Temp"
$Url = "https://www.google.com/"

$Now = Get-Date -Format yyyyMMddHHmmss
$File1 = "google-$Now-1.html"

If (!(Test-Path -Path $Dir)) {
    New-Item -ItemType Directory -Path $Dir
}

Invoke-WebRequest -Uri $Url -OutFile "$Dir\$File1" -Verbose
```