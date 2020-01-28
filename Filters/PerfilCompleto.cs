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
    public class PerfilCompleto : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HelperFunctions.EstaAutenticado(context.HttpContext))
            {
                bool redirect = false;
                var NoRedirect = false;
                var db = context.HttpContext.RequestServices.GetRequiredService<HealthUpContext>();
                // se ja estiver na pagina, para nao causar loops infinitos de redirecionamento!
                if (context.HttpContext.Request.Path == "/Utilizadores/CompletarPerfil")
                {
                    NoRedirect = true;
                }
                // deixar utilizador dar logout
                if (context.HttpContext.Request.Path == "/Utilizadores/Logout")
                {
                    NoRedirect = true;
                }

                if (!NoRedirect)
                {
                    if (HelperFunctions.IsCurrentUserProfessor(context.HttpContext))
                    {
                        var professor = db.Professores.Include(p => p.NumProfessorNavigation).SingleOrDefault(p => p.NumCC == context.HttpContext.Session.GetString("UserId"));
                        if (String.IsNullOrWhiteSpace(professor.Especialidade))
                        {
                            redirect = true;
                        }
                    }
                    if (HelperFunctions.IsCurrentUserSocio(context.HttpContext))
                    {
                        var socio = db.Socios.Include(s => s.NumSocioNavigation).SingleOrDefault(p => p.NumCC == context.HttpContext.Session.GetString("UserId"));
                        if (socio.Peso == null || socio.Altura == null)
                        {
                            redirect = true;
                        }

                    }
                }
                
                if (redirect)
                {
                    var values = new RouteValueDictionary(new
                    {
                        action = "CompletarPerfil",
                        controller = "Utilizadores"
                    });
                    context.Result = new RedirectToRouteResult(values);
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
