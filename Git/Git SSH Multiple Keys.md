# SSH

Adding multiple Git SSH keys in Linux seems to not work by default. The default ones like id_ed25519 seem to be ok, but if you add another, it won't try to use it when using Git.

Try adding this to bashrc and I think this will work:

Assuming your second key is named id_ed25519_1:

```bash
eval `ssh-agent`
ssh-add ~/.ssh/id_ed25519_1
```
