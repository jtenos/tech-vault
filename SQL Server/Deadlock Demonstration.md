## Setup

```sql
create table dbo.Bar
(
		id int identity not null primary key
		,name nvarchar(10)
);
go
create table dbo.Foo
(
		id int identity not null primary key
		,name nvarchar(10)
);
go


insert dbo.Bar (name) values ('a');
insert dbo.Foo (name) values ('b');

go

create procedure dbo.Proc1
as
begin
		--set transaction isolation level snapshot;
		begin tran;
				update dbo.Foo set name = 'aa';
				waitfor delay '00:00:05';
				select * from dbo.Bar;
		commit tran;
end;
go
create procedure dbo.Proc2
as
begin
		--set transaction isolation level snapshot;
		select * from dbo.Foo;
end;
go
create procedure dbo.Proc3
as
begin
		--set transaction isolation level snapshot;
		begin tran;
				update dbo.Bar set name = 'bb';
				select * from dbo.Foo;
		commit tran;
end;
go
```

## Client application
```csharp
var conn1 = new SqlConnection(cs);
conn1.Open();
var comm1 = conn1.CreateCommand();
comm1.CommandText = "Proc1;";
new Thread(() => {
	try {
		Console.WriteLine("Executing comm1");
		comm1.ExecuteNonQuery();
		Console.WriteLine("comm1 done");
	} catch (Exception ex) {
		Console.WriteLine("comm1: " + ex);
	}
 }).Start();


var conn2 = new SqlConnection(cs);
conn2.Open();
var comm2 = conn2.CreateCommand();
comm2.CommandText = "Proc2";
new Thread(() => {
	try {
		Console.WriteLine("Executing comm2");
		comm2.ExecuteNonQuery();
		Console.WriteLine("comm2 done");
	} catch (Exception ex) {
		Console.WriteLine("comm2: " + ex);
	}
}).Start();


var conn3 = new SqlConnection(cs);
conn3.Open();
var comm3 = conn3.CreateCommand();
comm3.CommandText = "Proc3;";
new Thread(() => {
	try {
		Console.WriteLine("Executing comm3");
		comm3.ExecuteNonQuery();
		Console.WriteLine("comm3 done");
	} catch (Exception ex) {
		Console.WriteLine("comm3: " + ex);
	}
}).Start();

Console.Read();
```

## SSMS Window 1
```sql
begin tran;
update dbo.Foo set name = 'aa'
```

## SSMS Window 2
```sql
select * from dbo.Foo;
```

## SSMS Window 3
```sql
begin tran;
update dbo.Bar set name = 'bb';
select * from dbo.Foo;

select * from dbo.Bar;
```