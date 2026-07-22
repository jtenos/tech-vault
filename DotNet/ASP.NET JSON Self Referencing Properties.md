```csharp
services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

public class Job
{
    public int JobID { get; set; }
    public Job ParentJob { get; set; }
}
  
[ApiController]
[Route("[controller]")]
public class JobController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Job> Get()
    {
        // var job1 = new Job { JobID = 1001, ParentJob = null };
        // var job2 = new Job { JobID = 1002, ParentJob = job1 };
        // return new[] { job1, job2 };
  
        var job1 = new Job { JobID = 1001, ParentJob = null };
        var job2 = new Job { JobID = 1002 };
        job2.ParentJob = job2;
        return new[] { job1, job2 };
    }
}
```