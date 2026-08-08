
I’ve had amazingly bad luck with different programs and operating systems, just trying to create bootable thumb drives. In Windows, when creating a Windows bootable USB, this seems to work, with the instructions found here:

https://social.technet.microsoft.com/Forums/windows/en-US/ffdb9465-8f63-4304-8af4-38a17ce023fb/how-to-create-a-bootable-usb-for-windows-8-64bit-installation?forum=w8itproinstall

No promises that this will work everywhere.

```shell
C:\> diskpart
Diskpart> list disk
Diskpart> sel disk 1  <-- (the number of your usb)
Diskpart> clean
Diskpart> create part pri
Diskpart> format fs=ntfs quick
Diskpart> Active
Diskpart> Assign
```

Now copy the install files to the drive, and use it to boot.