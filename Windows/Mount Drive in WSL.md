To mount a drive with a letter, including a network drive:

```
sudo mkdir /mnt/g
sudo mount -t drvfs G: /mnt/g
```

To mount a network share:

```
sudo mkdir /mnt/folder
sudo mount -t '\\server\folder' /mnt/folder
```

With specific credentials:

```
net.exe use f: \\server\folder /user:Bob LetMeIn
```

Unmounting:

```
sudo umount /mnt/folder
```

Source: Chris Hoffman on HowToGeek

https://www.howtogeek.com/331053/how-to-mount-removable-drives-and-network-locations-in-the-windows-subsystem-for-linux/

