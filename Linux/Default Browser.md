It seems that Mint is overwriting the default browser even when I tell it in the Preferred Applications setting.

I haven't confirmed, but it might be that the default-web-browser gets set, but the HTTP handlers don't, so they get reverted.

So far, setting all four values seems to make it stick.

Check default browser:

```bash
xdg-settings get default-web-browser
xdg-mime query default x-scheme-handler/http
xdg-mime query default x-scheme-handler/https
xdg-mime query default text/html
```

Set default browser to LinkLever:

```bash
xdg-settings set default-web-browser linklever
xdg-mime default linklever.desktop x-scheme-handler/http
xdg-mime default linklever.desktop x-scheme-handler/https
xdg-mime default linklever.desktop text/html
```
