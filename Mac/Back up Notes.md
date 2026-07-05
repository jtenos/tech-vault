## AppleScript
```applescript
use scripting additions

on run
	set rootDir to "/Users/myname/Downloads" as string
	set folderDestination to my (POSIX file rootDir)
	set dtFormatted to do shell script "date +'%Y-%m-%d %H_%M_%S'"
	
	tell application "Finder"
		make new folder at folder folderDestination with properties {name:dtFormatted}
	end tell
	
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
			log "Creating " & dtFormatted & subFolder
			set fullPath to rootDir & "/" & dtFormatted & subFolder
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
			#set theBody to findAndReplaceInText(theBody, "�", "'")
			#set theBody to findAndReplaceInText(theBody, "�", "\"")
			#set theBody to findAndReplaceInText(theBody, "�", "\"")
			#set theBody to findAndReplaceInText(theBody, "�", "-")
			
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

(*
	
on getParentFolderName(n)
/*
function writeToFile(fileName, contents) {
	const file = app.openForAccess(Path(fileName), { writePermission: true });
	app.setEof(file, { to: 0 });
	app.write(contents, { to: file });
	app.closeAccess(file);
}

for(let i=0; i<notes.length; i++) {
	const fn = `${notes[i].container().name()}__${notes[i].name()}`;
	
	const fileName = `${folder}/${makeFileName(fn)}`;
	let contents = "";
	contents += notes[i].name();
	contents += notes[i].body().replace(/�/g, "'").replace(/�/g, "\"").replace(/�/g, "").replace(/�/g, "-");
	contents += "<hr>";
	
	writeToFile(fileName, contents);
}


*)
```

## JavaScript
```javascript
const app = Application.currentApplication();
app.includeStandardAdditions = true;

const notesApp = Application('Notes');
notesApp.includeStandardAdditions = true;

const notes = notesApp.notes;

const folder = app.chooseFolder();

function makeFileName(s) {
	const dt = new Date().toISOString().replace(/\:/g, "_")
	const rnd = Math.floor(Math.random() * 10000)
	let name = ""
	for (let i = 0; i < s.length && i < 30; ++i) {
		if (/^[a-zA-Z0-9]$/.test(s.charAt(i))) {
			name += s.charAt(i)
		} else {
			name += "_"
		}
	}
	return `${name}_${rnd}.html`
}

for(let i=0; i<notes.length; i++) {
	const fn = `${notes[i].container().name()}__${notes[i].name()}`;
	
	const fileName = `${folder}/${makeFileName(fn)}`;
	const file = app.openForAccess(Path(fileName), { writePermission: true });
	app.setEof(file, { to: 0 });

	app.write(notes[i].name(), {to: file});
	app.write(notes[i].body().replace(/’/g, "'").replace(/“/g, "\"").replace(/”/g, "").replace(/–/g, "-"), {to: file});
	app.write("<hr>", {to: file});

	app.closeAccess(file);
}
```

## Raw Data
```bash
#!/bin/zsh

user=$(whoami)
input="/Users/$user/Library/Group Containers/group.com.apple.notes"
now="$(date +%Y)$(date +%m)$(date +%d)$(date +%H)$(date +%M)$(date +%S)"
output="/Users/$user/Library/Mobile Documents/com~apple~CloudDocs/ZZ_temp/bak/notes/notes-$now.tgz"

tar -czf $output $input

# OR:

now=$(date +'%Y-%m-%d')
echo "$now"

tar czvf /Users/$user/Downloads/Apple\ Notes\ $now.tgz ~/Library/Group\ Containers/group.com.apple.notes
```