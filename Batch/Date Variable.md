```batch
@echo off
for /f "delims=" %%a in (' powershell -Command "Get-Date -format yyyyMMdd" ') do set "dtformatted=%%a"
echo %dtformatted%
```