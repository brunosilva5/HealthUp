using HealthUp.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Filters
{
    public class ApagarAulasAntigas : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var db = context.HttpContext.RequestServices.GetRequiredService<HealthUpContext>();
            var AulasAntigas = db.Aulas.Where(a => a.ValidoAte < DateTime.Now);
            db.Aulas.RemoveRange(AulasAntigas);
            db.SaveChanges();
            base.OnActionExecuting(context);
        }
    }
}
