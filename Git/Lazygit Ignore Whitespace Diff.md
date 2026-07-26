Temporarily, navigate to the diff panel, and press Ctrl-W to toggle ignoring whitespace changes.

For permanently setting the default, find the configuration directory:

```shell
lazygit --print-config-dir
```

Then edit `config.yml` and add the following:

```yaml
git:
  ignoreWhitespaceInDiffView: true
```
