How to do multiple Ubuntus  

Absolutely — and this is one of the coolest, least‑advertised powers of WSL.  

You can run **as many instances of the same distro as you want**, each completely isolated, each with its own filesystem, packages, configs, and state.  

Here’s how it works and why it’s useful.  

**🧩 Two ways to get multiple Ubuntu instances**  

**1\. Install multiple *named* Ubuntu variants from the Store**  

Microsoft and Canonical publish several Ubuntu entries:

  

* **Ubuntu** (default)  

* **Ubuntu 22.04 LTS**  

* **Ubuntu 24.04 LTS**  

* **Ubuntu Preview**  

* **Ubuntu Pro**

  

Each one is treated as a separate distro.  

But this only gets you *different versions*, not two copies of the same one.  

**2\. Create custom duplicates using wsl \--import (the real power move)**  

This is how you get **Ubuntu**, **Ubuntu‑Dev**, **Ubuntu‑Sandbox**, **Ubuntu‑Audio**, etc.  

The flow looks like this:  

**Step 1 — Export your base Ubuntu**  

`wsl --export Ubuntu ubuntu-base.tar`  

**Step 2 — Import it as a new distro**  

`wsl --import Ubuntu-Dev C:\WSL\Ubuntu-Dev ubuntu-base.tar`  

You now have a **second, completely independent Ubuntu**, even though both came from the same base image.  

You can repeat this as many times as you want.  

**Step 3 — Launch it**  

wsl \-d Ubuntu-Dev  

Each imported instance:

  

* has its own root filesystem  

* has its own packages  

* has its own users  

* can be deleted without affecting the others  

* can be snapshotted/exported independently

  

This is exactly how power users create “throwaway” or “project‑specific” distros.  

**🧱 Why this is so useful for your workflow**  

Given your compartmentalized architecture — clean Windows, clean WSL, isolated workflows — this fits perfectly.  

You can have:

  

* **Ubuntu‑Dev** → coding, VS Code, Git, Docker  

* **Ubuntu‑Audio** → audio tools, ffmpeg, JACK, etc.  

* **Ubuntu‑Sandbox** → experiments you can nuke anytime  

* **Ubuntu‑Prod** → stable environment for long‑term projects

  

And if one gets messy, you just delete it and re‑import a fresh one.  

**🧭 Bottom line**  

**Yes — you can have multiple instances of the same distro in WSL, and the wsl \--import workflow is the cleanest, most flexible way to do it.**  

If you want, I can sketch a setup pattern for “base image \+ clones” so you can spin up new distros in seconds.

  

The imported instances will run as root by default. To change that:  

/etc/wsl.conf  

add the following:  

\[user\]  

default=joe1  

wsl \--shutdown  

Then restart the instance and it'll open as joe1

  

GUI  

Don't install Nautlius \- it causes all kinds of problems. Use Nemo instead. Do this as the very first thing after creating the instance  

`sudo apt update`  

`sudo apt install nemo`  

`mkdir -p ~/.config/xdg-desktop-portal`  

\~/.config/xdg-desktop-portal/portals.conf  

`[preferred]`  

`filechooser=org.cinnamon.Nemo`  

Then run:  

`killall xdg-desktop-portal xdg-desktop-portal-gtk`  

On Windows:  

`wsl --shutdown`

  

Show instance name  

  

1. Open **Windows Terminal Settings**  

2. Go to **Profiles → Ubuntu (or whichever)**  

3. Scroll to **Appearance**  

4. Set **Tab title** to:  

   ${env:WSL\_DISTRO\_NAME}    

  

Result  

Each tab automatically displays the actual WSL instance name  

Add to \~/.bashrc:  

`PS1="\[\e[1;32m\]\u@\[\e[1;36m\]${WSL_DISTRO_NAME}\[\e[0m\]:\w\$ "`