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
using System.Globalization;
using HealthUp.Helpers;

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
            var gym = _context.Ginasios.SingleOrDefault();

            string[] coordenadas = gym.LocalizacaoGps.Split(',');
            ViewBag.Latitude = coordenadas[0];
            ViewBag.Longitude = coordenadas[1];
            return View(gym);
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
            var gym = _context.Ginasios.SingleOrDefault();

            string[] coordenadas = gym.LocalizacaoGps.Split(',');
            ViewBag.Latitude = coordenadas[0];
            ViewBag.Longitude = coordenadas[1];

            return View(gym);
        }

        public IActionResult MapaAulas()
        {
            List<Aula> Listafinal = new List<Aula>();
            var Aulas = _context.Aulas.ToList();
            foreach (var item in Aulas)
            {
                var datas_aulas_semanaAtual=item.GetAulasInCurrentWeek();
                foreach (var subitem in datas_aulas_semanaAtual)
                {
                   
                }

            }
            return View(_context.Aulas.ToList());
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
