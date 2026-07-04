## The Cause:
Windows User Account Control (UAC) or the network stack. When you are fully logged out, Windows drops back to the Login Screen. If CRD was not installed with elevated administrative privileges, or if the "Chrome Remote Desktop Service" is blocked from interacting with the Windows secure login desktop (Winlogon), the service loses the ability to capture the screen or communicate over the network.

## The Fix:
- Edit the Chrome Remote Desktop service.\
	- Startup type: Automatic
	- Log On: Check "Allow service to interact with desktop"
- Turn off all of the "sleep", "hibernate", "shut off monitor", etc. in settings
- Hidden system unattended sleep timeout setting
	- (As Admin): `powercfg -attributes SUB_SLEEP 7bc4a2f9-d8fc-4469-b07b-33eb785aaca0 -ATTRIB_HIDE`
	- Open classic control panel: Power Options -> Change plan settings -> Change Advanced power settings -> Sleep -> System unattended sleep timeout
		- 0 minutes (never)