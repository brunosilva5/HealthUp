using HealthUp.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace HealthUp.Filters
{
    public class ApagarAulasAntigas : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HealthUpContext db = context.HttpContext.RequestServices.GetRequiredService<HealthUpContext>();
            IQueryable<Models.Aula> AulasAntigas = db.Aulas.Where(a => a.ValidoAte < DateTime.Now);
            db.Aulas.RemoveRange(AulasAntigas);
            db.SaveChanges();
            base.OnActionExecuting(context);
        }
    }
}
