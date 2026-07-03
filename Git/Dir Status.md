```powershell
foreach ($dir in Get-ChildItem -Directory) {
    Set-Location $dir
    git status
    Set-Location ..
}
```

```powershell
foreach ($dir in (Get-ChildItem -Directory)) {
  Set-Location $dir
  Write-Host $dir.Name
  git fetch --all
  git status --short --branch
  Set-Location ..
}
```