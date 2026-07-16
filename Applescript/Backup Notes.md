```applescript
use scripting additions

on run
	set rootDir to "/Users/jtenos/Downloads" as string
	set folderDestination to my (POSIX file rootDir)
	set dtFormatted to do shell script "date +'%Y-%m-%d %H_%M_%S'"
	
	tell application "Finder"
		make new folder at folder folderDestination with properties {name:"Notes " & dtFormatted}
	end tell
	
	set fullPath to rootDir & "/" & "Notes " & dtFormatted
	set quotedFullPath to quoted form of fullPath
	set readmeFileName to "readme.txt"
	set the targetFile to fullPath & "/" & readmeFileName
	set the openTargetFile to open for access targetFile with write permission
	set eof of the openTargetFile to 0
	set theContents to "HTML version only - attachments are not saved here. To save attachments, use the BackupNotesDB application which saves the entire database and all attachments"
	write theContents to the openTargetFile starting at eof
	close access the openTargetFile
	
	tell application "Notes"
		repeat with theNote in notes
			set nm to name of theNote
			
			set newFolders to {}
			
			try
				set curr to theNote
				repeat while true
					set par to container of curr
					copy name of par to end of newFolders
					set curr to par
				end repeat
				
			end try
			
			set newFolders to reverse of newFolders
			set newFoldersSafe to {}
			repeat with f in newFolders
				set safeName to my fixName(f)
				copy safeName to end of newFoldersSafe
			end repeat
			
			# Remove the first one (iCloud)
			set newFoldersSafe to newFoldersSafe's (items 2 through -1)
			
			set subFolder to ""
			repeat with f in newFoldersSafe
				set subFolder to subFolder & "/" & f
			end repeat
			log "Creating " & "Notes " & dtFormatted & subFolder
			set fullPath to rootDir & "/" & "Notes " & dtFormatted & subFolder
			set quotedFullPath to quoted form of fullPath
			do shell script "mkdir -p " & quotedFullPath
			
			set noteFileName to my makeNoteFileName(nm)
			log "Saving file " & noteFileName
			
			set the targetFile to fullPath & "/" & noteFileName
			log targetFile
			set the openTargetFile to open for access targetFile with write permission
			set eof of the openTargetFile to 0
			
			#set theContents to "<h1>" & nm & "</h1>"
			set theContents to ""
			
			set theBody to body of theNote
			#set theBody to findAndReplaceInText(theBody, " ", "'")
			#set theBody to findAndReplaceInText(theBody, " ", "\"")
			#set theBody to findAndReplaceInText(theBody, " ", "\"")
			#set theBody to findAndReplaceInText(theBody, " ", "-")
			
			set theContents to theContents & theBody
			
			write theContents to the openTargetFile starting at eof
			close access the openTargetFile
			
		end repeat
	end tell
	
end run

# Replaces all characters except letters, numbers, hyphen, underscore, and space with underscores
on fixName(nm)
	set validChars to "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_ "
	set res to ""
	
	repeat with ch in nm
		set theOffset to offset of ch in validChars
		if theOffset is not 0 then
			set res to res & ch
		else
			set res to res & "_"
		end if
	end repeat
	return res
end fixName

# Makes a filename for a given note name by putting a random integer on the file,
# in case of duplicate named notes
on makeNoteFileName(nm)
	set r to (random number from 1000 to 9999) as text
	return fixName(nm) & "_" & r & ".html"
end makeNoteFileName

# https://developer.apple.com/library/archive/documentation/LanguagesUtilities/Conceptual/MacAutomationScriptingGuide/ManipulateText.html
on findAndReplaceInText(theText, theSearchString, theReplacementString)
	set AppleScript's text item delimiters to theSearchString
	set theTextItems to every text item of theText
	set AppleScript's text item delimiters to theReplacementString
	set theText to theTextItems as string
	set AppleScript's text item delimiters to ""
	return theText
end findAndReplaceInText
```