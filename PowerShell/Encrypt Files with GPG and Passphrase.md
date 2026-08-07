```powershell
# This script will retrieve all files in the "converted" directory
# and encrypt them, placing the encrypted files in the "output"
# directory with a GUID file name

$InputDir = Resolve-Path "converted"
$OutputDir = Resolve-Path "output"
$GpgExe = "C:\Program Files (x86)\GnuPG\bin\gpg.exe"

$Files = Get-ChildItem -Path $InputDir
$Files | ForEach-Object {
    $FileName = $_.FullName
    # Write-Output $FileName
    $GUID = New-Guid
    $EncryptedFileName = Join-Path -Path $OutputDir -ChildPath $GUID.ToString("N")
    # Write-Output $EncryptedFileName
    Start-Process -FilePath $GpgExe -ArgumentList "--batch --symmetric --passphrase XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX --cipher-algo aes256 --compress-algo 0 --output `"$EncryptedFileName`" `"$FileName`"" -Wait -NoNewWindow
    Write-Output "$($_.Name)`t$($GUID.ToString("N"))"
}
```