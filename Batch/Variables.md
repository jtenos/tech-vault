```batch
set somevar=myvalue
echo %somevar%

echo %temp%

if defined somevar echo somevar=%somevar%
```

# Numbers

```batch
set /a num=0
:numbers
set /a num=%num%+1
if %num% EQU 10 (goto :next) else (echo %num%)
goto :numbers
:next
echo done
```
