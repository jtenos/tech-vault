Run this right after opening the connection:

```shell
conn.Query("PRAGMA journal_mode=WAL;");
```