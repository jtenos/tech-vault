```csharp
/*
In ASP.NET Core, if you want to render a view to a string, this Stack Overflow answer 
https://stackoverflow.com/a/50024209 makes it simple. I’ve used this to return HTML in a JSON request 
that also includes other data, to build an email body, and to get raw HTML to pass off to wkhtmltopdf 
to build PDF from HTML.
*/

public static async Task<string> RenderViewAsync<TModel>(this Controller controller, 
    string viewName, TModel model, bool partial = false) {
    if (string.IsNullOrEmpty(viewName)) {
        viewName = controller.ControllerContext.ActionDescriptor.ActionName;
    }
 
    controller.ViewData.Model = model;
 
    using (var writer = new StringWriter()) {
        IViewEngine viewEngine = controller.HttpContext.RequestServices
            .GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
        ViewEngineResult viewResult = viewEngine.FindView(
            controller.ControllerContext, viewName, !partial);
 
        if (viewResult.Success == false) {
            return $"A view with the name {viewName} could not be found";
        }
 
        ViewContext viewContext = new ViewContext(controller.ControllerContext,
            viewResult.View, controller.ViewData, controller.TempData,
            writer, new HtmlHelperOptions()
        );
 
        await viewResult.View.RenderAsync(viewContext);
 
        return writer.GetStringBuilder().ToString();
    }
}
```