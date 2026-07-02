```sql
create procedure util.SendEmailWithQueryContents
(
    @DBMailProfileName sysname
    ,@SemiColonSeparatedEmails varchar(max)
    ,@Subject nvarchar(255)
    ,@Body nvarchar(max)
    ,@QueryText nvarchar(max)
    ,@DatabaseName sysname
    ,@AttachmentFileName nvarchar(255)
    ,@IncludeHeader bit = 1
    ,@Delimiter char(1) = ','
    ,@IsBodyHtml bit = 0
)
as
begin
    declare @BodyFormat varchar(20);

    if @IsBodyHtml = 1
        set @BodyFormat = 'HTML';
    else
        set @BodyFormat = 'TEXT';

    exec msdb.dbo.sp_send_dbmail
      @profile_name = @DBMailProfileName
      ,@recipients = @SemiColonSeparatedEmails
      ,@subject = @Subject
      ,@body = @Body
      ,@query = @QueryText
      ,@execute_query_database = @DatabaseName
      ,@attach_query_result_as_file = 1
      ,@query_attachment_filename = @AttachmentFileName
      ,@query_result_header = @IncludeHeader
      ,@query_result_separator = @Delimiter
      ,@query_result_no_padding = 1
      ,@body_format = @BodyFormat;
end;
go
```