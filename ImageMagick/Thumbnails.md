```bash
mkdir thumbs
cp *.jpg thumbs/
cd thumbs/
mogrify -quality 25 -resize 400 *.jpg
for v in *.jpg; do mv $v 400-$v; done
cd ..
for v in *.jpg; do echo \<a href=\"$v\"\>\<img src=\"thumbs/400-$v\" /\>\</a\>\<br /\>; done
```