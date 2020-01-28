using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HealthUp.Controllers
{
    [ApagarAulasAntigas]
    public class BaseController : Controller
    {
        // este controller é a base de todos os controllers, este filtro vai ser aplicado a todos os controllers
    }
}