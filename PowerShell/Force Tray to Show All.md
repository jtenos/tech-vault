```powershell
# Run this one time as admin, and this will create
# a scheduled task which sets all the tray icons to always
# show, to avoid having to go into the settings all the time.

# The timer part is broken, so once the task is there, fix the timer on the scheduled task
# TODO: Fix the script

# Create the script
$script = @'
Get-ChildItem "HKCU:\Control Panel\NotifyIconSettings" |
  ForEach-Object {
    Set-ItemProperty -Path $_.PSPath -Name 'IsPromoted' -Type DWORD -Value 1 -ErrorAction SilentlyContinue
  }
'@

$scriptPath = "$env:APPDATA\ShowAllTrayIcons.ps1"
$script | Set-Content -Path $scriptPath

# Register a scheduled task: runs at logon + every 30 minutes
$action = New-ScheduledTaskAction -Execute "powershell.exe" `
  -Argument "-WindowStyle Hidden -ExecutionPolicy Bypass -File `"$scriptPath`""

$repeatTrigger = New-ScheduledTaskTrigger -Once -At (Get-Date)
$repeatTrigger.Repetition.Interval = "PT30M"
$repeatTrigger.Repetition.StopAtDurationEnd = $false

$triggers = @(
  $(New-ScheduledTaskTrigger -AtLogOn),
  $repeatTrigger
)

$settings = New-ScheduledTaskSettingsSet -ExecutionTimeLimit (New-TimeSpan -Minutes 1)

Register-ScheduledTask -TaskName "ShowAllTrayIcons" `
  -Action $action `
  -Trigger $triggers `
  -Settings $settings `
  -RunLevel Limited `
  -Force
```