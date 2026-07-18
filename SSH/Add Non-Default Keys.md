Add this to bashrc, for any key generated with ssh-keygen which isn't a default name, and therefore doesn't get picked up by default. In this case, I already had an id_ed25519, so when I generated another one named id_ed25519_1, it didn't work until I added this to bashrc:

```shell
eval `ssh-agent`
ssh-add ~/.ssh/id_ed25519_1
```