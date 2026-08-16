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
Wants=network-online.target
After=network-online.target

[Service]
Type=notify
ExecStart=/usr/bin/rclone mount MY_SERVICE_NAME: /home/YOURUSER/MY_SERVICE_NAME \
  --vfs-cache-mode full \
  --vfs-cache-max-age 24h \
  --dir-cache-time 72h \
  --poll-interval 15s \
  --vfs-read-chunk-size 64M \
  --vfs-read-chunk-size-limit 1G
ExecStop=/usr/bin/fusermount3 -uz /home/YOURUSER/MY_SERVICE_NAME
Restart=on-failure
RestartSec=10s

[Install]
WantedBy=default.target
```

```bash
systemctl --user daemon-reload
systemctl --user enable rclone-MY_SERVICE_NAME
systemctl --user start rclone-MY_SERVICE_NAME
loginctl enable-linger $USER
```
