```powershell
Import-Module SqlServer

$dbs = @(
  @{ Server = "SERVER1"; Database = "Database1" },
  @{ Server = "SERVER2"; Database = "Database2" }
)

foreach ($db in $dbs) {
  Write-Output "Working on $($db.Server).$($db.Database)'

  # Assumes Windows authentication
  Invoke-SqlCmd `
    -ServerInstance $db.Server `
    -Database $db.Database `
    -Query "EXEC sp_updatestats;" `
    -Verbose 
}
```