```go
package main

// https://learn.microsoft.com/en-us/azure/azure-sql/database/connect-query-go?view=azuresql
// https://github.com/microsoft/go-mssqldb

import (
	"time"
	_ "github.com/microsoft/go-mssqldb"
	"context"
	"database/sql"
	"log"
)

type tbl struct {
	sch    string
	tbl    string
	currDt time.Time
}

func (t *tbl) cols() []any {
	return []any{&t.sch, &t.tbl, &t.currDt}
}

func main() {
	cs := "Data Source=(local);Initial Catalog=sandbox;Integrated Security=SSPI;"
	db, err := sql.Open("sqlserver", cs)
	if err != nil {
		log.Fatal(err.Error())
	}
	defer db.Close()
	ctx := context.Background()
	err = db.PingContext(ctx)
	if err != nil {
		log.Fatal(err.Error())
	}
	log.Println("Connected successfully")
	
	
	tsql := "SELECT [TABLE_SCHEMA], [TABLE_NAME], GETDATE() [CurrentDate] FROM [INFORMATION_SCHEMA].[TABLES] ORDER BY [TABLE_SCHEMA], [TABLE_NAME];"
	stmt, err := db.PrepareContext(ctx, tsql)
	if err != nil {
		log.Fatal(err.Error())
	}
	defer stmt.Close()
	rows, err := stmt.QueryContext(ctx)
	if err != nil {
		log.Fatal(err.Error())
	}
	defer rows.Close()
	
	
	for rows.Next() {
		var t tbl
		err := rows.Scan(t.cols()...)
		if err != nil {
			log.Fatal(err.Error())
		}
		log.Printf("%s.%s - %v\n", t.sch, t.tbl, t.currDt)
	}
	
	
	stmt, err = db.Prepare(`
		IF OBJECT_ID('dbo.FooBarBaz') IS NOT NULL DROP TABLE dbo.FooBarBaz;
		CREATE TABLE dbo.FooBarBaz (ID INT NOT NULL IDENTITY PRIMARY KEY, [Val] NVARCHAR(100));
	`)
	if err != nil {
		log.Fatal(err.Error())
	}
	defer stmt.Close()
	stmt.ExecContext(ctx)
	
	stmt, err = db.Prepare(`
		INSERT INTO dbo.FooBarBaz ([Val]) VALUES (@Val);
	`)
	if err != nil {
		log.Fatal(err.Error())
	}
	defer stmt.Close()
	res, err := stmt.ExecContext(
		ctx,
		sql.Named("Val", "Hello"),
	)
	if err != nil {
		log.Fatal(err.Error())
	}
	log.Println(res)
}
```