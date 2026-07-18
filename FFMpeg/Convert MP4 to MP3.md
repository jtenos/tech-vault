Using FFmpeg for Audacity command line tool, here’s a simple batch file that lets you drag a series of MP4 files on top of the batch file, and it will convert them all to MP3.

```batch
@echo off
 
:start
if [%1] == [] goto done
set args=%1
"c:\Program Files (x86)\FFmpeg for Audacity\ffmpeg.exe" -i %args% %args%.mp3
shift
goto start
 
:done
pause
```