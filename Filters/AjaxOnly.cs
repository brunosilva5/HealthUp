using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace HealthUp.Filters
{
    public class AjaxOnly : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Microsoft.Extensions.Primitives.StringValues Header = context.HttpContext.Request.Headers["X-Requested-With"];

            if (Header != "XMLHttpRequest")
            {
                RouteValueDictionary values = new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Home"
                });
                context.Result = new RedirectToRouteResult(values);
            }
            base.OnActionExecuting(context);
        }
    }
}
