There's some kind of overlap causing an issue.

Looks like WSL tries to be helpful by working with Windows - kill that:

`/etc/wsl.conf` add this:

```toml
[interop]
appendWindowsPath=false
```

```
wsl --shutdown
```

Start up WSL and now NVM should work:

```bash
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.5/install.sh | bash
nvm install --lts
nvm use --lts
source ~/.bashrc
```
