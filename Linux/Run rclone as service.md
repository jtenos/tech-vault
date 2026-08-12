Install rclone and configure according to official instructions.

Starting in foreground:

```bash
rclone mount MY_SERVICE_NAME: ~/MY_SERVICE_NAME --vfs-cache-mode writes
```

To run as a service:

```bash
mkdir -p ~/.config/systemd/user
nano ~/.config/systemd/user/rclone-MY_SERVICE_NAME.service
```

```toml
[Unit]
Description=Rclone mount for MY_SERVICE_NAME
After=network-online.target

[Service]
Type=notify
ExecStart=/usr/bin/rclone mount MY_SERVICE_NAME: /home/YOURUSER/MY_SERVICE_NAME --vfs-cache-mode writes
ExecStop=/bin/fusermount -u /home/YOURUSER/MY_SERVICE_NAME
Restart=on-failure

[Install]
WantedBy=default.target
```

```bash
systemctl --user enable rclone-MY_SERVICE_NAME
systemctl --user start rclone-MY_SERVICE_NAME
loginctl enable-linger $USER
```
