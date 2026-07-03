```batch
@echo off
set /p ans=Are you sure? 
if "%ans%" == "y" goto :dowork
if "%ans%" == "Y" goto :dowork
goto :eof
:dowork
echo Work is being done...
```
