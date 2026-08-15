```csharp
public void HandleException(IApplicationBuilder app)
{
  app.Run(async requestContext =>
  {
    try
    {
      string path = requestContext.Request.Path;
      var exceptionHandlerPathFeature = requestContext.Features.Get<IExceptionHandlerPathFeature>();
      var exception = exceptionHandlerPathFeature?.Error;

      User? user = null;
      try
      {
        user = /* implementation */ Cast requestContext.User into your custom user type
      }
      catch (Exception ex) { Debug.WriteLine(ex); }

      if (exception != null)
      {
        new Thread(() =>
        {
          // Using a new thread to escape any open transactions that are getting rolled back

          try
          {
            using var conn = new SqlConnection(Configuration.GetConnectionString("connstr"));
            conn.Open();
            using var comm = conn.CreateCommand();
            comm.CommandText = @"insert dbo.ExceptionLog () values (); // whatever...";
            comm.Parameters.AddRange(new[] {
              // exception.Message, exception.ToString(), path, user, whatever else...
            });
            comm.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
            Debug.WriteLine(ex.ToString());
          }
        }).Start();
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine(ex);
    }
    await Task.CompletedTask;
  });
}

app.UseExceptionHandler(HandleException);
```