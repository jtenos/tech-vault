# GLOBAL LIBRARIES

You can run npm list -g to see where global libraries are installed. On Unix systems they are normally placed in /usr/local/lib/node or /usr/local/lib/node_modules when installed globally. If you set the NODE_PATH environment variable to this path, the modules can be found by node.

# Windows XP:

`%USERPROFILE%\Application Data\npm\node_modules`

# Windows 7:

`%AppData%\npm\node_modules`

# NON-GLOBAL LIBRARIES

Non-global libraries are installed the node_modules sub folder in the folder you are currently in. You can run npm list to see the installed non-global libraries for your current location.

# REFERENCE

Answer copied from this Stackoverflow question
https://stackoverflow.com/a/5926706
