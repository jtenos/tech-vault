This script will retrieve all zip files in a directory, unzip them one at a time into new directories, then add those directories to a .7z file. Useful if you have a collection of zip files with mostly the same contents, where you’ll benefit from combining them together, but you don’t have enough disk space to unzip all of them at once.

```powershell


param(
    [Parameter(Position=0,Mandatory)]
    [string]$OutputBaseFileName
)

# Set these two constant values:

$SevenZipExe = "C:\Program Files\7-zip\7z.exe"
$Dir = "C:\temp\je"

$ZipFiles = Get-ChildItem -Path "$($Dir)\*" -Include "*.zip"
$OutputFile = "$($Dir)\$($OutputBaseFileName).7z"

$ZipFiles | ForEach-Object {
    $ZipFileName = $_.FullName
    $DirName = $_.FullName -replace "\.zip$", ""
    Start-Process -FilePath $SevenZipExe -ArgumentList "x `"$ZipFileName`" -o`"$DirName`"" -Wait -NoNewWindow
    Start-Process -FilePath $SevenZipExe -ArgumentList "a `"$OutputFile`" `"$DirName`"" -Wait -NoNewWindow
    Remove-Item -Recurse -Path $DirName
}
```