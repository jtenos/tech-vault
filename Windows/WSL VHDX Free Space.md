> I had a ton of data on my WSL disk, and I moved it off using File Explorer. Now there's a lot of space taken up by a vhdx file in my AppData/Local/Packages/CanonicalGroupLimited.Ubuntu directory on my C: drive. How can I clear that up since my Ubuntu WSL machine isn't using it anymore.

This is a very common issue with WSL2. When you add data to your Ubuntu machine, the virtual hard disk (.vhdx file) automatically expands on your Windows host. However, when you delete or move files out of it, the Linux filesystem marks that space as free, but Windows doesn’t automatically shrink the virtual disk file.  To get that hard drive space back, you need to manually compact the virtual disk.

⚠️ Important First Step: Shut Down WSLBefore compacting the disk, you must ensure that your WSL environment is completely shut down and not locking the file.

```shell
wsl --shutdown
```  

Steps to compact the disk - check the file path for the VHDX file, should be easy to find with WizTree:

```shell
Microsoft Windows [Version 10.0.26200.8524]
(c) Microsoft Corporation. All rights reserved.
C:\Users\joe1>diskpart
Microsoft DiskPart version 10.0.26100.1150
Copyright (C) Microsoft Corporation.
On computer: MYCOMPUTERNAME
DISKPART> select vdisk file="C:\Users\joe1\AppData\Local\Packages\CanonicalGroupLimited.Ubuntu_79rhkp1fndgsc\LocalState\ext4.vhdx"
DiskPart successfully selected the virtual disk file.
DISKPART> compact vdisk
 100 percent completed
DiskPart successfully compacted the virtual disk file.
DISKPART> exit
Leaving DiskPart...
C:\Users\joe1>
```