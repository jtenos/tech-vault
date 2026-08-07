```powershell
$MagickExe = "magick"

$Extensions = "jpg", "jpeg", "heic"

$Extensions | ForEach-Object {
    $Extension = $_
    Write-Output "Working on $Extension"

    Get-ChildItem -Filter "*.$Extension" | Sort-Object -Property Name | ForEach-Object {
        Write-Output $_.Name
        $BaseFileName = [System.IO.Path]::GetFileNameWithoutExtension($_.Name)
        [ref]$PicDate = [DateTime](Get-Date)
   
        $Provider = [System.Globalization.CultureInfo]::InvariantCulture
        $Style = [System.Globalization.DateTimeStyles]::None
        if ([DateTime]::TryParseExact($BaseFileName.Substring(0, 13), "yyyyMMdd\_HHmm", $Provider, $Style, $PicDate)) {
            $FormattedDate = $PicDate.Value.ToString("ddd, MMM dd, yyyy \a\t hh:mm") + $PicDate.Value.ToString("tt").ToLower()
            $Arguments = "convert" `
                + " -verbose" `
                + " $($_.Name)" `
                + " -gravity SouthEast" `
                + " -font Arial" `
                + " -pointsize 81" `
                + " -fill gold" `
                + " -stroke black" `
                + " -annotate +81+81" `
                + " `"$FormattedDate`"" `
                + " stamped.$BaseFileName.jpg"
            Start-Process -FilePath $MagickExe -ArgumentList "$Arguments" -Wait -NoNewWindow
        }
    }
}

Write-Output "DONE"
Read-Host
```