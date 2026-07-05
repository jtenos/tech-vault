```bash
#!/bin/zsh
  
user=$(whoami)
input="/Users/$user/Library/Mail"
now="$(date +%Y)$(date +%m)$(date +%d)$(date +%H)$(date +%M)$(date +%S)"
output="/Users/$user/Library/Mobile Documents/com~apple~CloudDocs/ZZ_temp/bak/mail/mail-$now.tgz"

tar -czf $output $input
```