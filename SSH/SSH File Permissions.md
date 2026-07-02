If migrating SSH files from another machine, need to reset the permissions and ownership:

```bash
chmod 700 ~/.ssh
chmod 600 ~/.ssh/*
chmod 644 ~/.ssh/*.pub ~/.ssh/known_hosts
chown -R $USER:$USER ~/.ssh
```