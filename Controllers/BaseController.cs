using HealthUp.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HealthUp.Controllers
{
    [ApagarAulasAntigas]
    [PerfilCompleto]
    public class BaseController : Controller
    {
        // este controller é a base de todos os controllers, este filtro vai ser aplicado a todos os controllers
    }
}