## %USERPROFILE%\Mercurial.ini 

```toml
[tortoisehg]
vdiff = edpd

[extensions]
extdiff =

[extdiff]
cmd.edp = <Path to ExamDiff.exe>
opts.edpd = /nh 

[ui]
merge = edpm

[merge-tools]
edpm.args = /merge $other $base $local /o:$output /dn1:"Theirs" /dn2:"Base" /dn3:"Yours" /dno:"Output" /nh
edp.executable = <Path to ExamDiff.exe>
edpm.gui = True 


```

## mercurial.ini

```toml
[extdiff] 
cmd.examdiff = c:\Program Files (x86)\ExamDiff Pro\ExamDiff.exe 

[tortoisehg] 
vdiff = examdiff 

[extensions] 
hgext.extdiff = 

[merge-tools] 
filemerge.executable = C:\Program Files\TortoiseSVN\bin\TortoiseMerge.exe 
filemerge.args = /base:$base /theirs:$other /mine:$local /merged:$output
```