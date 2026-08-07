```powershell
# 1. Check if the current session is already running as Administrator
$currentPrincipal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
$isDefinedAdmin = $currentPrincipal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isDefinedAdmin) {
    Write-Host "Requesting administrative privileges..." -ForegroundColor Yellow
    
    # 2. Re-launch the script with 'runas' to trigger the UAC prompt
    Start-Process powershell.exe -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs
    
    # 3. Exit the current non-admin session
    exit
}

# --- Everything below this line runs with Admin privileges ---
Write-Host "Success! Now running with full Administrative access." -ForegroundColor Green
Set-Location $PSScriptRoot

# Your admin-required code goes here:
# Get-Service | Stop-Service ...
pause
```