# Diff  

TortoiseSVN Settings | External Programs | Diff Viewer, select the External radio button, and enter the following into the Path field

```text
<Path to ExamDiff.exe> %base %mine /dn1:%bname /dn2:%yname /nh
```

# Merge  

 TortoiseSVN Settings | External Programs | Merge Tool, select the External radio button, and enter the following into the Path field

```text
<Path to ExamDiff.exe> /merge %theirs %base %mine /o:%merged /dn1:%tname /dn2:%bname /dn3:%yname /dno:%mname /nh
```