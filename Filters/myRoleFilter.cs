using HealthUp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Filters
{
    public class MyRoleFilter : ActionFilterAttribute
    {
        public string Perfil { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HelperFunctions.EstaAutenticado(context.HttpContext))
            {
                if (context.HttpContext.Session.GetString("Role") == Perfil)
                    base.OnActionExecuting(context);
                else
                {
                    Controller c = (context.Controller as Controller);
                    c.ViewData["mensagem"] = "Necessita de ter Perfil " + Perfil;
                    context.Result = new ViewResult { StatusCode = 401, ViewName = "Erro", ViewData = c.ViewData };
                }
            }
            else
            {
                Controller c = (context.Controller as Controller);
                c.ViewData["mensagem"] = "Necessita de estar autenticado";
                context.Result = new ViewResult { StatusCode = 401, ViewName = "Erro", ViewData = c.ViewData };
            }
        }
    }
}