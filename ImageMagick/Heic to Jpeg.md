```powershell
$InputFolder = "C:\Temp\HeicInput"
$OutputFolder = "C:\Temp\JpgOutput"

Get-ChildItem -Path $InputFolder -Filter *.heic | ForEach-Object {
    $InputFile = $_.FullName
    $OutputFile = Join-Path $OutputFolder ($_.BaseName + ".jpg")
    magick convert "$InputFile" "$OutputFile"
}
```