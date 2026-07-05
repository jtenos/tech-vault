```powershell
param(
        [Parameter(Position=0,Mandatory)]
        [string]$Path
)

Write-Output ""
Write-Output "Info for $Path`:"
Write-Output ""

$Details = Get-ChildItem -Path $Path | Measure-Object -Property Length -Sum
Write-Output "Num Files: $($Details.Count.ToString('#,##0'))"
Write-Output "Num Bytes: $($Details.Sum.ToString('#,##0'))"
Write-Output ""

# PS C: \Users\myname> Get-FolderSize -Path C:\ Temp
# Info for C:\Temp:
# Num Files:
# 5
# Num Bytes: 493,404,517
```