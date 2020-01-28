using HealthUp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace HealthUp.Filters
{
    public class MyRoleFilter : ActionFilterAttribute
    {
        public string Perfil { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HelperFunctions.EstaAutenticado(context.HttpContext))
            {
                bool allow = false;
                string[] Roles = Perfil.Split(",");
                string[] RolesNormalized = new string[Roles.Length];
                int i = 0;
                foreach (string item in Roles)
                {
                    RolesNormalized[i] = (HelperFunctions.NormalizeWhiteSpace(item));
                    i++;
                }
                foreach (string item in RolesNormalized)
                {
                    if (context.HttpContext.Session.GetString("Role") == item)
                    {
                        // O utilizador tem permissoes
                        allow = true;
                        base.OnActionExecuting(context);
                        break;
                    }
                }
                if (!allow)
                {
                    Controller c = (context.Controller as Controller);
                    c.ViewData["mensagem"] = "Não tem permissões suficientes para aceder a este recurso!";
                    context.Result = new ViewResult { StatusCode = 401, ViewName = "Erro", ViewData = c.ViewData };
                }
            }
            else
            {
                // Reencaminhamos para o login!
                RouteValueDictionary values = new RouteValueDictionary(new
                {
                    action = "Login",
                    controller = "Utilizadores"
                });
                context.Result = new RedirectToRouteResult(values);
                base.OnActionExecuting(context);
            }
        }
    }
}