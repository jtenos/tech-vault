```applescript
use scripting additions

on run
	set rootDir to "/Users/jtenos/Downloads" as string
	set folderDestination to my (POSIX file rootDir)
	set dtFormatted to do shell script "date +'%Y-%m-%d %H_%M_%S'"
	
	
	tell application "Finder"
		make new folder at folder folderDestination with properties {name:"Contacts " & dtFormatted}
	end tell
	
	set fullPath to rootDir & "/" & "Contacts " & dtFormatted
	set quotedFullPath to quoted form of fullPath
	
	tell application "Contacts"
		repeat with cardPerson in people
			#			set outFile to (open for access file
		end repeat
	end tell
	
end run

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

on makeContactFileName(nm)
	return fixName(nm) & ".html"
end makeContactFileName
```