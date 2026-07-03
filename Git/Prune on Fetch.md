# Easiest - Global Git Setting:

~/.gitconfig:

```ini
[fetch]
	prune = true
```

-----

```bash
git config --global fetch.prune true
```

-----

Lazygit config.yml:

customCommands:
  - key: 'F'
    command: 'git fetch --prune --all'
    context: 'remotes'
    output: log
