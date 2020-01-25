using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Filters
{
    public class AjaxOnly : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var Header = context.HttpContext.Request.Headers["X-Requested-With"];

            if (Header != "XMLHttpRequest")
            {
                var values = new RouteValueDictionary(new
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
