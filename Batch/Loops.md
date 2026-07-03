```batch
set /a num=0
:numbers
set /a num=%num%+1
if %num% EQU 10 (goto :next) else (echo %num%)
goto :numbers

:next
```
