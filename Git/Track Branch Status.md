# Track branch status

If you type git status on a branch and don’t see the relationship to the origin, then you need to set the default upstream:

```bash
git branch --set-upstream-to origin/MyBranch
```

Thanks to this StackOverflow answer

https://stackoverflow.com/a/37669540/111266
