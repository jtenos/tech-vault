```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
 
namespace Acme.Web {
    /// <summary>
    /// You must initialize this in order to use it.
    /// Generally you'll want to initialize in your Global.asax.cs
    /// Application_Start method, by calling:    
    /// ActionListController.Initialize(typeof(HomeController).Assembly);
    /// If you have controllers in multiple assemblies,
    /// pass in an array of assemblies to the Initialize method.
    /// </summary>
    public class ActionListController : Controller {
        private static Assembly[] _controllerAssemblies = new Assembly[0];
 
        public static void Initialize(params Assembly[] controllerAssemblies) {
            if (controllerAssemblies != null) {
                _controllerAssemblies = controllerAssemblies;
            }
        }
 
        [HttpGet]
        [OutputCache(Duration = 6000, VaryByParam = "none")]
        public ActionResult ControllerActionList() {
            var rootObjects = new Dictionary<string, object>();
 
            // Area -> Controller -> Action -> Url
            var areas = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
 
            // Controller -> Action -> Url
            var rootMethods = new Dictionary<string, Dictionary<string, string>>();
 
            foreach (var assembly in _controllerAssemblies) {
                foreach (var controller in assembly.GetTypes().Where(t => typeof (Controller).IsAssignableFrom(t))) {
                    string controllerName = Regex.Replace(controller.Name, "Controller$", string.Empty);
                    var areaNameMatch = Regex.Match(controller.Namespace, @"(?<=\.Areas\.)[a-zA-Z0-9_]+");
                    if (areaNameMatch.Success) {
                        if (!areas.ContainsKey(areaNameMatch.Value)) {
                            areas[areaNameMatch.Value] = new Dictionary<string, Dictionary<string, string>>();
                        }
                        areas[areaNameMatch.Value].Add(controllerName, GetActionsFromController(controller, new {Area = areaNameMatch.Value}));
                    } else {
                        rootMethods.Add(controllerName, GetActionsFromController(controller));
                    }
                }
            }
            foreach (var kvp in areas) {
                rootObjects[kvp.Key] = kvp.Value;
            }
            foreach (var kvp in rootMethods) {
                rootObjects[kvp.Key] = kvp.Value;
            }
            string script = string.Format("window._controllerActions={0};", JsonConvert.SerializeObject(rootObjects).Replace(@"/", @"\/"));
            return Content(script, "text/javascript");
        }
 
        private Dictionary<string, string> GetActionsFromController(Type controller, object routeValue = null) {
            var controllerActions = new Dictionary<string, string>();
            string controllerName = Regex.Replace(controller.Name, "Controller$", string.Empty);
 
            var controllerMethods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            var syncActionMethods = controllerMethods.Where(m => typeof (ActionResult).IsAssignableFrom(m.ReturnType));
            var asyncActionMethods = controllerMethods
                .Where(m => m.ReturnType.IsGenericType
                    && m.ReturnType.GetGenericTypeDefinition() == typeof (Task<>)
                    && typeof (ActionResult).IsAssignableFrom(m.ReturnType.GetGenericArguments()[0]));
 
            foreach (var method in syncActionMethods.Union(asyncActionMethods)) {
                controllerActions[method.Name] = Url.Action(method.Name, controllerName, routeValue);
            }
            return controllerActions;
        }
    }
}

/**
In your layout page, make sure you put that script tag above all others.

<script src="@Url.Action("ControllerActionList", "ActionList", new {v = "1.0"})"></script>

When you’re ready to use it, simply call:

var url = _controllerActions.People.SavePerson;
**/
```