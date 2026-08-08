Identifying mouse information 

To identify the mouse identification number, use these steps: 
- Open Start.
- Search for Device Manager, click the top result to open the app. 
- Expand the Mice and other pointing devices branch. 
- Right-click the active mouse and select the Properties option. 
- Click on the Details tab. 
- Use the "Property" drop-down menu and choose the Device instance path option. 
- Confirm the "VID ID" value for the mouse — for example, VID_0E0F&PID_0003&MI_01. 
Reverse mouse scrolling direction 

To use the Registry to reverse the mouse scrolling direction, use these steps: 

- Open Start. 
- Search for regedit and click the top result to open the Registry Editor.
- Browse the following path: 
- Expand the key (folder) that matches the "VID ID" of your mouse — for example, VID_0E0F&PID_0003&MI_01. 
- Expand the available key. 
- Select the Device Parameters key. 
- Double-click the FlipFlopWheel DWORD and set the value from 0 to 1. 
- Click OK.
- Restart your computer. 
