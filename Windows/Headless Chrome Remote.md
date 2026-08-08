Google provides a headless setup flow that works through another device’s browser, and the host installer on macOS does not require Chrome to be present. This is confirmed by Google’s own headless setup flow and by technical guidance showing that Chrome is only needed on the client side or for the initial authorization, not on the host machine itself Super User. 

---

🧩 How it works (macOS version) 
Google’s “headless” setup lets you authorize and install the Chrome Remote Desktop Host service on a Mac without ever installing Chrome on that Mac. 
✔ What you do need 
• A Mac where you want the host installed 
• A different device (phone, tablet, or another computer) where you can sign into your Google account and visit the headless setup page 
 
✔ What you don’t need 
• Chrome installed on the Mac 
• Any Chromium browser on the Mac 
• Syncing your Google account to a browser on the Mac 
 
--- 
🛠️ Step‑by‑step (Mac) 
1. On another device, go to: 
https://remotedesktop.google.com/headless 
This page walks you through generating an authorization code for the Mac host Super User. 
2. On the Mac, download and install: 
Chrome Remote Desktop Host.pkg 
This is the standalone host service. It runs as a system daemon and does not require Chrome. 
3. Paste the authorization command 
The headless page gives you a command that looks like: 
/Applications/Chrome\ Remote\ Desktop\ Host.app/Contents/MacOS/Chrome\ Remote\ Desktop\ Host --code="AUTH_CODE" --redirect-url="https://remotedesktop.google.com/_/oauthredirect" --name="YOUR_MAC_NAME" 
 
This registers the Mac with your Google account. 
4. Set your PIN 
The host will prompt you to create a PIN for remote access. 
That’s it — the Mac is now a Chrome Remote Desktop host. 
--- 
🧠 Why this works 
Chrome Remote Desktop is technically two components: 
Component   Needs Chrome?     Purpose      
Host service (runs on the machine being controlled)   No    System daemon that handles remote connections    
Client UI (used to connect to the host)   Yes (or mobile app)     The browser/app you use to initiate the connection     
 
The host does not depend on Chrome — only the client does. 
This is why the headless setup exists and why the host installer is a standalone macOS package. 
--- 
🧭 Bottom line 
Yes — you can fully install and run Chrome Remote Desktop Host on macOS without Chrome or any Chromium browser. 
Use the headless setup flow and the standalone host installer, and you never need to sign into Chrome on the Mac. 
--- 
If you want, I can walk you through the exact macOS terminal commands and permissions prompts so you know exactly what to expect on Sonoma/Ventura. 

