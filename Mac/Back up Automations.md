```bash
#!/bin/zsh

user=$(whoami)
input="/Users/$user/Library/Workflows/Applications/Calendar"
now="$(date +%Y)$(date +%m)$(date +%d)$(date +%H)$(date +%M)$(date +%S)"
output="/Users/$user/__TO_COPY_TO_CLOUD/automations/automations-$now.tgz"

tar -czf $output $input
```