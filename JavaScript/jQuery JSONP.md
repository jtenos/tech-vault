```javascript
function go() {
    jQuery.ajax({
        // url: "/Home/DoSomething", // Regular MVC
        // url: "/Handler1.ashx", // WebForms
        data: {xyz:"World"},
        dataType: "jsonp",
        type: "get"
    }).done(function (result) {
        console.log(result);
    });
}

// Regular MVC - no special filters, return types, etc. 
public ActionResult DoSomething(string xyz) 
{
    Response.ContentType = "application/json";
    var foo = new { FirstName = "John", LastName = "Doe", Message = "Hello " + xyz };
    Response.Write(string.Format("{0}({1});", Request["callback"], JsonConvert.SerializeObject(foo)));
    return null;
}
 
// WebForms
public class Handler1 : IHttpHandler 
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var foo = new { FirstName = "John", LastName = "Doe", Message = "Hello " + context.Request["xyz"] };
        context.Response.Write(string.Format("{0}({1});", context.Request["callback"], JsonConvert.SerializeObject(foo)));
    }
 
    public bool IsReusable { get { return false; } }
}
```