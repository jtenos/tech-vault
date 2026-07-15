```csharp
using Microsoft.Data.SqlClient;
using System.Data;

const string CONNECTION_STRING = @"Data Source=(localdb)\MSSqlLocalDB;Initial Catalog=sandbox;Integrated Security=SSPI;";

await AddPersonRecordsAsync();
await AddCourseRecordsAsync();

async Task AddPersonRecordsAsync()
{
    /*
        CREATE TABLE dbo.People (
            ID INT NOT NULL IDENTITY PRIMARY KEY
            ,FirstName NVARCHAR(100) NOT NULL
            ,LastName NVARCHAR(100) NOT NULL
        );
    */
    DataTable personTable = new(tableName: "dbo.People");
    personTable.Columns.Add("FirstName", typeof(string));
    personTable.Columns.Add("LastName", typeof(string));

    DataRow row = personTable.NewRow();
    row["FirstName"] = "John";
    row["LastName"] = "Doe";
    personTable.Rows.Add(row);

    row = personTable.NewRow();
    row["FirstName"] = "Jane";
    row["LastName"] = "Smith";
    personTable.Rows.Add(row);

    // For a table with identity:
    using SqlBulkCopy bulkCopy = new(CONNECTION_STRING, SqlBulkCopyOptions.KeepIdentity)
    {
        DestinationTableName = personTable.TableName
    };
    foreach (DataColumn col in personTable.Columns)
    {
        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
    }
    await bulkCopy.WriteToServerAsync(personTable);
}

async Task AddCourseRecordsAsync()
{
    /*
        CREATE TABLE dbo.Courses (
            CourseKey VARCHAR(100) NOT NULL
            ,CourseDescription VARCHAR(100) NULL
        );
    */
    DataTable courseTable = new(tableName: "dbo.Courses");
    courseTable.Columns.Add("CourseKey", typeof(string));
    courseTable.Columns.Add("CourseDescription", typeof(string));

    DataRow row = courseTable.NewRow();
    row["CourseKey"] = "ENG101";
    row["CourseDescription"] = "Intro to English";
    courseTable.Rows.Add(row);

    // The CourseDescription column allows null
    row = courseTable.NewRow();
    row["CourseKey"] = "ZZZ999";
    courseTable.Rows.Add(row);

    using SqlBulkCopy bulkCopy = new(CONNECTION_STRING)
    {
        DestinationTableName = courseTable.TableName
    };
    foreach (DataColumn col in courseTable.Columns)
    {
        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
    }
    await bulkCopy.WriteToServerAsync(courseTable);
}
```
