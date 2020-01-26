using HealthUp.Data;
using HealthUp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Filters
{
    public class Socios_PTs_Filter : ActionFilterAttribute
    {
        public bool DeixarAcederSeTiver { get; set; } = false;
        public string Pessoa { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
                 
            var db = context.HttpContext.RequestServices.GetRequiredService<HealthUpContext>();

            bool redirect = false;
            if (HelperFunctions.NormalizeWhiteSpace(Pessoa)=="Socio")
            {
                if (HelperFunctions.IsCurrentUserSocio(context.HttpContext))
                {
                    if (HelperFunctions.DoesSocioHavePT(context.HttpContext) && DeixarAcederSeTiver == true)
                    {
                        redirect = false;
                    }
                    
                    else
                    {
                        redirect = true;
                    }
                }
            }
            if (HelperFunctions.NormalizeWhiteSpace(Pessoa) == "Professor")
            {
                if (HelperFunctions.IsCurrentUserProfessor(context.HttpContext))
                {
                    if (HelperFunctions.DoesProfHaveStudents(context.HttpContext) && DeixarAcederSeTiver == true)
                    {
                        redirect = false;
                    }
                    else
                    {
                        redirect = true;
                    }
                }
            }

            if (redirect)
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

