using HealthUp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace HealthUp.Filters
{
    public class NaoAutenticado : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HelperFunctions.EstaAutenticado(context.HttpContext))
            {
                RouteValueDictionary values = new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Home",
                });
                context.Result = new RedirectToRouteResult(values);
                base.OnActionExecuting(context);
            }

        }
    }
}
