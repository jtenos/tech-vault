If project is cloned in WSL but editing in Visual Studio in Windows, make the following changes:

## .editorconfig
```toml
root = true

# Apply LF line endings to all files
[*]
end_of_line = lf
insert_final_newline = true
charset = utf-8
```

## .gitattributes
```
* text=auto eol=lf
```

## Solution file

.sln files still need to be CRLF to work - everything else should be ok with LF.

Convert .sln to .slnx and use that instead.
