# Git Worktrees for Parallel Work on a Single Feature

Slightly different from [Feature Branches](./Worktrees-Feature-Branches.md) since this assumes only a single remote branch, and multiple local branches that never get pushed to the server.

### The constraint

Git will not check out the same branch in two worktrees at once. `git worktree add <path> feature-a` fails with `fatal: 'feature-a' is already checked out at ...` if `feature-a` is live in another worktree.

To work on several parts of one feature in parallel (frontend, backend, database), create one feature branch plus a short-lived sub-branch per work area, each in its own worktree, then merge the sub-branches back into the feature branch.

### Project layout

This assumes the bare-repo layout from the multi-branch guide. If starting fresh:

```bash
cd repos
mkdir my-project
cd my-project
git clone --bare git@github.com:username/my-project.git .bare
echo "gitdir: ./.bare" > .git
git config remote.origin.fetch "+refs/heads/*:refs/remotes/origin/*"
git fetch origin
```

### Create the feature branch

Create `feature-a` from `main` on the server (server-first, as before), then fetch:

```bash
git fetch origin
```

### Create the integration worktree

```bash
git worktree add feature-a feature-a
```

This creates the local `feature-a` branch tracking `origin/feature-a` and gives you a worktree to merge the parts into and push from.

### Create a worktree and sub-branch per work area

```bash
git worktree add -b feature-a-frontend ./frontend feature-a
git worktree add -b feature-a-backend  ./backend  feature-a
git worktree add -b feature-a-database ./database feature-a
```

Each sub-branch starts at the current tip of `feature-a`. The directory name and branch name are independent; they are named separately here for clarity.

Resulting structure:

```
my-project/
├── .bare/      # the bare repository
├── .git        # gitdir: ./.bare
├── feature-a/  # branch feature-a (integration target)
├── frontend/   # branch feature-a-frontend
├── backend/    # branch feature-a-backend
└── database/   # branch feature-a-database
```

### Work in each worktree independently

```bash
cd frontend
echo hello > hello.txt
git add hello.txt
git commit -m "Frontend changes"
```

Repeat in `backend` and `database`. Each commits to its own sub-branch in its own directory, so there is no index or checkout collision.

### Merge the parts back into the feature branch

```bash
cd ../feature-a
git merge feature-a-frontend
git merge feature-a-backend
git merge feature-a-database
```

Because the three areas touch different files, these merges normally apply cleanly. The first may fast-forward; later ones create merge commits. Resolve conflicts here if any area changed shared files.

### Push the feature branch

```bash
git push
```

No arguments are needed: `feature-a` was created server-first, so the worktree set up tracking against `origin/feature-a` automatically.

### Remove the worktrees and sub-branches when done

```bash
git worktree remove frontend
git worktree remove backend
git worktree remove database

git branch -d feature-a-frontend
git branch -d feature-a-backend
git branch -d feature-a-database
```

`git worktree remove` refuses to remove a worktree with uncommitted changes unless you pass `--force`; commit or discard first. `git branch -d` deletes only fully-merged branches, which acts as a safety check that each part landed in `feature-a`.

### Alternative: detached worktrees

If you do not want sub-branches, `git worktree add --detach ./frontend feature-a` checks out `feature-a`'s commit in detached HEAD. Commits made there are not on any branch and must be brought onto `feature-a` with `git cherry-pick` (or `git merge` of the detached commit hash). The sub-branch approach above is cleaner when three lines of work diverge and then converge, so it is the recommended default.