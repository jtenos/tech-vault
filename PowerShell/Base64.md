# String to Base64
```powershell
param(
    [Parameter(Mandatory=$true)]
    [string]$String
)

$bytes = [System.Text.Encoding]::UTF8.GetBytes($String)
$base64 = [Convert]::ToBase64String($bytes)
Write-Output $base64
```

# Base64 to String
```powershell
param(
    [Parameter(Mandatory=$true)]
    [string]$Base64String
)

function IsBase64String {
    param([string]$String)
    if ([string]::IsNullOrEmpty($String)) { return $false }
    try {
        [Convert]::FromBase64String($String) | Out-Null
        return $true
    } catch {
        return $false
    }
}

if (-not (IsBase64String $Base64String)) {
    Write-Error "Input is not a valid Base64 string."
    exit 1
}

try {
    $bytes = [Convert]::FromBase64String($Base64String)
    $output = [System.Text.Encoding]::UTF8.GetString($bytes)
    Write-Output $output
} catch {
    Write-Error "Failed to decode Base64 string."
    exit 1
}
```