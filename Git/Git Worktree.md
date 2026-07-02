Common Commands

## Create a new branch and directory based on HEAD
```shell
git worktree add <path>
```

## Check out an existing branch in a new directory
```shell
git worktree add <path> <branch-name>
```

## Explicitly name a new branch based on an existing branch
```shell
git worktree add -b <new-branch-name> <path> <based-on-branch>
```
