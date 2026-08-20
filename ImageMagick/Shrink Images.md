```batch
@ECHO OFF
SETLOCAL
FOR /R %%G IN (*.jpg) DO CALL :process "%%G"
GOTO :end
 
:process
    SET _inname=%1
    SET _outname=%_inname:~0,-5%_1024.jpg"
 
    identify -format %%w %_inname% >width.txt
    identify -format %%h %_inname% >height.txt
 
    set /p width=<width.txt
    set /p height=<height.txt
    DEL width.txt
    DEL height.txt
    ECHO Processing %_inname% ...
 
    if %width% gtr %height% call :landscape %_inname% %_outname%
    if %height% geq %width% call :portrait %_inname% %_outname%
    rem convert %_inname% %_inname%
    EXIT /B
 
:landscape
    convert %_inname% -resize 1024x -quality 90 %_outname%
    EXIT /B
 
:portrait
    convert %_inname% -resize x1024 -quality 90 %_outname%
    EXIT /B
 
:end
```