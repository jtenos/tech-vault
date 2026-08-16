<img src="https://r2cdn.perplexity.ai/pplx-full-logo-primary-dark%402x.png" style="height:64px;margin-right:32px"/>

# Using a USB TP-Link Wi-Fi adapter. How can I be user in Linux Mint that it's being used? It has an LED light on it but it isn't lighting up - when I use it in Windows, it does light up.

The LED alone is not a reliable Linux test—some TP-Link adapters’ LEDs are controlled differently by their Linux driver. The definitive test is whether Linux detects the USB device, loads a Wi‑Fi driver, and exposes a wireless interface.

## Check it in three layers

Open a terminal, unplug the adapter, plug it back in, then run:

```bash
lsusb
```

You should see a new line containing `TP-Link`, or a chipset/vendor such as Realtek, MediaTek, or Qualcomm. Note the USB ID, for example `2357:0138`; TP-Link’s vendor ID is commonly `2357`.

Then see the plug-in and driver messages:

```bash
sudo dmesg --follow
```

With that running, unplug and reconnect the dongle. Look for lines mentioning:

- `USB device`
- a chipset/driver such as `rtl8xxxu`, `rtl88...`, `mt76...`, or `ath...`
- errors such as `firmware failed to load`, `device descriptor read`, or `failed`

Press `Ctrl+C` when done.

Finally, check whether NetworkManager sees a usable wireless interface:

```bash
nmcli device status
```

A working adapter normally appears as something like:

```text
DEVICE   TYPE  STATE         CONNECTION
wlan0    wifi  disconnected  --
```

If it says `connected`, it is already being used. If your built-in card also exists, you may see names such as `wlan0`, `wlp2s0`, or `wlx<MAC-address>` instead. `lsusb` verifies USB-level detection, while `nmcli` shows whether Linux has created a usable Wi‑Fi device.[^1_1][^1_2]

## Confirm which Wi‑Fi adapter is active

Run:

```bash
nmcli -f GENERAL.DEVICE,GENERAL.TYPE,GENERAL.DRIVER,GENERAL.HWADDR,GENERAL.STATE device show
```

This identifies each Wi‑Fi interface, its loaded driver, MAC address, and state. To see which interface is actually carrying your current network connection:

```bash
ip route get 1.1.1.1
```

Look for `dev ...`, such as:

```text
1.1.1.1 via 192.168.1.1 dev wlx001122334455 src 192.168.1.50
```

That `dev` interface is the adapter Linux is using for outbound traffic.

You can map that interface back to a physical adapter by comparing its MAC address with:

```bash
nmcli device show wlx001122334455 | grep -E 'GENERAL.HWADDR|GENERAL.DRIVER'
```


## Rule out Wi‑Fi blocking

```bash
rfkill list
```

For the TP-Link adapter, you want both fields to be `no`:

```text
Soft blocked: no
Hard blocked: no
```

If it is soft-blocked:

```bash
sudo rfkill unblock wifi
```

Then restart NetworkManager if necessary:

```bash
sudo systemctl restart NetworkManager
```

NetworkManager is ordinarily responsible for managing Wi‑Fi interfaces, and `rfkill` reveals software or hardware radio blocks.[^1_3][^1_4]

## Interpret the results

| Result | Meaning | Next step |
| :-- | :-- | :-- |
| Not present in `lsusb` | Linux is not seeing it as a USB device | Try a direct port rather than a hub, a different port, preferably USB 2 and USB 3, and inspect `dmesg` for USB errors |
| Present in `lsusb`, absent from `nmcli device status` | USB works, but no compatible driver/firmware created a Wi‑Fi interface | Capture the TP-Link USB ID and determine the underlying chipset/driver |
| Appears as `wifi` but is `unavailable` | Driver exists, but it may be rfkill-blocked or unable to initialize | Check `rfkill list` and `dmesg` |
| Appears as `wifi` and `disconnected` | Adapter is working and ready to connect | Select a Wi‑Fi network from the Mint network icon or use `nmcli` |
| Appears as `wifi` and `connected` | It is actively in use | Verify with `ip route get 1.1.1.1` |

## Most useful diagnostic bundle

If you want to determine the exact issue quickly, run this after inserting the adapter:

```bash
echo '=== USB ==='
lsusb

echo
echo '=== NetworkManager ==='
nmcli device status

echo
echo '=== Wi-Fi hardware / drivers ==='
sudo lshw -C network

echo
echo '=== RFKill ==='
rfkill list

echo
echo '=== Recent kernel messages ==='
sudo dmesg | tail -80
```

The important identifier is the `lsusb` line—not just “TP-Link,” since TP-Link sells devices with several different chipsets and even hardware revisions under similar model names. Linux compatibility depends on that actual chipset and the kernel/firmware support, not the LED behavior. TP-Link itself advises checking the specific model/version’s Linux support and driver availability.[^1_5]
<span style="display:none">[^1_10][^1_11][^1_12][^1_13][^1_14][^1_15][^1_6][^1_7][^1_8][^1_9]</span>

<div align="center">⁂</div>

[^1_1]: https://superuser.com/questions/760449/how-can-i-find-the-usb-wireless-adapter-into-the-dmesg-log-file

[^1_2]: https://bbs.archlinux.org/viewtopic.php?id=287124

[^1_3]: https://system76.com/support/articles/wireless

[^1_4]: https://www.reddit.com/r/linuxmint/comments/1sfjfbw/help_regarding_usb_wifi_adapter_driver/

[^1_5]: https://www.tp-link.com/us/support/faq/1076/

[^1_6]: https://github.com/morrownr/USB-WiFi/issues/355

[^1_7]: https://www.youtube.com/watch?v=lmJFD7xkKUk

[^1_8]: https://www.youtube.com/watch?v=ZZYQsjunjbs\&vl=en

[^1_9]: https://www.reallinuxuser.com/how-to-install-wifi-drivers-in-linux-mint/

[^1_10]: https://discourse.nixos.org/t/add-loadable-kernel-modules-for-usb-wi-fi/29761

[^1_11]: https://www.facebook.com/groups/linuxmintdesktop/posts/1798329761121924/

[^1_12]: https://www.reddit.com/r/raspberry_pi/comments/1b2ahg2/wifi_adapter_showing_in_dmesg_and_lsusb_but_not/

[^1_13]: https://forums.debian.net/viewtopic.php?t=159328

[^1_14]: https://forums.raspberrypi.com/viewtopic.php?t=49927

[^1_15]: https://www.youtube.com/watch?v=FL0E-rMMgX8


---

# Looks like it is being used, and the built-in one isn't. But it keeps disconnecting for a few seconds every few minutes. When on Windows, it's very reliable.

Since the same adapter is stable in Windows, this is most likely a Linux driver/firmware or power-management issue—not RF interference or a failing dongle. Start by disabling NetworkManager Wi‑Fi power saving; USB autosuspend is the next likely culprit.

## First: disable Wi‑Fi power saving

Create a NetworkManager override:

```bash
sudo tee /etc/NetworkManager/conf.d/99-wifi-powersave-off.conf >/dev/null <<'EOF'
[connection]
wifi.powersave = 2
EOF

sudo systemctl restart NetworkManager
```

`wifi.powersave = 2` means disabled. Reconnect to Wi‑Fi and let it run for a while. Aggressive Wi‑Fi power management is a known cause of periodic drops on Linux, including with TP-Link USB adapters.[^2_1][^2_2]

Check the live state, replacing the interface name with yours from `nmcli device status`:

```bash
iw dev
iwconfig wlxYOURINTERFACE
```

You want the latter to show:

```text
Power Management:off
```


## Capture the cause

Before changing more settings, run this in a terminal and leave it open while you wait for the next dropout:

```bash
sudo journalctl -kf
```

When it disconnects, look for one of these patterns:

- `deauthenticated`, `disassociated`, or `beacon loss` → Wi‑Fi link/driver issue.
- `USB disconnect`, `reset ... USB device`, or device re-enumeration → USB power/port issue.
- `failed to load firmware` or repeated driver errors → driver/firmware issue.

You can save the relevant lines after a drop with:

```bash
sudo journalctl -k --since "10 minutes ago" | tail -200
```


## If the log shows USB resets

Disable USB autosuspend as a **test** for the current boot:

```bash
echo -1 | sudo tee /sys/module/usbcore/parameters/autosuspend
```

If the periodic drops stop, make it permanent:

```bash
sudo nano /etc/default/grub
```

Find:

```bash
GRUB_CMDLINE_LINUX_DEFAULT="quiet splash"
```

Add `usbcore.autosuspend=-1` inside the quotes, for example:

```bash
GRUB_CMDLINE_LINUX_DEFAULT="quiet splash usbcore.autosuspend=-1"
```

Then apply it and reboot:

```bash
sudo update-grub
sudo reboot
```

This globally prevents the kernel from automatically suspending USB devices. It can slightly increase idle power use, but it is an effective diagnostic and workaround for USB Wi‑Fi devices that repeatedly reset.[^2_3][^2_4]

Also test the adapter in:

- A different direct motherboard USB port, not a hub or front-panel extension.
- A USB 2 port, if available; some 2.4 GHz adapters behave poorly in USB 3 environments.
- A short USB extension cable, moving it away from the PC chassis and USB 3 noise sources.


## Identify the exact chipset

TP-Link model labels do not uniquely identify Linux support—the **hardware revision and USB chipset** matter. Run:

```bash
lsusb -nn | grep -i -E 'tp-link|wireless|realtek|mediatek'
```

And:

```bash
nmcli -f GENERAL.DEVICE,GENERAL.DRIVER,GENERAL.HWADDR device show
```

If it reports a Realtek driver such as `rtl8xxxu`, `8812au`, `88x2bu`, or `8821cu`, the in-kernel driver can be less stable than a chipset-specific DKMS driver for some adapter revisions. Don’t install a random Realtek driver yet—use the exact USB ID and driver name from these commands to select the right one. USB ID/chipset identification is the appropriate basis for determining driver support.[^2_5][^2_6]

## Avoid these first

- Do not use TP-Link’s Windows driver in Mint.
- Do not install a generic “TP-Link Linux driver”; two adapters with similar branding can use different chips.
- Don’t disable autosuspend permanently until you have tested it or the logs show USB resets.

If disabling NetworkManager power saving does not solve it, paste the output of these two commands after a dropout:

```bash
lsusb -nn
sudo journalctl -k --since "10 minutes ago" | tail -200
```

<span style="display:none">[^2_10][^2_11][^2_12][^2_13][^2_14][^2_15][^2_16][^2_17][^2_7][^2_8][^2_9]</span>

<div align="center">⁂</div>

[^2_1]: https://www.baeldung.com/linux/wi-fi-frequent-disconnections-solution

[^2_2]: https://askubuntu.com/questions/1364239/tp-link-usb-wireless-adapter-keep-losing-data-every-several-minutes-without-disc

[^2_3]: https://www.reddit.com/r/linux/comments/t7pxpk/are_all_of_your_usb_devices_disconnecting/

[^2_4]: https://forum.endeavouros.com/t/prevent-sleep-suspend-mode-from-stopping-my-usb-wifi-adapter-kde-plasma/71996

[^2_5]: https://www.tp-link.com/us/support/faq/1076/

[^2_6]: https://discourse.nixos.org/t/add-loadable-kernel-modules-for-usb-wi-fi/29761

[^2_7]: https://askubuntu.com/questions/959010/wifi-usb-adapter-disconnecting-randomly

[^2_8]: https://bbs.archlinux.org/viewtopic.php?id=311351

[^2_9]: https://www.reddit.com/r/techsupport/comments/158w8z3/tplink_usb_wifi_adapter_continuously_disconnects/

[^2_10]: https://www.reddit.com/r/techsupport/comments/1362r8f/usb_wifi_adapter_gets_disconnect_every_510_minutes/

[^2_11]: https://unix.stackexchange.com/questions/413334/power-management-not-working-with-realtek-wifi-card

[^2_12]: https://www.youtube.com/watch?v=ynJOc0ZtS-8

[^2_13]: https://ubuntu-mate.community/t/stop-network-disconnecting-in-ubuntu/829

[^2_14]: https://www.facebook.com/groups/linuxmintdesktop/posts/2027886121499619/

[^2_15]: https://itechscotland.wordpress.com/2011/09/25/how-to-permanently-turn-off-wi-fi-power-management-in-ubuntu/

[^2_16]: https://bbs.archlinux32.org/viewtopic.php?id=3108

[^2_17]: https://www.youtube.com/watch?v=ZZYQsjunjbs\&vl=en


---

```text
usb 2-3: USB disconnect
...
rtw89_8852au ... timed out to flush queues
...
usb 2-3: new SuperSpeed USB device
...
loaded firmware rtw89/rtw8852a_fw.bin
```

The deauthentication is a consequence of the USB device vanishing. This strongly points to USB runtime power management, USB 3/controller behavior, or a driver/kernel issue—not your AP or normal Wi‑Fi settings.

## Do this first: test USB autosuspend

This disables USB autosuspend until the next reboot:

```bash
echo -1 | sudo tee /sys/module/usbcore/parameters/autosuspend
```

Verify it:

```bash
cat /sys/module/usbcore/parameters/autosuspend
```

Expected output:

```text
-1
```

Leave the computer running for at least as long as it normally takes to fail. If the drops stop, make it persistent:

```bash
sudo nano /etc/default/grub
```

Change the line that begins with `GRUB_CMDLINE_LINUX_DEFAULT` to include the parameter:

```bash
GRUB_CMDLINE_LINUX_DEFAULT="quiet splash usbcore.autosuspend=-1"
```

Then apply it and reboot:

```bash
sudo update-grub
sudo reboot
```

The kernel’s `usbcore.autosuspend` parameter controls the default USB-device autosuspend idle delay; `-1` disables automatic USB suspension.[^3_1][^3_2]

## Test a different USB path

Your log calls it a **SuperSpeed** device, so it is currently using USB 3. Test each option separately:

- Use a direct rear/motherboard USB port rather than a hub, monitor, docking station, or front-panel port.
- Prefer a **USB 2 port** for a stability test. The adapter will run at a lower USB link speed but its network throughput should still be fine for most connections.
- If only USB 3 ports are available, use a short USB extension lead to move the adapter away from the chassis and its USB 3 circuitry.
- Avoid sharing its hub/controller with high-traffic or high-power USB devices, where possible.

If the adapter stays connected on a USB 2 port but not USB 3, leave it there; that would isolate the problem to the SuperSpeed/xHCI path or this driver’s interaction with it.

## Update the kernel and firmware

Because `rtw89_8852au` is a relatively newer in-kernel driver, update Mint fully before replacing drivers manually:

```bash
sudo apt update
sudo apt full-upgrade
sudo reboot
```

Then inspect your running kernel and firmware package:

```bash
uname -r
apt policy linux-firmware
```

Use Mint’s **Update Manager → View → Linux Kernels** to install the newest supported kernel in your Mint release line. A newer kernel can contain important `rtw89` fixes, while `linux-firmware` supplies the `rtw8852a_fw.bin` firmware that your log shows loading successfully.

Do **not** install a random GitHub DKMS driver yet: the log proves you already have the correct device-specific in-kernel driver (`rtw89_8852au`), and an incompatible out-of-tree Realtek module can make the setup harder to recover.

## Keep Wi‑Fi power saving off

This is separate from USB autosuspend, so retain the NetworkManager setting from before:

```bash
sudo tee /etc/NetworkManager/conf.d/99-wifi-powersave-off.conf >/dev/null <<'EOF'
[connection]
wifi.powersave = 2
EOF

sudo systemctl restart NetworkManager
```

That may not cure the physical USB reset by itself, but it removes a second power-saving mechanism known to cause wireless disconnects.[^3_3][^3_4]

## Useful confirmation after testing

After the next failure—or after it stays stable—run:

```bash
sudo journalctl -k -b | grep -E -i 'usb 2-3|rtw89|disconnect|reset|timeout' | tail -100
```

If you see another `USB disconnect` followed by `new SuperSpeed USB device`, focus on the port/controller and the kernel/firmware update path. If USB disconnects disappear but you instead see only `deauthenticated`, then investigate radio/AP settings separately.
<span style="display:none">[^3_10][^3_11][^3_12][^3_13][^3_14][^3_15][^3_16][^3_17][^3_5][^3_6][^3_7][^3_8][^3_9]</span>

<div align="center">⁂</div>

[^3_1]: https://www.kernel.org/doc/Documentation/usb/power-management.txt

[^3_2]: https://www.reddit.com/r/linux/comments/t7pxpk/are_all_of_your_usb_devices_disconnecting/

[^3_3]: https://www.baeldung.com/linux/wi-fi-frequent-disconnections-solution

[^3_4]: https://askubuntu.com/questions/1364239/tp-link-usb-wireless-adapter-keep-losing-data-every-several-minutes-without-disc

[^3_5]: https://askubuntu.com/questions/185274/how-can-i-disable-usb-autosuspend-for-a-specific-device

[^3_6]: https://learn.microsoft.com/en-us/answers/questions/3147867/issue-with-usb-ports-turning-off

[^3_7]: https://github.com/ophub/amlogic-s9xxx-armbian/issues/2132

[^3_8]: https://bbs.archlinux.org/viewtopic.php?id=272328

[^3_9]: https://superuser.com/questions/704160/usb-drive-keeps-disconnecting-and-reconnecting

[^3_10]: https://www.reddit.com/r/buildapc/comments/123cdl2/generic_usb_hub_keeps_disconnecting_every_5/

[^3_11]: https://www.youtube.com/watch?v=IwfOcIz-S2Y

[^3_12]: https://www.youtube.com/watch?v=tUV-xdBsm5o

[^3_13]: https://community.frame.work/t/responded-usb-a-keeps-disconnecting/42556

[^3_14]: https://4ddig.tenorshare.com/usb-recovery/how-to-fix-usb-disconnecting-and-reconnecting.html

[^3_15]: https://community.home-assistant.io/t/how-to-disable-usb-autosuspend/392320

[^3_16]: https://discussion.fedoraproject.org/t/all-usb-devices-and-bluetooth-randomly-disconnected-until-reboot/147236

[^3_17]: https://bbs.archlinux.org/viewtopic.php?id=311351

