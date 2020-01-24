using HealthUp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Filters
{
    public class Autenticado : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!HelperFunctions.EstaAutenticado(context.HttpContext))
            {
                var values = new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "Utilizadores",
                });
                context.Result = new RedirectToRouteResult(values);
                base.OnActionExecuting(context);
            }

        }
    }
}
