using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HealthUp.Models;
using HealthUp.Filters;
using HealthUp.Data;

namespace HealthUp.Controllers
{
    [PerfilCompleto]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HealthUpContext _context;
        public HomeController(ILogger<HomeController> logger, HealthUpContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PedidoRegisto()
        {
            return View();
        }

        public IActionResult Sobrenos()
        {
            return View();
        }
        
        public IActionResult Contactos()
        {
            return View(_context.Ginasios.SingleOrDefault());
        }

        public IActionResult MapaAulas()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
