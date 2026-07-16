
Have pandoc installed

`sudo apt install pandoc`

Run this in terminal:

```bash
for inp in *.md; do pandoc --from markdown --to docx --output "${inp%.md}.docx" "$inp"; done
```