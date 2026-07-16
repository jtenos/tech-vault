```applescript
on run
 set posixPath to "/Users/clickingkeys/Desktop/Demo_1.jpg" # EXAMPLE OF A POSIX PATH 
 log "Example POSIX path"
 log posixPath
 
 set hfsFilePath to POSIX file posixPath # CONVERTS A POSIX PATH TO AN HFS FILE REFERENCE
 log "Example HFS file reference"
 log hfsFilePath
 
 set hfsPath to POSIX file posixPath as string # CONVERTS A POSIX PATH TO AN HFS FILE PATH
 log "Example HFS path"
 log hfsPath
 
 set aliasExample to hfsPath as alias
 log "Example Alias"
 log aliasExample
 
 set backToPosix to POSIX path of hfsPath # THIS WILL CONVERT AN HFS PATH TO A POSIX PATH
 log "Example POSIX path 2"
 log backToPosix
end run
```