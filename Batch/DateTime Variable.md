```batch
@echo off
for /f "delims=" %%a in (' powershell -Command "Get-Date -format yyyyMMddHHmmss" ') do set "dtformatted=%%a"
echo %dtformatted%
```