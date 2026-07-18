Make it prune local branches that have no remote, by default, instead of having to call `git fetch --prune`.

Keeps `origin/*` clean so that you can identify local branches that no longer have an upstream origin.

```shell
git config --global fetch.prune true
```