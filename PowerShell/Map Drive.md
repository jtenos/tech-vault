```powershell
New-PSDrive -Name 'P' `
-Root '\\my-server-name\my-share-name' `
-Persist `
-PSProvider FileSystem `
-Credential (
  Get-Credential -Credential 'MYDOMAIN\MYUSERNAME'
) `
-Scope Global
```