```applescript
(* Batch change the time of selected photos all at once 

How to use this script:
- Collect all photos you want to set to the same time in an album 
- ﻿and sort them manually
- Adjust the time of the first of your batch of photos with 
  ﻿"Adjust Date and time" in Photos from the "Image" menu, 
 even if the time is correct
- Select first the photo with the adjusted date, 
- ﻿the hold down the Shift key and select all photos you want 
  ﻿to set to the same date and time at once. 
- ﻿You need to select at least two photos.
- Open this script and run it by pressing the "Run" button.
- The script will copy the date from the first image 
  and set all photos to the same date, 
  ﻿and step the date by adding the increment 
  given by the variable timeIncrement.
- The script will return the last date it changed

- if you save this script as an Application 
  you can add it to the Dock and run it from there

Note - for some photos it may not be possible to change the time, 
﻿then running "Adjust date and time" for all photos once may help. 
﻿Photos has a bug that displays timezones incorrectly, 
﻿so the results may look wrong, 
﻿if the photos have been taken in different timezones.

This script has been tested in Photos version 1.0 to 4.0, 
﻿with MacOS X 10.10.3 - macOS 10.14.2
 
© Léonie
*)


set timeIncrement to 1 -- the time increment in minutes
(* select at least 2 images in Photos *)
tell application "Photos"
	activate
	set imageSel to (get selection)
	if (imageSel is {}) or (the length of imageSel < 2) then
		error "Please select at least two images."
	else
		
		set first_image to item 1 of imageSel
		set first_date to (the date of first_image) as date
		repeat with i from 2 to count of imageSel
			
			set next_image to item i of imageSel
			set next_date to (the date of next_image) as date
			
			set time_difference to (first_date - next_date) ¬
				+ ((i - 1) * timeIncrement * minutes)
			--	return time_difference -- testing
			
			tell next_image
				set the date of next_image to (next_date + time_difference) as date
			end tell
			
		end repeat
	end if
	return "Adjusted the date and time of " & (the length of imageSel) ¬
		& " photos. The last date is " & ((the date of next_image) as date)
end tell

```