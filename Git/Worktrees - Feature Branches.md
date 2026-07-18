# Git Work Trees for Feature Branches

### Create project on remote server (GitHub, Gitlab, etc.)

Assume project is named `my-project`

### Create local directory manually:

```bash
cd repos
mkdir my-project
cd my-project
```

### Initialize and prepare

```bash
git clone --bare git@github.com:username/my-project.git .bare
```

This brings in the git database into a directory named `.bare`

### Root pointer

```bash
echo "gitdir: ./.bare" > .git
```

This allows you to use git normally inside the directory, without confusing your tools

### Remote Refspec

```bash
git config remote.origin.fetch "+refs/heads/*:refs/remotes/origin/*"
git fetch origin
```

This is a vital step, which sets up your repo so that local branches can point to remote branches in origin. Without this, you'll hit all kinds of failures.

### Create branches on server

You can create branches locally, but creating them on the server is simpler.

Inside of GitHub/Gitlab/etc., create the branch from main. Suppose it's named `feature-a`

### Create worktree and local branch

```bash
git worktree add feature-a feature-a
# OR
git worktree add --track -b feature-a ./feature-a origin/feature-a
```

The first is a shortcut for the second.

### Switch to a directory and work in it like normal

```bash
cd feature-a
echo hello > hello.txt
git add hello.txt
git commit -m "Adding hello"
git push
```

The branch is pushed just like normal.

### Remove worktree when done

```bash
git worktree remove feature-a
```
