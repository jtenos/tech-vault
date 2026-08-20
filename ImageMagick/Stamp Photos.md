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

```batch
rem dptnt.com/2009/04/how-to-add-date-time-stamp-to-jpeg-photos-using-free-software/ 

@ECHO OFF
SETLOCAL
FOR /R %%G IN (*.jpg) DO CALL :process "%%G"
GOTO :end 

:process
SET _inname=%1 

identify -format %%w %_inname% >dttmpfile 
set /p width=<dttmpfile
Set /a pointsize=%width%/50 
rem echo ZZ >> dttempfile
DEL dttmpfile
ECHO Processing %_inname% ...
convert %_inname% -gravity SouthEast -font Arial -pointsize %pointsize% -fill orange 
-annotate +%pointsize%+%pointsize% "%%[exif:DateTimeOriginal]" %_inname% 
EXIT /B 

:end 
```