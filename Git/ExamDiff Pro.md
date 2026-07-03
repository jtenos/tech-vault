# **ExamDiff Pro git**

you can just use the following command:

```shell
git config --global merge.tool examdiff
```

Integrating ExamDiff Pro with Git, Mercurial, Subclipse, SourceSafe, TFS, and other version control systems

```shell
git config --global diff.tool edp
git config --global diff.guitool edp
git config --global difftool.edp.cmd "\"<Path to ExamDiff.exe>\" \"\$REMOTE\" \"\$LOCAL\" //nh"
git config --global difftool.prompt false
```

\<Path to ExamDiff.exe\> should replaced by the full path to the ExamDiff.exe file, with each backslash ('') replaced by a forward slash ('/').

Note: Every double quote ('"') and dollar sign ('$') in these command lines, inside the surrounding quotes, must be escaped with a preceding backslash. (It looks a bit convoluted like that, but it's needed to make it be correctly interpreted by the git command line.)

Example:

```shell
git config --global difftool.edp.cmd "\"C:/Program Files/ExamDiff Pro/ExamDiff.exe\" \"\$REMOTE\" \"\$LOCAL\" //nh"
```

Using ExamDiff Pro as an External Merge Tool In your Git command window, enter the following commands:

```shell
git config --global merge.tool edp
git config --global mergetool.edp.cmd "\"<Path to ExamDiff.exe>\" //merge \"\$REMOTE\" \"\$BASE\" \"\$LOCAL\"
     //o:\"\$MERGED\" //dn1:\"Theirs\" //dn2:\"Base\" //dn3:\"Yours\" //dno:\"Output\" //nh"
git config --global merge.prompt false
git config --global merge.keepBackup false
git config --global merge.trustExitCode false
```
