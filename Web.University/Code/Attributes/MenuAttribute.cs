using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.University;

// Specifies the currently active menu

public class MenuAttribute(string menu) : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (filterContext.Controller is Controller controller)
            controller.ViewBag.Menu = menu;

        base.OnActionExecuting(filterContext);
    }
}
