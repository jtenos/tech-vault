```batch
rem One at a time:

set args=%1
shift
:start
if [%1] == [] goto done
set args=%args% %1
shift
goto start
:done
.\SomeExecutable.exe %args%

rem Pass all at once:

call second.bat %*
```
