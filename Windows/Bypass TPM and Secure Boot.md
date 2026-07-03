## Method 1: The 60-Second Registry Bypass (Easiest)

This tells the Windows installer to skip the hardware checks entirely. It's the fastest way to get moving without messing with configuration files.

On the screen that says "This PC can't run Windows 11", press Shift + F10. This will open a Command Prompt.

```
regedit
```

Navigate to:

```
HKEY_LOCAL_MACHINE \ SYSTEM \ Setup
```

New -> Key -> Name `LabConfig`

New -> DWORD (32-bit) Value -> Name `BypassTPMCheckDouble` -> 1

New -> DWORD (32-bit) Value -> Name `BypassSecureBootCheck` -> 1

Click Back to go back one step, then click Next again. Installation will proceed normally.
