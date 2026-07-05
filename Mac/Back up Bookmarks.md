```bash
#!/bin/zsh

user=$(whoami)
input="/Users/$user/Library/Safari/Bookmarks.plist"
now="$(date +%Y)$(date +%m)$(date +%d)$(date +%H)$(date +%M)$(date +%S)"
output="/Users/$user/Library/Mobile Documents/com~apple~CloudDocs/ZZ_temp/bak/bookmarks/bookmarks-$now.plist"

cp $input $output
plutil -convert xml1 $output
gzip $output
```