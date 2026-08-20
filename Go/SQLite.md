```go
package main


import (
"context"
"database/sql"
"fmt"
"log"
"sync"
"time"


"github.com/jmoiron/sqlx"
_ "modernc.org/sqlite"
)


func main() {
db, err := sqlx.Open("sqlite", "/projects/GoSqlite2/mydb.sqlite3?_journal=WAL&_timeout=5000&_fk=true")


// This prevents the SQLITE_BUSY errors, but I don't know if this is the only approach
db.SetMaxOpenConns(1)
if err != nil {
log.Fatal(err.Error())
}
ctx, cancel := context.WithTimeout(context.Background(), 10*time.Second)
defer cancel()
db.ExecContext(ctx, "DROP TABLE Foo;")
r, err := db.ExecContext(ctx, "CREATE TABLE Foo([ID] INTEGER NOT NULL PRIMARY KEY, [Message] TEXT NOT NULL);")
if err != nil {
fmt.Println(err.Error())
}
fmt.Println(r)


var wg sync.WaitGroup


for i := 0; i < 1000; i++ {
wg.Add(1)
go func(i int) {
_, err := db.ExecContext(ctx, "INSERT INTO Foo ([Message]) VALUES (:Message)", sql.Named("Message", fmt.Sprintf("Message number %d", i)))
if err != nil {
fmt.Println(err.Error())
}
wg.Done()
}(i)
}


type Foo struct {
ID      int64  `db:"ID"`
Message string `db:"Message"`
}


wg.Wait()


var foos []Foo
err = db.SelectContext(ctx, &foos, "SELECT * FROM Foo;")
if err != nil {
fmt.Println(err.Error())
}
for i := range foos {
fmt.Println(foos[i])
}
}
```