Yes, you can configure Chrome to always open new links in the default profile, regardless of which profile you last used. This requires an additional step to ensure Windows and Chrome are properly configured.

---

### How to Make Links Open in the Default Profile

By default, Chrome will open links in the most recently active profile window. To override this and force all external links to open in the **Default** profile, you can use the command line.

1. **Change the Default Chrome Shortcut:** Edit the properties of your primary Chrome shortcut (the one not tied to a specific profile) to include the \-profile-directory="Default" argument.
   * Find the shortcut you use to open your main Chrome browser (likely pinned to the taskbar or on your desktop).
   * Right-click the shortcut and select **Properties**.
   * In the **Target** field, add \-profile-directory="Default" after the closing quotation mark of the executable path.
   * The target should look like this:
   * "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe" \--profile-directory="Default"
   * Click **Apply** and then **OK**.
1. **Ensure Chrome is the Default Browser:** Windows must be configured to use this specific Chrome shortcut as the default program for web links.
   * Navigate to **Settings** \> **Apps** \> **Default apps**.
   * Under "Web browser," ensure **Google Chrome** is selected.

This method ensures that when you click a link from an application like Outlook, Word, or another program, Windows will launch the chrome.exe executable with the added command-line argument, forcing it to open in the **Default** profile. The other profile-specific shortcuts you created will still function independently, allowing you to have multiple Chrome windows open at once, each with a different profile.

---

Yes, you can pin shortcuts for Chrome apps associated with different profiles to the Windows taskbar.1 Here's a method to achieve this.

### Creating and Pinning the Shortcuts

1. **Locate the Chrome Application:** First, navigate to the folder where Chrome is installed. The default location is C:\\Program Files\\Google\\Chrome\\Application.
2. **Create a Desktop Shortcut:** Inside the Chrome application folder, locate chrome.exe. Right-click on it, select **Send to**, and then click **Desktop (create shortcut)**.
3. **Find the Profile Directory:** Open File Explorer and enter %LocalAppData%\\Google\\Chrome\\User Data in the address bar. This is where all your Chrome profiles are stored, usually in folders named **Default**, **Profile 1**, **Profile 2**, etc.
4. **Edit the Shortcut's Properties:** Right-click on the new desktop shortcut and select **Properties**.
5. **Modify the Target Field:** You'll need to add a command-line argument to the "Target" field to specify the correct profile. The argument is \-profile-directory="Profile X", where "Profile X" is the name of the profile folder you found in the previous step.
   * Example for a Profile Named "Profile 1":
   * The target path would look like this:
   * "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe" \--profile-directory="Profile 1"
   * Example for the "Default" Profile:
   * The target path would look like this:
   * "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe" \--profile-directory="Default"
* You can also add other command-line arguments to open a specific URL or app directly. For example, to open Gmail for a specific profile, the target would be:
* "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe" \--profile-directory="Profile 1" \--app=[https://mail.google.com](https://mail.google.com/)
* This creates a shortcut that opens a new window for the specified profile directly to Gmail.
1. **Pin to Taskbar:** Click **Apply** and then **OK** to save the changes. You can then drag the modified shortcut from your desktop and pin it to the taskbar. Repeat this process for each profile you wish to pin. To differentiate them, you may want to change the shortcut's icon. You can do this by clicking the **Change Icon...** button in the shortcut's properties and choosing a different icon.

---

### Setting a Default Profile

To ensure that clicking links from other applications opens in your **default Chrome profile**, you need to ensure that the default browser setting in Windows points to your regular Chrome installation, not a specific profile shortcut. The profile is an internal setting that only applies when you launch Chrome via a specific shortcut. When Windows opens a link from an external application, it uses the standard, default chrome.exe executable, which opens whatever profile was last in use or your designated default. If you follow the above steps, the behavior of links from other applications should remain unaffected. If you find this is not the case, you'll need to change the default browser settings back to Google Chrome.